using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WalletApi.Application.Client;
using WalletApi.Application.Exceptions;
using WalletApi.Domain.Entity;
using WalletApi.Domain.Enums;
using WalletApi.Infrastructure.Configuration;
using WalletApi.Presentation.DTO;
using ApiException = Refit.ApiException;

namespace WalletApi.Application.Service;

public class UserService(
    AppDbContext context,
    IKeycloakClient keycloakClient,
    TokenService tokenService,
    IMapper mapper,
    IConfiguration configuration)
{
    private readonly string _realm = configuration["Keycloak:Realm"]!;
    private readonly string _clientSecret = configuration["Keycloak:ClientSecret"]!;
    private readonly string _clientId = configuration["Keycloak:ClientId"]!;
    private readonly string _grantType = configuration["Keycloak:GrantType"]!;
    private readonly string _adminsGroupName = configuration["Keycloak:AdminsGroup"]!;
    
    public async Task<TokenDto> Longin(LoginDto dto)
    {
        try
        {
            var formData = new Dictionary<string, string>
            {
                { "grant_type", _grantType },
                { "client_id", _clientId },
                { "client_secret", _clientSecret },
                { "username", dto.Email },
                { "password", dto.Password }
            };

            return await keycloakClient.Login(formData, _realm);
        }
        catch (ApiException e)
        {
            throw new KeycloakUnUnauthorizedException(e.Message);
        }
    }

    public async Task<User> FindTokenUser()
    {
        var email = tokenService.GetUserEmailFromToken();
        return await context.Users.FirstAsync(u => u.Email == email);
    }

    public async Task CreateUser(UserCreateDto dto)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            var user = mapper.Map<User>(dto);

            user.Wallet = new Wallet
            {
                Balance = (decimal) 0.0
            };
            
            context.Users.Add(user);
            
            await context.SaveChangesAsync();

            await CreateKeycloakUser(dto);

            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    
    public async Task<List<UserListDto>> FindAll()
    {
        var users = await context.Users.Include(u => u.Wallet).ToListAsync();
        return mapper.Map<List<UserListDto>>(users);
    }

    private async Task CreateKeycloakUser(UserCreateDto dto)
    {
        var keycloakUserDto = mapper.Map<KeycloakUserDto>(dto);

        var token = tokenService.GetToken();
            
        var response =  await keycloakClient.CreateUser(token, keycloakUserDto, _realm);
        
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Error when create keycloak user");
        }
        
        if (dto.UserType == UserType.ADMIN)
        {
            var locationHeader = response.Headers.Location?.ToString();
            var userId = locationHeader?.Split("/").Last();
            
            if (string.IsNullOrEmpty(userId))
            {
                throw new Exception("Cannot obtain user created id");
            }

            await AssociateUserToGroup(userId);
        }
        
    }

    private async Task AssociateUserToGroup(string keycloakUserId)
    {
        var token = tokenService.GetToken();
        var groups = await keycloakClient.GetGroups(token);
        var group = groups.FirstOrDefault(g => g.Name == _adminsGroupName);
        
        if (group == null)
        {
            throw new NotFoundException("Group not found");
        }
        
        await keycloakClient.AddUserToGroup(token, keycloakUserId, group.Id);
    }
}