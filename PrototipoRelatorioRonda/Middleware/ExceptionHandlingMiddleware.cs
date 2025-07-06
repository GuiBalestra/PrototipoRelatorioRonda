using PrototipoRelatorioRonda.Exceptions;
using System.Net;
using System.Text.Json;

namespace PrototipoRelatorioRonda.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        object errorResponse;

        switch (exception)
        {
            case NotFoundException:
                response.StatusCode = (int)HttpStatusCode.NotFound;
                errorResponse = new { message = exception.Message };
                break;

            case BusinessException:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse = new { message = exception.Message };
                break;

            case InvalidOperationException:
                response.StatusCode = (int)HttpStatusCode.Conflict;
                errorResponse = new { message = exception.Message };
                break;

            case KeyNotFoundException:
                response.StatusCode = (int)HttpStatusCode.NotFound;
                errorResponse = new { message = exception.Message };
                break;

            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                _logger.LogError(exception, "Erro não tratado: {Message}", exception.Message);
                errorResponse = new
                {
                    message = "Ocorreu um erro interno. Por favor, tente novamente mais tarde.",
                    error = exception.Message
                };
                break;
        }

        var result = JsonSerializer.Serialize(errorResponse);
        await response.WriteAsync(result);
    }
} 
