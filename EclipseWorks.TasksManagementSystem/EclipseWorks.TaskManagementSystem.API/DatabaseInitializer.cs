using EclipseWorks.TaskManagementSystem.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace EclipseWorks.TaskManagementSystem.API;

public static class DatabaseInitializer
{
    public static void InitializeDatabase(string connectionString)
    {
        var script = File.ReadAllText("/dbinit/001_DATABASE_CREATION.sql");

        var batches = script.Split(new[] { "GO\r\n", "GO ", "GO\t" }, StringSplitOptions.RemoveEmptyEntries);
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            foreach (var batch in batches)
            {
                using (var command = new SqlCommand(batch, connection))
                {
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (SqlException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
        }
    }
}
