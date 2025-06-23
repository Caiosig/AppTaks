using Application.UserCQ.Commands;
using Application.UserCQ.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infra.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;

namespace APITasksApp
{
    public static class BuilderExtensions
    {
        public static void AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Registra os serviços do MediatR, permitindo a injeção de dependência para handlers de comandos e queries.
            // O assembly do comando CreateUserCommand é utilizado para localizar e registrar automaticamente todos os handlers presentes.
            builder.Services.AddMediatR(config => config.RegisterServicesFromAssemblies(typeof(CreateUserCommand).Assembly));
        }

        public static void AddDbContext(this WebApplicationBuilder builder)
        {
            var configuation = builder.Configuration;
            // Instanciando o DbContext para conectar com o banco de dados
            builder.Services.AddDbContext<TasksDbContext>(options =>
                options.UseSqlServer(configuation.GetConnectionString("DefaultConnection")));
        }

        public static void AddFluentValidation(this WebApplicationBuilder builder)
        {
            //Registra os validadores do FluentValidation, permitindo a validação automática dos comandos e queries
            builder.Services.AddValidatorsFromAssemblyContaining<CreateUserComandValidator>();
            // Configura o FluentValidation para validar automaticamente os modelos de entrada em controladores ASP.NET Core
            builder.Services.AddFluentValidationAutoValidation();
        }
    }
}