using Lagrange.Core.Common;
using Lagrange.Core.Core.Packets.Service.Highway;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Extension;
using ProtoBuf.Meta;

namespace Lagrange.Core.Core.Context;

/// <summary>
/// <para>Provides BDH (Big-Data Highway) Operation</para>
/// </summary>
internal class HighwayContext : ContextBase
{
    private readonly HttpClient _client;
    private uint _sequence;
    private static readonly RuntimeTypeModel Serializer;

    static HighwayContext()
    {
        Serializer = RuntimeTypeModel.Create();
        Serializer.UseImplicitZeroDefaults = false;
    }
    
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

        _sequence = 0;
    }

    public async Task<bool> EchoAsync()
    {
        var uri = new Uri("https://sslv6.htdata.qq.com:443/cgi-bin/httpconn?htcmd=0x6FF0087&uin=114514");
        
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

    public async Task<bool> UploadSrcByStreamAsync(int commonId, uint uin, Stream data, string ticket, byte[] md5, byte[]? extendInfo = null)
    {
        bool success = true;
        var upBlocks = new List<UpBlock>();
        var uri = new Uri("https://sslv6.htdata.qq.com:443/cgi-bin/httpconn?htcmd=0x6FF0087&uin=114514");

        long fileSize = data.Length;
        int offset = 0;
        int chunkSize = fileSize is >= 1024 and <= 1048575 ? 8192 : 1024 * 1024;
        data.Seek(0, SeekOrigin.Begin);
        int concurrent = commonId == 2 ? 1 : 8;

        while (offset < fileSize)
        {
            var buffer = new byte[Math.Min(chunkSize, fileSize - offset)];
            int payload = await data.ReadAsync(buffer.AsMemory());
            var reqBody = new UpBlock(commonId, uin, Interlocked.Increment(ref _sequence), (ulong)fileSize, (ulong)offset, ticket, md5, buffer, extendInfo);
            upBlocks.Add(reqBody);
            offset += payload;

            if (upBlocks.Count >= concurrent || data.Position == data.Length)
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
        
        Console.WriteLine($"Block Result: {respHead.ErrorCode} | {respHead.MsgSegHead?.RetCode} | {respHead.BytesRspExtendInfo?.Hex()}");

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
        string Ticket,
        byte[] FileMd5,
        byte[] Block, 
        byte[]? ExtendInfo = null,
        ulong Timestamp = 0);
}