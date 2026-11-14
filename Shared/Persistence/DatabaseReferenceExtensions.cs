using Microsoft.EntityFrameworkCore;

namespace DotNetSkeleton.Shared.Persistence;

public static class DatabaseReferenceExtensions
{
    public static async Task<bool> HasReferencesAsync(
        this AppDbContext context,
        string tableName,
        string columnName,
        int referencedId,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(context);
        ValidateIdentifier(tableName, nameof(tableName));
        ValidateIdentifier(columnName, nameof(columnName));

        var tableExists = await context.Database.SqlQueryRaw<int>(
                "SELECT CASE WHEN to_regclass({0}) IS NULL THEN 0 ELSE 1 END AS \"Value\"",
                tableName)
            .SingleAsync(cancellationToken);
        if (tableExists == 0)
            return false;

        var sql = $"SELECT EXISTS (SELECT 1 FROM \"{tableName}\" WHERE \"{columnName}\" = {{0}}) AS \"Value\"";
        return await context.Database.SqlQueryRaw<bool>(sql, referencedId).SingleAsync(cancellationToken);
    }

    private static void ValidateIdentifier(string identifier, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(identifier) ||
            !(char.IsAsciiLetter(identifier[0]) || identifier[0] == '_') ||
            identifier.Any(character => !(char.IsAsciiLetterOrDigit(character) || character == '_')))
        {
            throw new ArgumentException("Database identifiers must contain only ASCII letters, digits, and underscores.", parameterName);
        }
    }
}
