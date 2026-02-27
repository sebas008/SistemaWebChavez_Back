using Microsoft.Data.SqlClient;

namespace Chavez_Logistica.Interfaces;

public interface IDbConnectionFactory
{
    SqlConnection CreateConnection();
}
