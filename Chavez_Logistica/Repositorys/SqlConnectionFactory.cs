using Microsoft.Data.SqlClient;
using Chavez_Logistica.Interfaces;

namespace Chavez_Logistica.Repositorys;

public class SqlConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public SqlConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new Exception("No existe DefaultConnection en appsettings.");
    }

    public SqlConnection CreateConnection()
    {
        var cn = new SqlConnection(_connectionString);
        cn.Open();
        return cn;
    }
}
