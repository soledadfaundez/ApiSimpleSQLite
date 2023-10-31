namespace Customer.Management.Api.Repositories;

using Dapper;
using Customer.Management.Api.Entities;
using Customer.Management.Api.Helpers;

public class UserRepository : IUserRepository
{
    private DataContext _context;

    public UserRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<UserApi> GetByUserName(string userName)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            SELECT * FROM UserApi 
            WHERE Username = @userName
        """;
        return await connection.QuerySingleOrDefaultAsync<UserApi>(sql, new { userName });
    }

    public async Task Create(UserApi user)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            INSERT INTO UserApi (Username, PasswordHash)
            VALUES (@Username, @PasswordHash)
        """;
        await connection.ExecuteAsync(sql, user);
    }

 
}