using System.Reflection;
using Lagrange.Core.Common;
using Lagrange.Core.Internal.Context.Uploader;
using Lagrange.Core.Internal.Packets.Service.Highway;
using Lagrange.Core.Message;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Extension;
using ProtoBuf.Meta;

namespace Lagrange.Core.Internal.Context;

/// <summary>
/// <para>Provides BDH (Big-Data Highway) Operation</para>
/// </summary>
internal class HighwayContext : ContextBase, IDisposable
{
    private const string Tag = nameof(HighwayContext);

    private readonly Dictionary<Type, IHighwayUploader> _uploaders;
    
    private readonly HttpClient _client;
    private uint _sequence;
    private static readonly RuntimeTypeModel Serializer;
    private readonly int _chunkSize;
    private readonly uint _concurrent;

    static HighwayContext()
    {
        Serializer = RuntimeTypeModel.Create();
        Serializer.UseImplicitZeroDefaults = false;
    }
    
    public HighwayContext(ContextCollection collection, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, BotConfig config)
        : base(collection, keystore, appInfo, device)
    {
        _uploaders = new Dictionary<Type, IHighwayUploader>();
        foreach (var impl in Assembly.GetExecutingAssembly().GetImplementations<IHighwayUploader>())
        {
            var attribute = impl.GetCustomAttribute<HighwayUploaderAttribute>();
            if (attribute != null) _uploaders[attribute.Entity] = (IHighwayUploader)impl.CreateInstance(false);
        }
        
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (_, _, _, _) => true,
        };
        
        _client = new HttpClient(handler);
        _client.DefaultRequestHeaders.Add("Accept-Encoding", "identity");
        _client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2)");

        _sequence = 0;
        _chunkSize = (int)config.HighwayChunkSize;
        _concurrent = config.HighwayConcurrent;
    }

    public async Task<bool> EchoAsync(uint uin)
    {
        var uri = new Uri($"http://htdata3.qq.com:80/cgi-bin/httpconn?htcmd=0x6FF0087&uin={uin}");
        
        var head = new ReqDataHighwayHead
        {
            MsgBaseHead = new DataHighwayHead
            {
                Version = 1, // isOpenUpEnable 2 else 1
                Uin = Keystore.Uin.ToString(),
                Command = "PicUp.Echo",
                Seq = Interlocked.Increment(ref _sequence),
                AppId = (uint)AppInfo.SubAppId,
                CommandId = 0
            }
        };

        try
        {
            var payload = await SendPacketAsync(head, new BinaryPacket(),  uri);
            var (parsedHead, _) = ParsePacket(payload);
            return parsedHead.ErrorCode == 0;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task UploadResources(MessageChain chain)
    {
        foreach (var entity in chain)
        {
            if (_uploaders.TryGetValue(entity.GetType(), out var uploader))
            {
                try
                {
                    if (chain.IsGroup) await uploader.UploadGroup(Collection, chain, entity);
                    else await uploader.UploadPrivate(Collection, chain, entity);
                }
                catch
                {
                    Collection.Log.LogFatal(Tag, $"Upload resources for {entity.GetType().Name} failed");
                }
            }
        }
    }

    public async Task<bool> UploadSrcByStreamAsync(int commonId, Stream data, byte[] ticket, byte[] md5, byte[]? extendInfo = null)
    {
        bool success = true;
        var upBlocks = new List<UpBlock>();
        var uri = new Uri($"http://htdata3.qq.com:80/cgi-bin/httpconn?htcmd=0x6FF0087&uin={Collection.Keystore.Uin}");

        long fileSize = data.Length;
        int offset = 0;

        data.Seek(0, SeekOrigin.Begin);
        while (offset < fileSize)
        {
            var buffer = new byte[Math.Min(_chunkSize, fileSize - offset)];
            int payload = await data.ReadAsync(buffer.AsMemory());
            var reqBody = new UpBlock(commonId, Collection.Keystore.Uin, Interlocked.Increment(ref _sequence), (ulong)fileSize, (ulong)offset, ticket, md5, buffer, extendInfo);
            upBlocks.Add(reqBody);
            offset += payload;

            if (upBlocks.Count >= _concurrent || data.Position == data.Length)
            {
                var tasks = upBlocks.Select(x => SendUpBlockAsync(x, uri)).ToArray();
                var results = await Task.WhenAll(tasks);
                success &= results.All(x => x);
                
                upBlocks.Clear();
            }
        }

        return success;
    }

    private async Task<bool> SendUpBlockAsync(UpBlock upBlock, Uri server)
    {
        var head = new DataHighwayHead
        {
            Version = 1,
            Uin = upBlock.Uin.ToString(),
            Command = "PicUp.DataUp",
            Seq = upBlock.Sequence,
            AppId = (uint)AppInfo.SubAppId,
            DataFlag = 16,
            CommandId = (uint)upBlock.CommandId,
        };
        var segHead = new SegHead
        {
            Filesize = upBlock.FileSize,
            DataOffset = upBlock.Offset,
            DataLength = (uint)upBlock.Block.Length,
            ServiceTicket = upBlock.Ticket,
            Md5 = (await upBlock.Block.Md5Async()).UnHex(),
            FileMd5 = upBlock.FileMd5,
        };
        var loginHead = new LoginSigHead
        {
            Uint32LoginSigType = 8,
            BytesLoginSig = Collection.Keystore.Session.Tgt,
            AppId = (uint)Collection.AppInfo.AppId
        };
        var highwayHead = new ReqDataHighwayHead
        {
            MsgBaseHead = head,
            MsgSegHead = segHead,
            BytesReqExtendInfo = upBlock.ExtendInfo,
            Timestamp = upBlock.Timestamp,
            MsgLoginSigHead = loginHead
        };

        bool isEnd = upBlock.Offset + (ulong)upBlock.Block.Length == upBlock.FileSize;
        var payload = await SendPacketAsync(highwayHead, new BinaryPacket(upBlock.Block),  server, isEnd);
        var (respHead, resp) = ParsePacket(payload);

        Collection.Log.LogDebug(Tag, $"Highway Block Result: {respHead.ErrorCode} | {respHead.MsgSegHead?.RetCode} | {respHead.BytesRspExtendInfo?.Hex()} | {resp.ToArray().Hex()}");

        return respHead.ErrorCode == 0;
    }

    private Task<BinaryPacket> SendPacketAsync(ReqDataHighwayHead head, BinaryPacket buffer, Uri server, bool end = true)
    {
        using var stream = new MemoryStream();
        Serializer.Serialize(stream, head);
        
        var writer = new BinaryPacket()
                .WriteByte(0x28) // packet start
                .WriteInt((int)stream.Length, false)
                .WriteInt((int)buffer.Length, false)
                .WriteBytes(stream.ToArray())
                .WritePacket(buffer)
                .WriteByte(0x29); // packet end
        
        return SendDataAsync(writer.ToArray(), server, end);
    }

    private static (RespDataHighwayHead, BinaryPacket) ParsePacket(BinaryPacket packet)
    {
        if (packet.ReadByte() == 0x28)
        {
            int headLength = packet.ReadInt(false);
            int bodyLength = packet.ReadInt(false);
            var head = Serializer.Deserialize<RespDataHighwayHead>(packet.ReadBytes(headLength).AsSpan());
            var body = packet.ReadPacket(bodyLength);
            
            if (packet.ReadByte() == 0x29) return (head, body);
        }
        
        throw new InvalidOperationException("Invalid packet");
    }
    
    private async Task<BinaryPacket> SendDataAsync(byte[] packet, Uri server, bool end)
    {
        var content = new ByteArrayContent(packet);
        var request = new HttpRequestMessage(HttpMethod.Post, server)
        {
            Content = content,
            Headers =
            {
                { "Connection" , end ? "close" : "keep-alive" },
            }
        };
        var response = await _client.SendAsync(request);
        var data = await response.Content.ReadAsByteArrayAsync();
        return new BinaryPacket(data);
    }
    
    private record struct UpBlock(
        int CommandId, 
        uint Uin,
        uint Sequence, 
        ulong FileSize,
        ulong Offset, 
        byte[] Ticket,
        byte[] FileMd5,
        byte[] Block, 
        byte[]? ExtendInfo = null,
        ulong Timestamp = 0);

    public void Dispose()
    {
        _client.Dispose();
    }
}