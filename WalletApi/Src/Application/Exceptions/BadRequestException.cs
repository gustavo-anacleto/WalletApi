namespace WalletApi.Application.Exceptions;

public class BadRequestException(string message, int statusCode = 400) : ApiException(message, statusCode)
{
}