using Lagrange.Core.Events;

namespace Lagrange.Core.Services;

/// <summary>
/// Builds and parses payloads for an SSO service command.
/// </summary>
public interface IService
{
    ValueTask<ProtocolEvent> Parse(ReadOnlyMemory<byte> input, BotContext context);

    ValueTask<ReadOnlyMemory<byte>> Build(ProtocolEvent input, BotContext context);
}
