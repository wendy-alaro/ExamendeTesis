using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace WebApplication_Ejemplo_1.Services;

internal static class SqlServerUniqueConstraint
{
    public static bool IsViolation(DbUpdateException exception)
    {
        return exception.GetBaseException() is SqlException { Number: 2601 or 2627 };
    }
}
