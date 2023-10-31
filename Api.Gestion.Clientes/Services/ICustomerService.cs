using Customer.Management.Api.Models.Customer;
namespace Customer.Management.Api.Services;
using Customer.Management.Api.Entities;

public interface ICustomerService
{
    Task<IEnumerable<Customer>> GetAll();
    Task<Customer> GetById(int id);
    Task Create(CreateRequest model);
    Task Update(int id, UpdateRequest model);
    Task Delete(int id);
}
