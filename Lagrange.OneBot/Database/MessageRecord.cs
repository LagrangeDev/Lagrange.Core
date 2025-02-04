using Lagrange.Core.Message;
using MessagePack;
using MessagePack.Resolvers;
using Realms;
using static Lagrange.Core.Message.MessageChain;

namespace Lagrange.OneBot.Database;

#pragma warning disable CS8618

// # ################################################### #
// #                       WARNING                       #
// # ################################################### #
// 
// When modifying MessageRecord, increment `SchemaVersion`.
// Beyond adding/removing fields (Realm auto-assigns
// defaults; complex processing requires migration logic
// in MigrationCallback), implement migration logic in
// `MigrationCallback` to handle all previous schema
// versions to current version.
// 
// SchemaVersion
// Lagrange.OneBot/Extensions/HostApplicationBuilderExtension.cs#L91
// 
// MigrationCallback
// Lagrange.OneBot/Extensions/HostApplicationBuilderExtension.cs#L92

public partial class MessageRecord : IRealmObject
{
    public static readonly MessagePackSerializerOptions OPTIONS = MessagePackSerializerOptions.Standard
        .WithResolver(CompositeResolver.Create(
            ContractlessStandardResolver.Instance,
            new MessageEntityResolver()
        ));

    [PrimaryKey]
    public int Id { get; set; }

    [MapTo(nameof(Type)), Indexed]
    public int TypeInt { get; set; }
    public MessageType Type { get => (MessageType)TypeInt; set => TypeInt = (int)value; }

    [MapTo(nameof(Sequence)), Indexed]
    public long SequenceLong { get; set; }
    public ulong Sequence { get => (ulong)SequenceLong; set => SequenceLong = (long)value; }

    [MapTo(nameof(ClientSequence)), Indexed]
    public long ClientSequenceLong { get; set; }
    public ulong ClientSequence { get => (ulong)ClientSequenceLong; set => ClientSequenceLong = (long)value; }

    [MapTo(nameof(MessageId)), Indexed]
    public long MessageIdLong { get; set; }
    public ulong MessageId { get => (ulong)MessageIdLong; set => MessageIdLong = (long)value; }

    public DateTimeOffset Time { get; set; }

    [MapTo(nameof(FromUin)), Indexed]
    public long FromUinLong { get; set; }
    public ulong FromUin { get => (ulong)FromUinLong; set => FromUinLong = (long)value; }

    [MapTo(nameof(ToUin)), Indexed]
    public long ToUinLong { get; set; }
    public ulong ToUin { get => (ulong)ToUinLong; set => ToUinLong = (long)value; }

    public byte[] Entities { get; set; }

    public static int CalcMessageHash(ulong msgId, uint seq)
    {
        return ((ushort)seq << 16) | (ushort)msgId;
    }

    public static implicit operator MessageRecord(MessageChain chain) => new()
    {
        Id = CalcMessageHash(chain.MessageId, chain.Sequence),
        Sequence = chain.Sequence,
        ClientSequence = chain.ClientSequence,
        MessageId = chain.MessageId,
        Time = chain.Time,
        FromUin = chain.FriendUin,
        ToUin = chain.Type switch
        {
            MessageType.Group => (ulong)chain.GroupUin!,
            MessageType.Temp or
            MessageType.Friend => chain.TargetUin,
            _ => throw new NotImplementedException(),
        },
        Entities = MessagePackSerializer.Serialize<List<IMessageEntity>>(chain, OPTIONS)
    };

    public static implicit operator MessageChain(MessageRecord record)
    {
        var chain = record.Type switch
        {
            MessageType.Group => new MessageChain(
                (uint)record.ToUin,
                (uint)record.FromUin,
                (uint)record.Sequence,
                record.MessageId
            ),
            MessageType.Temp or
            MessageType.Friend => new MessageChain(
                (uint)record.FromUin,
                string.Empty,
                string.Empty,
                (uint)record.ToUin,
                (uint)record.Sequence,
                (uint)record.ClientSequence,
                record.MessageId
            ),
            _ => throw new NotImplementedException(),
        };

        var entities = MessagePackSerializer.Deserialize<List<IMessageEntity>>(record.Entities, OPTIONS);
        chain.AddRange(entities);

        chain.Time = record.Time.DateTime;

        return chain;
    }
}
