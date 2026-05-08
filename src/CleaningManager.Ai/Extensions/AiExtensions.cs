using CleaningManager.Ai.Services;
using CleaningManager.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CleaningManager.Ai.Extensions;

public static class AiExtensions
{
    public static IServiceCollection AddAi(this IServiceCollection services)
    {
        services.AddScoped<IChatService, CleaningAiService>();
        return services;
    }
}
