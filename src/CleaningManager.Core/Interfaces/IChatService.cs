namespace CleaningManager.Core.Interfaces;

public interface IChatService
{
    Task<string> ChatAsync(string userMessage, CancellationToken cancellationToken = default);
}
