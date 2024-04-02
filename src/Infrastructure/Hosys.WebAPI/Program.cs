using System.Reflection;
using System.Text;
using Hosys.Application.Interfaces.Security.Hash;
using Hosys.Application.Interfaces.Security.Text;
using Hosys.Application.Interfaces.UseCases;
using Hosys.Application.UseCases;
using Hosys.Domain.Interfaces.User;
using Hosys.Persistence;
using Hosys.Persistence.Repositories.User;
using Hosys.Security.Hash;
using Hosys.Security.Text;
using Hosys.Services.Jwt.Handle;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

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

// Add JWT
builder.Services.AddAuthentication(options => 
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => 
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = config["Security:Jwt:Issuer"],
        ValidAudience = config["Security:Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Security:Jwt:Secret"]! ?? throw new Exception("Security:Jwt:Secret is missing."))),
        ClockSkew = TimeSpan.Zero
    };
});
builder.Services.AddAuthorization(options => 
{
    options.AddPolicy("Admin", policy => policy.RequireRole("ADMIN"));
    options.AddPolicy("User", policy => policy.RequireRole("USER"));
});

// Add database connection
builder.Services.AddSingleton(
    new Database(config["ConnectionStrings:DefaultConnection"]!)
    );

// Add use cases
builder.Services.AddScoped<IUserUseCases, UserUseCases>();

// Add repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserRecoveryRepository, UserRecoveryRepository>();

// Add security layers
builder.Services.AddSingleton<IHash>(new Argon2Hash(
    config["Security:Argon2:Salt"]!,
    config["Security:Argon2:Secret"]!,
    config["Security:Argon2:AssociatedData"]!
    ));
builder.Services.AddScoped<ITextSecurityAnalyzer, TextSecurityAnalyzer>();

// Add services
builder.Services.AddSingleton(new JwtService(
    config["Security:Jwt:Secret"]!,
    config["Security:Jwt:Issuer"]!,
    config["Security:Jwt:Audience"]!,
    int.Parse(config["Security:Jwt:ExpireIn"]!)
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
