using DotNetSkeleton.Shared.Api;
using DotNetSkeleton.Shared.Exceptions;
using DotNetSkeleton.Shared.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace DotNetSkeleton.Shared.Extensions;

public static class SharedApiExtensions
{
    public static IServiceCollection AddSharedApi(this IServiceCollection services)
    {
        services.Configure<MvcOptions>(options =>
            options.ModelMetadataDetailsProviders.Add(new VietnameseValidationMetadataProvider()));
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
        services.AddFluentValidationAutoValidation(configuration =>
            configuration.OverrideDefaultResultFactoryWith<ApiValidationResultFactory>());
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState
                    .Where(entry => entry.Value?.Errors.Count > 0)
                    .ToDictionary(
                        entry => entry.Key,
                        entry => entry.Value!.Errors
                            .Select(error => GetValidationErrorMessage(error.ErrorMessage, error.Exception is not null))
                            .Distinct()
                            .ToArray());

                var response = ApiResponse<object?>.Fail(
                    "Dữ liệu đầu vào không hợp lệ.",
                    "validation_error",
                    context.HttpContext.TraceIdentifier,
                    errors);
                return new BadRequestObjectResult(response);
            };
        });

        return services;
    }

    private static string GetValidationErrorMessage(string? message, bool hasException)
    {
        if (string.IsNullOrWhiteSpace(message))
            return hasException ? "Giá trị không đúng định dạng." : "Giá trị không hợp lệ.";

        return message.StartsWith("The ", StringComparison.OrdinalIgnoreCase) ||
               message.StartsWith("Error converting", StringComparison.OrdinalIgnoreCase)
            ? "Giá trị không đúng định dạng."
            : message;
    }
}
