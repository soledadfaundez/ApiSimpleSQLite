using Customer.Management.Api.Models.Customer;
using Customer.Management.Api.Models.User;
using Customer.Management.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Customer.Management.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private IUserService _userServiceService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AccountController> _logger;

    public AccountController(IUserService userService, IConfiguration configuration, ILogger<AccountController> logger)
    {
        _userServiceService = userService;
        _configuration = configuration;
        _logger = logger;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        await _userServiceService.Create(model);
        return Ok(new { message = "User created" });
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        try { 

            var user = await _userServiceService.GetByUserName(model.Username);

            if (user == null)
                return NotFound();

            if (!BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
                return Unauthorized();
        
            //ESFC: Registrar los claims, se agregarían roles pero en este caso lo hacemos simple.
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username)
            };

            //ESFC: Registrar los claims, se agregarían roles pero en este caso lo hacemos simple.
            //foreach (var userRole in userRoles)
            //{
            //    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            //}

            //ESFC: Creamos el JWT con duración de 3 horas
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
            var token = new JwtSecurityToken(
                        issuer: _configuration.GetSection("JWT:ValidIssuer").Value,
                        audience: _configuration.GetSection("JWT:ValidAudience").Value,
                        expires: DateTime.Now.AddHours(3),
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

            //ESFC: Retornamos el JWT
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });

        }
        catch (Exception ex)
        {
            _logger.LogError(message: ex.Message, ex);
            throw;
        }
    }
}
