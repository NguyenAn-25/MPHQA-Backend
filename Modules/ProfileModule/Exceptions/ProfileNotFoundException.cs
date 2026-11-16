using DotNetSkeleton.Shared.Exceptions;

namespace DotNetSkeleton.Modules.ProfileModule.Exceptions;

public sealed class ProfileNotFoundException()
    : AppException(StatusCodes.Status404NotFound, "Chưa có hồ sơ cá nhân.");
