using System.Text;
using Lagrange.Core.Common;
using Lagrange.Core.Core.Event.Protocol.System;
using Lagrange.Core.Core.Packets.Service.Highway;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Core.Context;

/// <summary>
/// <para>Provides BDH (Big-Data Highway) Operation</para>
/// </summary>
internal class HighwayContext : ContextBase
{
    private readonly HttpClient _client;

    private Dictionary<uint, List<Uri>>? _highwayUrls;
    private byte[]? _sigSession;
    
    public HighwayContext(ContextCollection collection, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device)
        : base(collection, keystore, appInfo, device)
    {
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (_, _, _, _) => true,
        };
        
        _client = new HttpClient(handler);
        _client.DefaultRequestHeaders.Add("Accept-Encoding", "identity");
        _client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2)");
    }

    public async Task<bool> EchoAsync(Uri uri, uint seq)
    {
        var head = new ReqDataHighwayHead
        {
            MsgBaseHead = new DataHighwayHead
            {
                Version = 1, // isOpenUpEnable 2 else 1
                Uin = Keystore.Uin.ToString(),
                Command = "PicUp.Echo",
                Seq = seq,
                AppId = (uint)AppInfo.SubAppId,
                DataFlag = 4096,
                CommandId = 0,
                LocaleId = 2052
            }
        };

        try
        {
            await SendPacketAsync(head, new BinaryPacket(), uri);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> UploadSrcByStreamAsync(int commonId, uint uin, Stream data, byte[] md5, byte[]? extendInfo = null)
    {
        if (_highwayUrls is null) await FetchHttpConnUri();
        
        bool success = true;
        var upBlocks = new List<UpBlock>();
        var uri = _highwayUrls?[(uint)commonId][0] ?? throw new InvalidOperationException("No highway url");

        long fileSize = data.Length;
        int sequence = 1;
        int offset = 0;
        int chunkSize = fileSize is >= 1024 and <= 1048575 ? 8192 : 1024 * 1024;

        while (data.Position < data.Length)
        {
            var buffer = new byte[Math.Min(chunkSize, fileSize - offset)];
            int payload = await data.ReadAsync(buffer.AsMemory());
            var reqBody = new UpBlock(commonId, uin, Interlocked.Increment(ref sequence),
                                      (ulong)fileSize, (ulong)offset, md5, buffer, extendInfo, 
                                      (ulong)DateTimeOffset.Now.ToUnixTimeMilliseconds());
            upBlocks.Add(reqBody);
            offset = (int)data.Position;

            if (upBlocks.Count >= 32 || data.Position == data.Length)
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
            Seq = (uint)upBlock.Sequence,
            AppId = (uint)AppInfo.SubAppId,
            CommandId = (uint)upBlock.CommandId,
        };
        var segHead = new SegHead
        {
            Filesize = upBlock.FileSize,
            DataOffset = upBlock.Offset,
            DataLength = (uint)upBlock.Block.Length,
            ServiceTicket = _sigSession ?? throw new InvalidOperationException("No login sig"),
            Md5 = (await upBlock.Block.Md5Async()).UnHex(),
            FileMd5 = upBlock.FileMd5,
        };
        
        var highwayHead = new ReqDataHighwayHead
        {
            MsgBaseHead = head,
            MsgSegHead = segHead,
            BytesReqExtendInfo = upBlock.ExtendInfo,
            Timestamp = upBlock.Timestamp,
        };

        bool isEnd = upBlock.Offset + (ulong)upBlock.Block.Length == upBlock.FileSize;
        var payload = await SendPacketAsync(highwayHead, new BinaryPacket(upBlock.Block),  server, isEnd);
        var (respHead, resp) = ParsePacket(payload);

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

    private async Task FetchHttpConnUri()
    {
        var highwayUrlEvent = HighwayUrlEvent.Create();
        var results = await Collection.Business.SendEvent(highwayUrlEvent);
        if (results.Count != 0)
        {
            var result = (HighwayUrlEvent)results[0];
            _highwayUrls = result.HighwayUrls;
            _sigSession = result.SigSession;
        }
    }
    
    private record struct UpBlock(
        int CommandId, 
        uint Uin,
        int Sequence, 
        ulong FileSize,
        ulong Offset, 
        byte[] FileMd5,
        byte[] Block, 
        byte[]? ExtendInfo = null,
        ulong Timestamp = 0);
}