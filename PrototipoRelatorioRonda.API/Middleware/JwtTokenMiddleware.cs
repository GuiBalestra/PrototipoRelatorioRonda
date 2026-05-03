using System.Text.RegularExpressions;

namespace PrototipoRelatorioRonda.API.Middleware;

/// <summary>
/// Middleware que garante que o header Authorization tenha o prefixo "Bearer " antes do token.
/// Isso permite que o usuário cole apenas o token JWT no Swagger sem o prefixo.
/// </summary>
public class JwtTokenMiddleware
{
    private readonly RequestDelegate _next;
    private static readonly Regex JwtPattern = new(@"^[A-Za-z0-9-_]+\.[A-Za-z0-9-_]+\.[A-Za-z0-9-_]*$");

    public JwtTokenMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var authHeader = context.Request.Headers.Authorization.FirstOrDefault();

        if (!string.IsNullOrEmpty(authHeader))
        {
            // Se não começa com "Bearer " mas parece um JWT (3 partes separadas por .)
            if (!authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase) &&
                JwtPattern.IsMatch(authHeader.Trim()))
            {
                context.Request.Headers.Authorization = $"Bearer {authHeader.Trim()}";
            }
        }

        await _next(context);
    }
}
