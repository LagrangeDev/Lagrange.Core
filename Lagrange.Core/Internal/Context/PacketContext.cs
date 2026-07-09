using System.Collections.Concurrent;
using Lagrange.Core.Common;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Internal.Packets.Struct;
using Lagrange.Core.Services;

namespace Lagrange.Core.Internal.Context;

internal class PacketContext
{
    private readonly ConcurrentDictionary<int, SsoPacketValueTaskSource> _pendingTasks = new();

    private readonly BotKeystore _keystore;
    private readonly SsoPacker _ssoPacker;
    private readonly ServicePacker _servicePacker;

    internal readonly BotSignProvider SignProvider;

    private readonly BotContext _context;

    public PacketContext(BotContext context)
    {
        _context = context;
        _keystore = context.Keystore;
        _ssoPacker = new SsoPacker(context);
        _servicePacker = new ServicePacker(context);

        SignProvider = context.Config.SignProvider ?? context.Config.Protocol switch
        {
            Protocols.Linux or Protocols.Windows or Protocols.MacOs => new DefaultBotSignProvider(),
            Protocols.AndroidPhone or Protocols.AndroidPad => new DefaultAndroidBotSignProvider(),
            _ => throw new ArgumentOutOfRangeException(nameof(context.Config.Protocol))
        };

        SignProvider.Context = context; // Initialize the sign provider with the context
    }

    public ValueTask<BotSsoPacket> SendPacket(BotSsoPacket packet, ServiceAttribute options)
    {
        var tcs = new SsoPacketValueTaskSource();
        _pendingTasks.TryAdd(packet.Sequence, tcs);

        Task.Run(async () => // Schedule the task to the ThreadPool
        {
            try
            {
                ReadOnlyMemory<byte> frame;

                switch (options.RequestType)
                {
                    case RequestType.D2Auth:
                    {
                        if (SignProvider.IsWhiteListCommand(packet.Command))
                        {
                            var secInfo = await SignProvider.GetSecSign(_keystore.Uin, packet.Command, packet.Sequence, packet.Data);
                            var sso = _ssoPacker.BuildProtocol12(packet, secInfo);
                            frame = _servicePacker.BuildProtocol12(sso, options);
                        }
                        else
                        {
                            var sso = _ssoPacker.BuildProtocol12(packet, null);
                            frame = _servicePacker.BuildProtocol12(sso, options);
                        }

                        break;
                    }
                    case RequestType.Simple:
                    {
                        if (SignProvider.IsWhiteListCommand(packet.Command))
                        {
                            var secInfo = await SignProvider.GetSecSign(_keystore.Uin, packet.Command, packet.Sequence, packet.Data);
                            var sso = _ssoPacker.BuildProtocol13(packet, secInfo);
                            frame = _servicePacker.BuildProtocol13(packet, sso, options);
                        }
                        else
                        {
                            var sso = _ssoPacker.BuildProtocol13(packet, null);
                            frame = _servicePacker.BuildProtocol13(packet, sso, options);
                        }
                        break;
                    }
                    default:
                    {
                        throw new InvalidOperationException($"Unknown RequestType: {options.RequestType}");
                    }
                }

                await _context.SocketContext.Send(frame);
            }
            catch (Exception e)
            {
                if (_pendingTasks.TryRemove(packet.Sequence, out var tcs))
                {
                    tcs.SetException(e);
                }
            }
        });

        return new ValueTask<BotSsoPacket>(tcs, 0);
    }

    public void DispatchPacket(ReadOnlySpan<byte> buffer)
    {
        var service = _servicePacker.Parse(buffer);
        var sso = _ssoPacker.Parse(service);

        if (_pendingTasks.TryRemove(sso.Sequence, out var tcs))
        {
            if (sso is { RetCode: not 0, Extra: var extra })
            {
                string msg = $"Packet '{sso.Command}' returns {sso.RetCode} with seq: {sso.Sequence}, extra: {extra}";
                tcs.SetException(new InvalidOperationException(msg));
            }
            else
            {
                tcs.SetResult(sso);
            }
        }
        else
        {
            Task.Run(() => _context.EventContext.HandleServerPacket(sso));
        }
    }
}
