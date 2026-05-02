using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using PrototipoRelatorioRonda.Infrastructure.Data;
using PrototipoRelatorioRonda.Application.Interfaces;
using PrototipoRelatorioRonda.Infrastructure.Repositories;
using PrototipoRelatorioRonda.API.Middleware;
using PrototipoRelatorioRonda.Application.Services;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("RelatorioRondaConnection");

builder.Services.AddDbContext<RelatorioRondaContext>(opts =>
{
    opts.UseLazyLoadingProxies().UseSqlServer(connectionString);
});

builder.Services.AddAutoMapper(typeof(Program).Assembly);

// Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Repositories
builder.Services.AddScoped<IEmpresaRepository, EmpresaRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IRelatorioRondaRepository, RelatorioRondaRepository>();
builder.Services.AddScoped<IVoltaRondaRepository, VoltaRondaRepository>();

// Services
builder.Services.AddScoped<IEmpresaService, EmpresaService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IRelatorioRondaService, RelatorioRondaService>();
builder.Services.AddScoped<IVoltaRondaService, VoltaRondaService>();

// JWT Authentication
var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "ChaveSecretaTemporariaParaTestes123456789");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "PrototipoRelatorioRonda",
        ValidAudience = builder.Configuration["Jwt:Audience"] ?? "PrototipoRelatorioRonda",
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddControllers().AddJsonOptions(opts =>
{
    opts.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
