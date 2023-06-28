using System.Reflection;
using Lagrange.Core.Core.Packets.Service.Oidb.Generics;
using Lagrange.Core.Core.Packets.Service.Oidb.Internal;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Core.Packets.Service.Oidb;

/// <summary>
/// This class to declear a OidbSvcTrpcTcp packet 我愿称之为Protobuf版Tlv
/// <para><see cref="OidbSvcTrpcTcp0xE37_1700"/> Responsible for the uploading of NotOnlineFile, originally from OfflineFilleHandler_1700 of legacy oicq protocol</para>
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
    
    protected OidbSvcTrpcTcpBase(T body, bool isLafter)
    {
        var (command, subCommand) = OidbReference[typeof(T)];
        
        Command = command;
        SubCommand = subCommand;
        Body = body;
        Reserved = 0; // const
        Lafter = isLafter ? new OidbLafter() : null;
    }
}