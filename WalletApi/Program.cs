using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Refit;
using WalletApi.Application.Client;
using WalletApi.Application.Middleware;
using WalletApi.Application.Service;
using WalletApi.Infrastructure.Configuration;

var builder = WebApplication.CreateBuilder(args);
var keycloakHost = builder.Configuration["Keycloak:Host"]!;
var keycloakRealm = builder.Configuration["Keycloak:Realm"]!;

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddRefitClient<IKeycloakClient>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(keycloakHost));

// [APPLICATION SERVICES]
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<WalletService>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<TransactionService>();

// [CONFIG RESOURCES]
builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = $"{keycloakHost}realms/{keycloakRealm}";
        options.Audience = "account";
        options.RequireHttpsMetadata = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            NameClaimType = "preferred_username",
            RoleClaimType = ClaimTypes.Role
        };

        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = context =>
            {
                var identity = context.Principal?.Identity as ClaimsIdentity;
                var realmAccessClaim = context.Principal?.FindFirst("realm_access");
                if (realmAccessClaim == null) return Task.CompletedTask;
                var realmAccessJson = JsonDocument.Parse(realmAccessClaim.Value);
                if (!realmAccessJson.RootElement.TryGetProperty("roles", out var roles)) return Task.CompletedTask;
                foreach (var role in roles.EnumerateArray())
                {
                    identity?.AddClaim(new Claim(ClaimTypes.Role, role.GetString() ?? string.Empty));
                }

                return Task.CompletedTask;
            }
        };
    });
builder.Services.AddAuthorization();

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();


var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();