using Microsoft.EntityFrameworkCore;
using PrototipoRelatorioRonda.Domain.Entities;
using PrototipoRelatorioRonda.Domain.Enums;
using System.Security.Cryptography;
using System.Text;

namespace PrototipoRelatorioRonda.Infrastructure.Data;

public static class DatabaseSeed
{
    public static async Task SeedAsync(RelatorioRondaContext context)
    {
        // Garante que o banco está criado
        await context.Database.MigrateAsync();

        // Seed da Empresa
        var empresa = await context.Empresas.FirstOrDefaultAsync(e => e.Nome == "GuiBalestra");
        if (empresa is null)
        {
            empresa = new Empresa { Nome = "GuiBalestra" };
            context.Empresas.Add(empresa);
            await context.SaveChangesAsync();
        }

        // Seed do Usuário Admin
        var adminExiste = await context.Usuarios.AnyAsync(u => u.Email == "gb-ol@hotmail.com");
        if (!adminExiste)
        {
            var usuario = new Usuario
            {
                Nome = "Admin",
                Email = "gb-ol@hotmail.com",
                HashSenha = HashPassword("123456"),
                Funcao = Funcao.Administrador,
                EmpresaId = empresa.Id
            };
            context.Usuarios.Add(usuario);
            await context.SaveChangesAsync();
        }
    }

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }
}
