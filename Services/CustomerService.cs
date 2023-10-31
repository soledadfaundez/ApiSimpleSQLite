namespace Customer.Management.Api.Services;

using AutoMapper;
using BCrypt.Net;
using Customer.Management.Api.Entities;
using Customer.Management.Api.Helpers;
using Customer.Management.Api.Models.Customer;
using Customer.Management.Api.Repositories;

public class CustomerService : ICustomerService
{
    private ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CustomerService> _logger;

    //ESFC: Constructor 
    public CustomerService(ICustomerRepository customerRepository, IMapper mapper, ILogger<CustomerService> logger)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<Customer>> GetAll()
    {
        try
        {
            return await _customerRepository.GetAll();
        }
        catch (Exception ex)
        {
            _logger.LogError(message: ex.Message, ex);
            throw;
        }
    }

    public async Task<Customer> GetById(int id)
    {
        try 
        { 
            var user = await _customerRepository.GetById(id);

            if (user == null)
                throw new KeyNotFoundException("Customer not found");

            return user;

        }
        catch (Exception ex)
        {
            _logger.LogError(message: ex.Message, ex);
            throw;
        }
    }

    public async Task Create(CreateRequest model)
    {
        try
        {
            //ESFC: validar
            if (await _customerRepository.GetByEmail(model.Email!) != null)
            throw new AppException("Customer with the email '" + model.Email + "' already exists");

            //ESFC: Mapear el DTO a la Entidad
            var customer = _mapper.Map<Customer>(model);

            //ESFC: Encriptar la password
            customer.PasswordHash = BCrypt.HashPassword(model.Password);

            //ESFC: Guardar customer
            await _customerRepository.Create(customer);

            _logger.LogInformation($"Create OK customer {customer.FirstName}");
        }
        catch (Exception ex)
        {
            _logger.LogError(message: ex.Message, ex);
            throw;
        }

    }

    public async Task Update(int id, UpdateRequest model)
    {

        try
        {
            var user = await _customerRepository.GetById(id);

            if (user == null)
                throw new KeyNotFoundException("Customer not found");

            // validate
            var emailChanged = !string.IsNullOrEmpty(model.Email) && user.Email != model.Email;
            if (emailChanged && await _customerRepository.GetByEmail(model.Email!) != null)
                throw new AppException("Customer with the email '" + model.Email + "' already exists");

            // hash password if it was entered
            if (!string.IsNullOrEmpty(model.Password))
                user.PasswordHash = BCrypt.HashPassword(model.Password);

            // copy model props to user
            _mapper.Map(model, user);

            // save Customer
            await _customerRepository.Update(user);

        }
        catch (Exception ex)
        {
            _logger.LogError(message: ex.Message, ex);
            throw;
        }
    }

    public async Task Delete(int id)
    {
        try
        {
            await _customerRepository.Delete(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(message: ex.Message, ex);
            throw;
        }
    }
}