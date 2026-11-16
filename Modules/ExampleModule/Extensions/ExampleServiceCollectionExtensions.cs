using DotNetSkeleton.Modules.ExampleModule.Validators;
using FluentValidation;

namespace DotNetSkeleton.Modules.ExampleModule.Extensions;

public static class ExampleServiceCollectionExtensions
{
    public static IServiceCollection AddExampleModule(this IServiceCollection services)
    {
        services.AddScoped<IExampleService, ExampleService>();
        services.AddValidatorsFromAssemblyContaining<CreateExampleDtoValidator>();

        return services;
    }
}
