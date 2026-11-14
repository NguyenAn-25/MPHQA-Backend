using DotNetSkeleton.Modules.ProfileModule.Services;
using DotNetSkeleton.Modules.ProfileModule.Services.Interfaces;

namespace DotNetSkeleton.Modules.ProfileModule.Extensions;

public static class ProfileServiceCollectionExtensions
{
    public static IServiceCollection AddProfileModule(this IServiceCollection services)
    {
        services.AddScoped<IVerificationRequestsService, VerificationRequestsService>();
        return services;
    }
}
