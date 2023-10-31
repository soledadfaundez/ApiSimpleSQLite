namespace Customer.Management.Api.Repositories;

using Dapper;
using Customer.Management.Api.Entities;
using Customer.Management.Api.Helpers;

public class CustomerRepository : ICustomerRepository
{
    private DataContext _context;

    public CustomerRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Customer>> GetAll()
    {
        using var connection = _context.CreateConnection();
        var sql = """
            SELECT * FROM Customer
        """;
        return await connection.QueryAsync<Customer>(sql);
    }

    public async Task<Customer> GetById(int id)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            SELECT * FROM Customer 
            WHERE Id = @id
        """;
        return await connection.QuerySingleOrDefaultAsync<Customer>(sql, new { id });
    }

    public async Task<Customer> GetByEmail(string email)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            SELECT * FROM Customer 
            WHERE Email = @email
        """;
        return await connection.QuerySingleOrDefaultAsync<Customer>(sql, new { email });
    }

    public async Task Create(Customer user)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            INSERT INTO Customer (FirstName, LastName, Email, Type, PasswordHash)
            VALUES (@FirstName, @LastName, @Email, @Type, @PasswordHash)
        """;
        await connection.ExecuteAsync(sql, user);
    }

    public async Task Update(Customer user)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            UPDATE Customer 
            SET FirstName = @FirstName,
                LastName = @LastName, 
                Email = @Email, 
                Type = @Type, 
                PasswordHash = @PasswordHash
            WHERE Id = @Id
        """;
        await connection.ExecuteAsync(sql, user);
    }

    public async Task Delete(int id)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            DELETE FROM Customer 
            WHERE Id = @id
        """;
        await connection.ExecuteAsync(sql, new { id });
    }
}