using DotNetSkeleton.Modules.AuthModule.Services;
using DotNetSkeleton.Modules.AuthModule.Services.Interfaces;

namespace DotNetSkeleton.Modules.AuthModule.Extensions;

internal static class OtpSmsServiceCollectionExtensions
{
    internal static IServiceCollection AddOtpSms(this IServiceCollection services)
    {
        services.AddScoped<IOtpSmsSender, LoggingOtpSmsSender>();
        return services;
    }
}
