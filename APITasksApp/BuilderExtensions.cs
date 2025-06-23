using Application.Mappings;
using Application.UserCQ.Commands;
using Application.UserCQ.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infra.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace APITasksApp
{
    public static class BuilderExtensions
    {
        public static void AddSwaggerDocs(this WebApplicationBuilder builder)
        {
            // Configura o Swagger para gerar documentação da API, facilitando o teste e a exploração dos endpoints.
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Tasks App API",
                    Version = "V1",
                    Description = "Um aplicativo de tarefas baseados no Trello e escrita em ASP.NET Core V8",
                    Contact = new OpenApiContact
                    {
                        Name = "Caio da Silva Godinho",
                        Url = new Uri("https://example.com/contact")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT License",
                        Url = new Uri("https://example.com/license")
                    }
                });

                var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName));
            });
        }

        public static void AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

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

        public static void AddMappings(this WebApplicationBuilder builder)
        {
            // Registra os mapeamentos do AutoMapper, permitindo a conversão automática entre entidades e modelos de visualização
            builder.Services.AddAutoMapper(typeof(ProfileMappings).Assembly);
        }
    }
}