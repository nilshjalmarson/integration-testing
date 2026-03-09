using System.Data;
using Api.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Api.Data;

public class LiftRepository
{
    private readonly SqlConnection _connection;

    public LiftRepository(SqlConnection connection)
    {
        _connection = connection;
    }

    public async Task<IEnumerable<Lift>> GetAllLifts()
    {
        const string query = "SELECT Id, Name, Status, WaitTimeMinutes FROM dbo.Lift";
        return await _connection.QueryAsync<Lift>(query);
    }
}
