using Grpc.Core;
using Kritor.Message;

namespace Lagrange.Kritor.Services;

public class MessageService : global::Kritor.Message.MessageService.MessageServiceBase
{
    public override Task<SendMessageResponse> SendMessage(SendMessageRequest request, ServerCallContext context)
    {
        return base.SendMessage(request, context);
    }
}