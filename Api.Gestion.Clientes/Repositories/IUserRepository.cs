using Customer.Management.Api.Entities;

namespace Customer.Management.Api.Repositories;

public interface IUserRepository
{
    Task<UserApi> GetByUserName(string userName);

    Task Create(UserApi user);
}
