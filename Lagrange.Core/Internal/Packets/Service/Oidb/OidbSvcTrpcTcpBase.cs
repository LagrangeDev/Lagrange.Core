using System.Reflection;
using Lagrange.Core.Internal.Packets.Service.Oidb.Generics;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb;

/// <summary>
/// <para>This class to declear a OidbSvcTrpcTcp packet 我愿称之为Protobuf版Tlv</para>
/// <para><see cref="OidbSvcTrpcTcp0xE37_1700"/> Responsible for the uploading of NotOnlineFile, originally from OfflineFilleHandleSvr.pb_ftn_CMD_REQ_APPLY_UPLOAD_V3-1700 of legacy oicq protocol</para>
/// </summary>
[ProtoContract]
internal class OidbSvcTrpcTcpBase<T> where T : class
{
    private static readonly Dictionary<Type, (uint, uint)> OidbReference;

    static OidbSvcTrpcTcpBase()
    {
        OidbReference = new Dictionary<Type, (uint, uint)>();

        var assembly = Assembly.GetExecutingAssembly();
        var types = assembly.GetTypeByAttributes<OidbSvcTrpcTcpAttribute>(out var attributes);

        for (int i = 0; i < types.Count; i++)
        {
            var type = types[i];
            var attribute = attributes[i];
            OidbReference[type] = (attribute.Command, attribute.SubCommand);
        }
    }

    [ProtoMember(1)] public uint Command { get; set; }
    
    [ProtoMember(2)] public uint SubCommand { get; set; }
    
    [ProtoMember(4)] public T Body { get; set; }
    
    [ProtoMember(7)] public OidbLafter? Lafter { get; set; } // if Lafter is null, it would not be serialized
    
    [ProtoMember(12)] public int Reserved { get; set; }
    
    public OidbSvcTrpcTcpBase(T body, bool isLafter = false, bool isUid = false)
    {
        var (command, subCommand) = OidbReference[typeof(T)];
        
        Command = command;
        SubCommand = subCommand;
        Body = body;
        Reserved = Convert.ToInt32(isUid);
        Lafter = isLafter ? new OidbLafter() : null;
    }
    
    public OidbSvcTrpcTcpBase(T body, uint command,uint subCommand, bool isLafter = false, bool isUid = false)
    {
        Command = command;
        SubCommand = subCommand;
        Body = body;
        Reserved = Convert.ToInt32(isUid);
        Lafter = isLafter ? new OidbLafter() : null;
    }
}