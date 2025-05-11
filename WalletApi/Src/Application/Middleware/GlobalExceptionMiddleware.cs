using System.Net;
using System.Text.Json;
using WalletApi.Application.Exceptions;
using WalletApi.Presentation.DTO;

namespace WalletApi.Application.Middleware;

public class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (KeycloakUnUnauthorizedException ex)
        {
            logger.LogError(ex, ex.Message);
            context.Response.StatusCode = ex.StatusCode;
            context.Response.ContentType = "application/json";

            var errorResponse = new ErrorResponseDto(
                StatusCode: context.Response.StatusCode,
                Message: ex.Message,
                Trace: ex.StackTrace
            );

            var result = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(result);
        }
        catch (BadRequestException ex)
        {
            logger.LogError(ex, ex.Message);
            context.Response.StatusCode = ex.StatusCode;
            context.Response.ContentType = "application/json";

            var errorResponse = new ErrorResponseDto(
                StatusCode: context.Response.StatusCode,
                Message: ex.Message,
                Trace: ex.StackTrace
            );

            var result = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ocorreu um erro não tratado.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var errorResponse = new ErrorResponseDto(
            StatusCode: context.Response.StatusCode,
            Message: exception.Message,
            Trace: exception.StackTrace
        );

        return context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
    }
}