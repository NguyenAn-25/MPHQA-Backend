namespace DotNetSkeleton.Shared.Configurations;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerDocs(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

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
}
