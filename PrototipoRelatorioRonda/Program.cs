using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PrototipoRelatorioRonda.Data;
using PrototipoRelatorioRonda.Data.Interface;
using PrototipoRelatorioRonda.Data.Repositories;
using PrototipoRelatorioRonda.Middleware;
using PrototipoRelatorioRonda.Services;
using PrototipoRelatorioRonda.Services.Interface;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("RelatorioRondaConnection");

builder.Services.AddDbContext<RelatorioRondaContext>(opts =>
{
    opts.UseLazyLoadingProxies().UseSqlServer(connectionString);
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

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

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(opts =>
{
    opts.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(s =>
{
    s.SwaggerDoc("v1", new OpenApiInfo { Title = "RelatórioRondaApi", Version = "v1" });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    s.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Middleware de tratamento de exceções
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
