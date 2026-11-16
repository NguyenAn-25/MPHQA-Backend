using DotNetSkeleton.Modules.AuthModule.DTOs.Responses;

namespace DotNetSkeleton.Modules.AuthModule.Services.Interfaces;

public interface IFunctionsService
{
    Task<IReadOnlyList<FunctionSummaryResponseDto>> ListAsync(CancellationToken cancellationToken);
}
