using AutoMapper;
using Customer.Management.Api.Entities;
using Customer.Management.Api.Helpers;
using Customer.Management.Api.Models.User;
using Customer.Management.Api.Repositories;

namespace Customer.Management.Api.Services;

public class UserService : IUserService
{
    private IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UserService> _logger;

    //ESFC: Constructor 
    public UserService(IUserRepository userRepository, IMapper mapper, ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task Create(RegisterModel model)
    {
        try
        {
            if (await _userRepository.GetByUserName(model.Username!) != null)
                throw new AppException("User with the Username '" + model.Username + "' already exists");

            //ESFC: Mapear el DTO a la Entidad
            var userApi = _mapper.Map<UserApi>(model);

            //ESFC: Hash password
            userApi.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

            // save user
            await _userRepository.Create(userApi);

            _logger.LogInformation($"Create OK user {userApi.Username}");
        }
        catch (Exception ex)
        {
            _logger.LogError(message: ex.Message, ex);
            throw;
        }
        
    }

    public async Task<UserApi> GetByUserName(string userName)
    {
        var user = await _userRepository.GetByUserName(userName);

        if (user == null)
            throw new KeyNotFoundException("User not found");

        return user;
    }
}
