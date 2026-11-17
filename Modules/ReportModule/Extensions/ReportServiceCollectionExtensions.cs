using DotNetSkeleton.Modules.ReportModule.Services;
using DotNetSkeleton.Modules.ReportModule.Services.Interfaces;

namespace DotNetSkeleton.Modules.ReportModule.Extensions;

public static class ReportServiceCollectionExtensions
{
    public static IServiceCollection AddReportModule(this IServiceCollection services)
    {
        services.AddScoped<IReportsService, ReportsService>();
        return services;
    }
}
