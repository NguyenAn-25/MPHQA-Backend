using DotNetSkeleton.Modules.ReportModule.DTOs.Requests;
using DotNetSkeleton.Modules.ReportModule.DTOs.Responses;

namespace DotNetSkeleton.Modules.ReportModule.Services.Interfaces;

public interface IReportsService
{
    Task<PagedReportsResponseDto> ListAsync(
        ListReportsRequestDto request,
        CancellationToken cancellationToken);

    Task<ReportDetailResponseDto> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task<ReportProcessResponseDto> ProcessAsync(
        int id,
        int reviewerId,
        ProcessReportRequestDto request,
        CancellationToken cancellationToken);
}
