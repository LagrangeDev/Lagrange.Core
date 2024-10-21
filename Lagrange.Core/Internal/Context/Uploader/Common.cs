using Lagrange.Core.Internal.Event.Message;
using Lagrange.Core.Internal.Event.System;
using Lagrange.Core.Internal.Packets.Service.Highway;
using Lagrange.Core.Internal.Packets.Service.Oidb.Common;
using Lagrange.Core.Utility.Crypto.Provider.Sha;
using Lagrange.Core.Utility.Extension;

namespace Lagrange.Core.Internal.Context.Uploader;

internal static class Common
{
    private const int BlockSize = 1024 * 1024;
    
    public static NTV2RichMediaHighwayExt? GenerateExt(NTV2RichMediaUploadEvent @event)
    {
        if (@event.UKey == null) return null;
        
        var index = @event.MsgInfo.MsgInfoBody[0].Index;
        return new NTV2RichMediaHighwayExt
        {
            FileUuid = index.FileUuid,
            UKey = @event.UKey,
            Network = Convert(@event.Network),
            MsgInfoBody = @event.MsgInfo.MsgInfoBody,
            BlockSize = BlockSize,
            Hash = new NTHighwayHash
            {
                FileSha1 = new List<byte[]> { index.Info.FileSha1.UnHex() }
            }
        };
    }
    
    public static NTV2RichMediaHighwayExt? GenerateExt(NTV2RichMediaUploadEvent @event, SubFileInfo subFile)
    {
        if (subFile.UKey == null) return null;
        
        var index = @event.MsgInfo.MsgInfoBody[1].Index;
        return new NTV2RichMediaHighwayExt
        {
            FileUuid = index.FileUuid,
            UKey = subFile.UKey,
            Network = Convert(subFile.IPv4s),
            MsgInfoBody = @event.MsgInfo.MsgInfoBody,
            BlockSize = BlockSize,
            Hash = new NTHighwayHash
            {
                FileSha1 = new List<byte[]> { index.Info.FileSha1.UnHex() }
            }
        };
    }
    
    public static async Task<byte[]> GetTicket(ContextCollection context)
    {
        var hwUrlEvent = HighwayUrlEvent.Create();
        var highwayUrlResult = await context.Business.SendEvent(hwUrlEvent);
        return ((HighwayUrlEvent)highwayUrlResult[0]).SigSession;
    }

    public static List<byte[]> CalculateStreamBytes(Stream inputStream)
    {
        const int blockSize = 1024 * 1024;
        
        inputStream.Seek(0, SeekOrigin.Begin);
        var byteArrayList = new List<byte[]>();
        var sha1 = new Sha1Stream();
        
        var buffer = new byte[Sha1Stream.Sha1BlockSize];
        var digest = new byte[Sha1Stream.Sha1DigestSize];
        int lastRead;
        
        while (true)
        {
            int read = inputStream.Read(buffer);
            if (read < Sha1Stream.Sha1BlockSize)
            {
                lastRead = read;
                break;
            }
            
            sha1.Update(buffer, Sha1Stream.Sha1BlockSize);
            if (inputStream.Position % blockSize == 0)
            {
                sha1.Hash(digest, false);
                byteArrayList.Add((byte[])digest.Clone());
            }
        }
        
        sha1.Update(buffer, lastRead);
        sha1.Final(digest);
        byteArrayList.Add((byte[])digest.Clone());

        return byteArrayList;
    }
    
    private static NTHighwayNetwork Convert(List<IPv4> ipv4s) => new()
    {
        IPv4s = ipv4s.Select(x => new NTHighwayIPv4
        {
            Domain = new NTHighwayDomain
            {
                IsEnable = true,
                IP = ConvertIP(x.OutIP)
            },
            Port = x.OutPort
        }).ToList()
    };

    private static string ConvertIP(uint raw)
    {
        var ip = BitConverter.GetBytes(raw);
        return $"{ip[0]}.{ip[1]}.{ip[2]}.{ip[3]}";
    }
}