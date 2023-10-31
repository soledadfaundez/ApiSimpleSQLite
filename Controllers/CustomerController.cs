namespace Customer.Management.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using Customer.Management.Api.Models.Customer;
using Customer.Management.Api.Services;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("[controller]")]
public class CustomerController : ControllerBase
{
    private ICustomerService _customerServiceService;

    public CustomerController(ICustomerService customerService)
    {
        _customerServiceService = customerService;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _customerServiceService.GetAll();
        return Ok(users);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _customerServiceService.GetById(id);
        return Ok(user);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(CreateRequest model)
    {
        await _customerServiceService.Create(model);
        return Ok(new { message = "Customer created" });
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateRequest model)
    {
        await _customerServiceService.Update(id, model);
        return Ok(new { message = "Customer updated" });
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _customerServiceService.Delete(id);
        return Ok(new { message = "Customer deleted" });
    }
}