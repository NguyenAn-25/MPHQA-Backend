using DotNetSkeleton.Modules.WalletModule.Converters;
using DotNetSkeleton.Modules.WalletModule.Services;
using DotNetSkeleton.Modules.WalletModule.Services.Interfaces;

namespace DotNetSkeleton.Modules.WalletModule.Extensions;

public static class WalletServiceCollectionExtensions
{
    public static IServiceCollection AddWalletModule(this IServiceCollection services)
    {
        services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.Converters.Add(new WalletObjectTypeJsonConverter());
            options.SerializerOptions.Converters.Add(new WalletTransactionTypeJsonConverter());
        });
        services.Configure<Microsoft.AspNetCore.Mvc.JsonOptions>(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new WalletObjectTypeJsonConverter());
            options.JsonSerializerOptions.Converters.Add(new WalletTransactionTypeJsonConverter());
        });

        services.AddScoped<IPaymentMethodService, PaymentMethodService>();

        return services;
    }
}
