using System.Data;
using Bookify.Application.Abstraction.Data;
using Microsoft.Data.SqlClient;

namespace Bookify.Infrastructure.Data;

public class SqlConnectionfactory:ISqlConnectionFactory
{
    private readonly string _connectionString;

    public SqlConnectionfactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection CreateConnection()
    {
        var connection= new SqlConnection(_connectionString);
        connection.Open();
        return connection;
    }
}