using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Store.Common.Models;

namespace Store.Infrastructure.Persistence;

internal static class DbSaveChangesExceptionMapper
{
    public static ErrorModel Map(Exception exception, bool includeTechnicalDetails)
    {
        if (exception is DbUpdateConcurrencyException)
            return ErrorModel.Create("ConcurrencyConflict");

        var sqlException = FindSqlException(exception);
        if (sqlException is not null)
        {
            foreach (SqlError error in sqlException.Errors)
            {
                var mapped = MapSqlError(error.Number, error.Message);
                if (mapped is not null)
                    return mapped;
            }
        }

        if (exception is DbUpdateException)
            return ErrorModel.Create("GeneralException");

        return includeTechnicalDetails
            ? ErrorModel.CreateLiteral("ServerError", GetInnermostMessage(exception))
            : ErrorModel.Create("GeneralException");
    }

    public static bool IsBusinessRuleViolation(Exception exception)
    {
        if (exception is DbUpdateConcurrencyException)
            return true;

        var sqlException = FindSqlException(exception);
        if (sqlException is null)
            return false;

        foreach (SqlError error in sqlException.Errors)
        {
            if (error.Number is 547 or 2601 or 2627 or 515 or 8152 or 1205)
                return true;
        }

        return false;
    }

    internal static ErrorModel? MapSqlError(int number, string message)
    {
        return number switch
        {
            547 => MapReferenceConstraintError(message),
            2601 or 2627 => ErrorModel.Create("DuplicateRecord"),
            515 => ErrorModel.Create("RequiredFieldMissing"),
            8152 => ErrorModel.Create("DataTooLong"),
            1205 => ErrorModel.Create("DatabaseDeadlock"),
            -2 => ErrorModel.Create("DatabaseTimeout"),
            _ => null
        };
    }

    private static ErrorModel MapReferenceConstraintError(string message)
    {
        if (message.Contains("DELETE statement conflicted", StringComparison.OrdinalIgnoreCase))
            return ErrorModel.Create("RecordHasDependencies");

        if (message.Contains("CHECK constraint", StringComparison.OrdinalIgnoreCase))
            return ErrorModel.Create("ValidationError");

        if (message.Contains("INSERT statement conflicted", StringComparison.OrdinalIgnoreCase) ||
            message.Contains("UPDATE statement conflicted", StringComparison.OrdinalIgnoreCase))
            return ErrorModel.Create("InvalidReference");

        return ErrorModel.Create("RecordHasDependencies");
    }

    private static SqlException? FindSqlException(Exception exception)
    {
        for (var current = exception; current is not null; current = current.InnerException)
        {
            if (current is SqlException sqlException)
                return sqlException;
        }

        return null;
    }

    private static string GetInnermostMessage(Exception exception)
    {
        var message = exception.Message;
        while (exception.InnerException is not null)
        {
            exception = exception.InnerException;
            if (!string.IsNullOrWhiteSpace(exception.Message))
                message = exception.Message;
        }

        return message;
    }
}
