using System.Reflection;
using Hosys.Application.Interfaces.Security;
using Hosys.Application.Interfaces.UseCases;
using Hosys.Application.UseCases;
using Hosys.Domain.Interfaces.User;
using Hosys.Persistence;
using Hosys.Persistence.Repositories.User;
using Hosys.Security.Hash;

var builder = WebApplication.CreateBuilder(args);
var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables()
    .AddUserSecrets(Assembly.GetExecutingAssembly(), optional: false, reloadOnChange: true)
    .Build();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add database connection
builder.Services.AddSingleton(
    new Database(config["ConnectionStrings:DefaultConnection"]!)
    );

// Add use cases
builder.Services.AddScoped<IUserUseCases, UserUseCases>();

// Add repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Add security layers
builder.Services.AddSingleton<IHash>(new Argon2Hash(
    config["Security:Argon2:Salt"]!,
    config["Security:Argon2:Secret"]!,
    config["Security:Argon2:AssociatedData"]!
    ));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
