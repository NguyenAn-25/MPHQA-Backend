using System.ComponentModel;
using DotNetSkeleton.Modules.AuthModule.Converters;

namespace DotNetSkeleton.Modules.AuthModule.Enums;

[TypeConverter(typeof(UserRoleTypeConverter))]
public enum UserRole
{
    User,
    Admin
}
