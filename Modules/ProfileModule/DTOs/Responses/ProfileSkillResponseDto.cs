using DotNetSkeleton.Modules.SkillModule.DTOs.Responses;

namespace DotNetSkeleton.Modules.ProfileModule.DTOs.Responses;

public sealed record ProfileSkillResponseDto(
    int Id,
    SkillDetailResponseDto Skill);
