using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Net.Http;

namespace DotNetSkeleton.Shared.Configurations;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerDocs(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Nhập JWT access token. Swagger sẽ tự thêm tiền tố Bearer."
            });
            options.DocumentFilter<AuthorizeDocumentFilter>();
        });

        return services;
    }

    public static WebApplication UseSwaggerDocs(this WebApplication app)
    {
        var swaggerEnabled = app.Configuration.GetValue<bool?>("Swagger:Enabled")
                             ?? app.Environment.IsDevelopment();

        if (swaggerEnabled)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        return app;
    }

    private sealed class AuthorizeDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument document, DocumentFilterContext context)
        {
            foreach (var apiDescription in context.ApiDescriptions)
            {
                var requiresAuthorization = apiDescription.ActionDescriptor.EndpointMetadata
                    .OfType<IAuthorizeData>()
                    .Any();

                if (!requiresAuthorization ||
                    string.IsNullOrWhiteSpace(apiDescription.HttpMethod) ||
                    string.IsNullOrWhiteSpace(apiDescription.RelativePath))
                    continue;

                var path = "/" + apiDescription.RelativePath.TrimEnd('/');
                if (!document.Paths.TryGetValue(path, out var pathItem) ||
                    pathItem is null)
                    continue;

                var operationType = apiDescription.HttpMethod.ToUpperInvariant() switch
                {
                    "DELETE" => HttpMethod.Delete,
                    "GET" => HttpMethod.Get,
                    "HEAD" => HttpMethod.Head,
                    "OPTIONS" => HttpMethod.Options,
                    "PATCH" => HttpMethod.Patch,
                    "POST" => HttpMethod.Post,
                    "PUT" => HttpMethod.Put,
                    "TRACE" => HttpMethod.Trace,
                    _ => null
                };

                if (operationType is null)
                    continue;

                var operations = pathItem?.Operations;
                if (operations is null ||
                    !operations.TryGetValue(operationType, out var operation) ||
                    operation is null)
                    continue;

                operation.Security =
                [
                    new OpenApiSecurityRequirement
                    {
                        [new OpenApiSecuritySchemeReference("Bearer", document, null)] = []
                    }
                ];
            }
        }
    }
}
