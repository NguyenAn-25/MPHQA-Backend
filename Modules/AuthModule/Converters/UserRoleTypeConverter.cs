using System.ComponentModel;
using System.Globalization;
using DotNetSkeleton.Modules.AuthModule.Enums;

namespace DotNetSkeleton.Modules.AuthModule.Converters;

public sealed class UserRoleTypeConverter : EnumConverter
{
    public UserRoleTypeConverter() : base(typeof(UserRole))
    {
    }

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (value is string text)
        {
            if (string.Equals(text.Trim(), "Người dùng", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(text.Trim(), "USER", StringComparison.OrdinalIgnoreCase))
                return UserRole.User;

            if (string.Equals(text.Trim(), "Quản trị viên", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(text.Trim(), "ADMIN", StringComparison.OrdinalIgnoreCase))
                return UserRole.Admin;

            throw new FormatException("Role phải là Người dùng hoặc Quản trị viên.");
        }

        return base.ConvertFrom(context, culture, value);
    }
}
