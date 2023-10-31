using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;
using Customer.Management.Api.Helpers;
using Customer.Management.Api.Repositories;
using Customer.Management.Api.Services;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

{
    var services = builder.Services;
    var env = builder.Environment;
 
    services.AddSingleton<DataContext>();
    services.AddCors();
    services.AddControllers().AddJsonOptions(x =>
    {      
        x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());  
        x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

    //ESFC: Configurar Swagger para que use token
    services.AddSwaggerGen(swagger =>
    {
        //This is to generate the Default UI of Swagger Documentation
        swagger.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "ASP.NET 7 Web API",
            Description = "Authentication and Authorization in ASP.NET 7 with JWT and Swagger"
        });
        // To Enable authorization using Swagger (JWT)
        swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
        });
        swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
            {{ new OpenApiSecurityScheme{
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
            }
            });
    });

    //ESFC: Configurar automaper
    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    //ESFC: Configurar los servicios y repositorios
    services.AddScoped<ICustomerRepository, CustomerRepository>();
    services.AddScoped<ICustomerService, CustomerService>();
    services.AddScoped<IUserRepository, UserRepository>();
    services.AddScoped<IUserService, UserService>();

    //ESFC: Adding Authentication
    services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    // Adding Jwt Bearer
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = builder.Configuration.GetSection("JWT:ValidAudience").Value, 
            ValidIssuer = builder.Configuration.GetSection("JWT:ValidIssuer").Value,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JWT:SecretKey").Value))
        };
    });

    //ESFC: Para usar swagger
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

    //ESFC: Add support to logging with SERILOG
    builder.Host.UseSerilog((context, configuration) =>
        configuration.ReadFrom.Configuration(context.Configuration));
}

var app = builder.Build();

 //ESFC: Inicializar la BD
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<DataContext>();
    await context.Init();
}

{
    //ESFC: global cors policy
    app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

    //ESFC: Global error handler
    app.UseMiddleware<ErrorHandlerMiddleware>();


    
    app.MapControllers();

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseSwagger();
    app.UseSwaggerUI();
}

var frontendURL = builder.Configuration.GetValue<string>("EndPointConfig");
app.Run(frontendURL);