using CleaningManager.Core.Interfaces;

namespace CleaningManager.Application.Chat;

public class ChatHandler(IChatService chatService)
{
    public async Task<ChatResponse> HandleAsync(ChatRequest request, CancellationToken cancellationToken = default)
    {
        var reply = await chatService.ChatAsync(request.Message, cancellationToken);
        return new ChatResponse(reply);
    }
}
