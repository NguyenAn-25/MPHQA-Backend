using DotNetSkeleton.Modules.AuthModule.DTOs.Responses;
using DotNetSkeleton.Modules.AuthModule.Mappings;
using DotNetSkeleton.Modules.AuthModule.Services.Interfaces;
using DotNetSkeleton.Shared.Persistence;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace DotNetSkeleton.Modules.AuthModule.Services;

public sealed class FunctionsService(AppDbContext context) : IFunctionsService
{
    public async Task<IReadOnlyList<FunctionSummaryResponseDto>> ListAsync(
        CancellationToken cancellationToken) =>
        await context.Functions
            .AsNoTracking()
            .OrderBy(function => function.Id)
            .ProjectToType<FunctionSummaryResponseDto>(AuthMappingConfiguration.Config)
            .ToListAsync(cancellationToken);
}
