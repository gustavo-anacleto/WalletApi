namespace WalletApi.Application.Exceptions;

public class NotFoundException(string message, int statusCode = 404) : ApiException(message, statusCode)
{
}