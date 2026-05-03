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
    opts.UseLazyLoadingProxies().UseSqlServer(connectionString, sqlOpts =>
        sqlOpts.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null));
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
var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!);
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
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddControllers().AddJsonOptions(opts =>
{
    opts.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opts =>
{
    opts.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Prototipo Relatório de Ronda",
        Version = "v1"
    });

    // Lê comentários XML para documentação
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        opts.IncludeXmlComments(xmlPath);

    // Configuração JWT para Swagger - usuário cola apenas o token
    opts.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "Cole apenas o token JWT (sem o prefixo 'Bearer ')",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    opts.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

var app = builder.Build();

// Seed do banco de dados
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<RelatorioRondaContext>();
    await DatabaseSeed.SeedAsync(context);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opts =>
    {
        opts.SwaggerEndpoint("/swagger/v1/swagger.json", "Prototipo Relatório de Ronda v1");
    });
}

app.UseHttpsRedirection();

app.UseMiddleware<JwtTokenMiddleware>();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
