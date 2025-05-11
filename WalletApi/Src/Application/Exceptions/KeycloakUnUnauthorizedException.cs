namespace WalletApi.Application.Exceptions;

public class KeycloakUnUnauthorizedException(string message, int statusCode = 401) : ApiException(message, statusCode)
{
}