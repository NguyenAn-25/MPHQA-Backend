using DotNetSkeleton.Modules.NotificationModule.Services;
using DotNetSkeleton.Modules.NotificationModule.Services.Interfaces;

namespace DotNetSkeleton.Modules.NotificationModule.Extensions;

public static class NotificationServiceCollectionExtensions
{
    public static IServiceCollection AddNotificationModule(this IServiceCollection services)
    {
        services.AddScoped<INotificationsService, NotificationsService>();
        return services;
    }
}
