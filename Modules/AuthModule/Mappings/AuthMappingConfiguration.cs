using DotNetSkeleton.Modules.AuthModule.DTOs.Responses;
using DotNetSkeleton.Modules.AuthModule.Entities;
using DotNetSkeleton.Modules.AuthModule.Enums;
using Mapster;

namespace DotNetSkeleton.Modules.AuthModule.Mappings;

public static class AuthMappingConfiguration
{
    public static TypeAdapterConfig Config { get; } = CreateConfig();

    private static TypeAdapterConfig CreateConfig()
    {
        var config = new TypeAdapterConfig();

        config.NewConfig<UserEntity, UserResponseDto>()
            .Map(destination => destination.Id, source => source.Id)
            .Map(destination => destination.PublicId, source => source.PublicId)
            .Map(destination => destination.Phone, source => source.Phone)
            .Map(destination => destination.IsActive, source => source.IsActive)
            .Map(destination => destination.IsVerified, source => source.IsVerified)
            .Map(destination => destination.VerifiedAt, source => source.VerifiedAt)
            .Map(destination => destination.CreatedAt, source => source.CreatedAt)
            .Map(destination => destination.Role, source => source.Role);

        config.NewConfig<UserEntity, UserSummaryResponseDto>()
            .Map(destination => destination.Id, source => source.Id)
            .Map(destination => destination.PublicId, source => source.PublicId)
            .Map(destination => destination.Phone, source => source.Phone)
            .Map(destination => destination.IsActive, source => source.IsActive)
            .Map(destination => destination.IsVerified, source => source.IsVerified)
            .Map(destination => destination.CreatedAt, source => source.CreatedAt)
            .Map(destination => destination.Role, source => source.Role)
            .Map(
                destination => destination.Permission,
                source => source.Permission == null
                    ? null
                    : new UserPermissionSummaryResponseDto(
                        source.Permission.Id,
                        source.Permission.Name));

        config.NewConfig<UserEntity, UserDetailResponseDto>()
            .Map(destination => destination.Id, source => source.Id)
            .Map(destination => destination.PublicId, source => source.PublicId)
            .Map(destination => destination.Phone, source => source.Phone)
            .Map(destination => destination.IsActive, source => source.IsActive)
            .Map(destination => destination.IsVerified, source => source.IsVerified)
            .Map(destination => destination.VerifiedAt, source => source.VerifiedAt)
            .Map(destination => destination.CreatedAt, source => source.CreatedAt)
            .Map(destination => destination.LastActiveAt, source => source.LastActiveAt)
            .Map(destination => destination.Role, source => source.Role)
            .Map(
                destination => destination.Permission,
                source => source.Permission == null
                    ? null
                    : new UserPermissionDetailResponseDto(
                        source.Permission.Id,
                        source.Permission.Name,
                        source.Permission.Description,
                        source.Permission.IsActive));

        config.NewConfig<FunctionEntity, FunctionSummaryResponseDto>()
            .Map(destination => destination.Id, source => source.Id)
            .Map(destination => destination.Code, source => source.Code)
            .Map(destination => destination.NameVi, source => source.NameVi)
            .Map(destination => destination.NameEn, source => source.NameEn)
            .Map(destination => destination.Actions, source => source.Actions);

        config.NewConfig<PermissionEntity, PermissionSummaryResponseDto>()
            .Map(destination => destination.Id, source => source.Id)
            .Map(destination => destination.Name, source => source.Name)
            .Map(destination => destination.Description, source => source.Description)
            .Map(destination => destination.IsActive, source => source.IsActive)
            .Map(destination => destination.UserCount, source => source.Users.Count);

        config.NewConfig<PermissionEntity, PermissionDetailResponseDto>()
            .Map(destination => destination.Id, source => source.Id)
            .Map(destination => destination.Name, source => source.Name)
            .Map(destination => destination.Description, source => source.Description)
            .Map(destination => destination.IsActive, source => source.IsActive)
            .Map(destination => destination.UserCount, source => source.Users.Count)
            .Map(
                destination => destination.Details,
                source => source.Details.OrderBy(detail => detail.FunctionId).ThenBy(detail => detail.Action));

        config.NewConfig<PermissionDetailEntity, PermissionGrantResponseDto>()
            .Map(
                destination => destination.Function,
                source => new PermissionFunctionResponseDto(
                    source.Function.Id,
                    source.Function.Code,
                    source.Function.NameVi,
                    source.Function.NameEn))
            .Map(destination => destination.Action, source => source.Action);

        return config;
    }
}
