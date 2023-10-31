namespace Customer.Management.Api.Helpers;

using System.Data;
using Dapper;
using Microsoft.Data.Sqlite;

public class DataContext
{
    protected readonly IConfiguration Configuration;

    public DataContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IDbConnection CreateConnection()
    {
        return new SqliteConnection(Configuration.GetConnectionString("WebApiDatabase"));
    }

    public async Task Init()
    {
        // create database tables if they don't exist
        using var connection = CreateConnection();
        await _initCustomer();
        await _initUsers();

        async Task _initCustomer()
        {
            var sql = """
                CREATE TABLE IF NOT EXISTS 
                Customer (
                    Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                    FirstName TEXT,
                    LastName TEXT,
                    Email TEXT,
                    Type INTEGER,
                    PasswordHash TEXT
                );
            """;
            await connection.ExecuteAsync(sql);
        }

        async Task _initUsers()
        {
            var sql = """
                CREATE TABLE IF NOT EXISTS 
                UserApi (
                    Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                    Username TEXT,
                    PasswordHash TEXT                   
                );
            """;
            await connection.ExecuteAsync(sql);
        }
    }
}