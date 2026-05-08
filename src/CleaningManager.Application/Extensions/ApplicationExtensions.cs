using CleaningManager.Application.Chat;
using Microsoft.Extensions.DependencyInjection;

namespace CleaningManager.Application.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ChatHandler>();
        return services;
    }
}
