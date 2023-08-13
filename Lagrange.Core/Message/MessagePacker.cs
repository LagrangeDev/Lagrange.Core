using System.Reflection;
using ProtoBuf;
using Lagrange.Core.Core.Packets.Message.C2C;
using Lagrange.Core.Core.Packets.Message.Component;
using Lagrange.Core.Core.Packets.Message.Element;
using Lagrange.Core.Core.Packets.Message.Routing;
using Lagrange.Core.Message.Entity;
using Lagrange.Core.Utility.Extension;
using MessageControl = Lagrange.Core.Core.Packets.Message.MessageControl;

namespace Lagrange.Core.Message;

/// <summary>
/// Pack up message into the Protobuf <see cref="Message"/>
/// </summary>
internal class MessagePacker
{
    private static readonly Dictionary<Type, List<PropertyInfo>> EntityToElem;
    private static readonly Dictionary<Type, IMessageEntity> Factory;
    private static readonly List<IMessageEntity> MsgFactory;

    private readonly string _selfUid;

    static MessagePacker()
    {
        EntityToElem = new Dictionary<Type, List<PropertyInfo>>();
        Factory = new Dictionary<Type, IMessageEntity>();

        var assembly = Assembly.GetExecutingAssembly();
        var types = assembly.GetTypeWithMultipleAttributes<MessageElementAttribute>(out var attributeArrays);
        var elemType = typeof(Elem);
        
        for (int i = 0; i < types.Count; i++)
        {
            var type = types[i];
            var attributes = attributeArrays[i];

            foreach (var attribute in attributes)
            {
                var property = elemType.GetProperty(attribute.Element.Name);
                if (property != null)
                {
                    if (EntityToElem.TryGetValue(type, out var properties)) properties.Add(property);
                    else EntityToElem[type] = new List<PropertyInfo> { property };
                }
            }

            if (type.CreateInstance() is IMessageEntity factory) Factory[type] = factory;
        }

        MsgFactory = assembly.GetImplementations<IMessageEntity>().Select(type => (IMessageEntity)type.CreateInstance()).ToList();
    }
    
    public MessagePacker(string selfUid)
    {
        _selfUid = selfUid;
    }

    public Core.Packets.Message.Message Build(MessageChain chain)
    {
        var message = BuildPacketBase(chain);

        foreach (var entity in chain)
        {
            entity.SetSelfUid(_selfUid);
            
            if (message.Body != null)
            {
                message.Body.RichText?.Elems.AddRange(entity.PackElement());
                
                var msgContent = entity.PackMessageContent();
                if (msgContent is not null)
                {
                    if (message.Body.MsgContent is not null) throw new InvalidOperationException("Message content is not null, conflicting with the message entity.");
                    
                    using var stream = new MemoryStream();
                    Serializer.Serialize(stream, msgContent);
                    message.Body.MsgContent = stream.ToArray();
                }
            }
        }

        return message;
    }
    
    public static MessageChain Parse(Core.Packets.Message.PushMsgBody message)
    {
        var chain = ParseChain(message);

        if (message.Body?.RichText?.Elems != null) // 怎么Body还能是null的
        {
            foreach (var element in message.Body.RichText.Elems)
            {
                foreach (var (entityType, expectElems) in EntityToElem)
                {
                    foreach (var expectElem in expectElems)
                    {
                        var val = expectElem.GetValueByExpr(element);
                        if (val != null)
                        {
                            var entity = Factory[entityType].UnpackElement(element);
                            if (entity != null)
                            {
                                chain.Add(entity);
                                break;
                            }
                        }
                    }
                }
            }
        }

        if (message.Body is { MsgContent: not null, RichText: null }) // if RichText is not null, it means that the message is from Tencent's SSO server
        {
            foreach (var factory in MsgFactory)
            {
                var entity = factory.UnpackMessageContent(message.Body.MsgContent);
                if (entity != null)
                {
                    chain.Add(entity);
                    break;
                }
            }
        }

        return chain;
    }

    private static Core.Packets.Message.Message BuildPacketBase(MessageChain chain) => new()
    {
        RoutingHead = new Core.Packets.Message.RoutingHead
        {
            C2C = chain.IsGroup ? null : new C2C
            {
                Uid = chain.Uid,
                Uin = chain.FriendUin
            },
            Grp = !chain.IsGroup ? null : new Grp // for consistency of code so inverted condition
            {
                GroupCode = chain.GroupUin
            },
            Trans0X211 = !chain.HasTypeOf<FileEntity>() ? null : new Trans0X211
            {
                CcCmd = 4,
                Uid = chain.Uid
            }
        },
        ContentHead = new Core.Packets.Message.ContentHead
        {
            PkgNum = 1, // regarded as the const
            PkgIndex = 0,
            DivSeq = 0
        },
        Body = new Core.Packets.Message.MessageBody { RichText = new RichText { Elems = new List<Elem>() } },
        Seq = (uint)Random.Shared.Next(1000000, 9999999), // 草泥马开摆！
        Rand = (uint)Random.Shared.Next(100000000, int.MaxValue),
        Ctrl = chain.IsGroup ? null : new MessageControl { MsgFlag = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds() }
    };
    
    private static MessageChain ParseChain(Core.Packets.Message.PushMsgBody message)
    {
        return message.ResponseHead.Grp == null
            ? new MessageChain(message.ResponseHead.FromUin,message.ResponseHead.ToUid ?? string.Empty ,message.ResponseHead.FromUid ?? string.Empty)
            : new MessageChain(message.ResponseHead.Grp.GroupUin, message.ResponseHead.FromUin, (uint)(message.ContentHead.Sequence ?? 0));
    }
}