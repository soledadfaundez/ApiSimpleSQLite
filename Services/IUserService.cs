using Customer.Management.Api.Entities;
using Customer.Management.Api.Models.Customer;
using Customer.Management.Api.Models.User;

namespace Customer.Management.Api.Services;

public interface IUserService
{
    Task<UserApi> GetByUserName(string userName);
    Task Create(RegisterModel model);
}


