using System.Collections.Concurrent;
using Lagrange.Core.Common;
using Lagrange.Core.Core.Network;
using Lagrange.Core.Core.Packets;
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
    
    public HighwayContext(ContextCollection collection, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device)
        : base(collection, keystore, appInfo, device)
    {
        _client = new HttpClient
        {
            DefaultRequestHeaders =
            {
                { "Accept-Encoding", "identity" },
            }
        };
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

    public async Task<bool> UploadSrcByStream(int commonId, uint uin, byte[] upKey, BinaryPacket data, long fileSize, byte[] md5)
    {
        return false;
    }

    private async Task<bool> SendUpBlockAsync(UpBlock upBlock, Uri server)
    {
        var head = new DataHighwayHead
        {
            Version = 1,
            Uin = upBlock.Uin.ToString(),
            Command = "PicUp.DataUp",
            Seq = (uint)upBlock.Sequence,
            RetryTimes = 0,
            AppId = (uint)AppInfo.SubAppId,
            DataFlag = 4096,
            CommandId = (uint)upBlock.CommandId,
            LocaleId = 2052
        };
        var segHead = new SegHead
        {
            Filesize = upBlock.FileSize,
            DataOffset = upBlock.Offset,
            DataLength = (uint)upBlock.Block.Length,
            ServiceTicket = upBlock.UpKey,
            Md5 = (await upBlock.Block.Md5Async()).UnHex(),
            FileMd5 = upBlock.FileMd5,
            CachePort = 0,
            CacheAddr = 0
        };
        
        var highwayHead = new ReqDataHighwayHead
        {
            MsgBaseHead = head,
            MsgSegHead = segHead,
            BytesReqExtendInfo = upBlock.ExtendInfo,
            Timestamp = upBlock.Timestamp,
            MsgLoginSigHead = upBlock.LoginSig
        };

        bool isEnd = upBlock.Offset + (ulong)upBlock.Block.Length == upBlock.FileSize;
        await SendPacketAsync(highwayHead, new BinaryPacket(upBlock.Block),  server, isEnd);

        return true;
    }

    private Task<BinaryPacket> SendPacketAsync(ReqDataHighwayHead head, BinaryPacket buffer, Uri server, bool end = true)
    {
        using var stream = new MemoryStream();
        Serializer.Serialize(stream, head);
        
        var writer = new BinaryPacket()
                .WriteByte(0x28) // packet start
                .WriteInt((int)stream.Length)
                .WriteInt((int)buffer.Length)
                .WriteBytes(stream.ToArray())
                .WritePacket(buffer)
                .WriteByte(0x29); // packet end
        
        return SendDataAsync(writer.ToArray(), server, end);
    }

    private static (RespDataHighwayHead, BinaryPacket) ParsePacket(BinaryPacket packet)
    {
        if (packet.ReadByte() == 0x28)
        {
            int headLength = packet.ReadInt();
            int bodyLength = packet.ReadInt();
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
                { "Content-Type", "application/x-www-form-urlencoded" },
            }
        };
        var response = await _client.SendAsync(request);
        var data = await response.Content.ReadAsByteArrayAsync();
        return new BinaryPacket(data);
    }
    
    private record UpBlock(
        int CommandId, 
        uint Uin,
        int Sequence, 
        ulong FileSize,
        ulong Offset, 
        byte[] FileMd5,
        byte[] UpKey,
        byte[] Block, 
        byte[]? ExtendInfo = null,
        ulong Timestamp = 0, 
        LoginSigHead? LoginSig = null);
}