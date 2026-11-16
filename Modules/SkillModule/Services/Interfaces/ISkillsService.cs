using DotNetSkeleton.Modules.SkillModule.DTOs.Requests;
using DotNetSkeleton.Modules.SkillModule.DTOs.Responses;

namespace DotNetSkeleton.Modules.SkillModule.Services.Interfaces;

public interface ISkillsService
{
    Task<PagedSkillsResponseDto> ListAsync(ListSkillsRequestDto request, CancellationToken cancellationToken);
    Task<IReadOnlyList<SkillOptionResponseDto>> GetOptionsAsync(CancellationToken cancellationToken);
    Task<SkillDetailResponseDto> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<SkillDetailResponseDto> CreateAsync(CreateSkillRequestDto request, CancellationToken cancellationToken);
    Task<SkillDetailResponseDto> UpdateAsync(int id, UpdateSkillRequestDto request, CancellationToken cancellationToken);
    Task<SkillDetailResponseDto> UpdateStatusAsync(int id, UpdateSkillStatusRequestDto request, CancellationToken cancellationToken);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
}
