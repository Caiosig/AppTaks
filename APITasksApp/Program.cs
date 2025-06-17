using Application.UserCQ.Commands;
using Infra.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);


var configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Instanciando o DbContext para conectar com o banco de dados
builder.Services.AddDbContext<TasksDbContext>(options =>
options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
// Registra os serviços do MediatR, permitindo a injeção de dependência para handlers de comandos e queries.
// O assembly do comando CreateUserCommand é utilizado para localizar e registrar automaticamente todos os handlers presentes.
builder.Services.AddMediatR(config => config.RegisterServicesFromAssemblies(typeof(CreateUserCommand).Assembly));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
