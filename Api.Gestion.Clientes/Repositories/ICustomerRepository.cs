namespace Customer.Management.Api.Repositories;
using Customer.Management.Api.Entities;

public interface ICustomerRepository
{
    Task<IEnumerable<Customer>> GetAll();
    Task<Customer> GetById(int id);
    Task<Customer> GetByEmail(string email);
    Task Create(Customer user);
    Task Update(Customer user);
    Task Delete(int id);
}

