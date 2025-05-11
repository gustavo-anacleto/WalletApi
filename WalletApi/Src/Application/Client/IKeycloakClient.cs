using Refit;
using WalletApi.Presentation.DTO;

namespace WalletApi.Application.Client;

public interface IKeycloakClient
{
    [Post("/realms/{realm}/protocol/openid-connect/token")]
    [Headers("Content-Type: application/x-www-form-urlencoded")]
    Task<TokenDto> Login([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, string> formData, string realm);

    [Post("/admin/realms/{realm}/users")]
    Task<HttpResponseMessage> CreateUser(
        [Header("Authorization")] string bearerToken,
        [Body] KeycloakUserDto user,
        string realm);

    [Get("/admin/realms/wallet_service/groups")]
    Task<List<GroupDto>> GetGroups([Header("Authorization")] string bearerToken);

    [Put("/admin/realms/wallet_service/users/{userId}/groups/{groupId}")]
    Task<HttpResponseMessage> AddUserToGroup(
        [Header("Authorization")] string bearerToken,
        string userId,
        string groupId
    );
}