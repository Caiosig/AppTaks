using Application.Mappings;
using Application.UserCQ.Commands;
using Application.UserCQ.Validators;
using Domain.Abstractions;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infra.Persistence;
using Infra.Repository.IRepositories;
using Infra.Repository.Repositories;
using Infra.Repository.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Services.AuthServices;
using System.Reflection;
using System.Text;

namespace APITasksApp.Extensions
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

        public static void AddJwtAuth(this WebApplicationBuilder builder)
        {
            // Configura a autenticação JWT, permitindo que a API valide tokens JWT para autenticação de usuários.
            var configuration = builder.Configuration;

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // Valida se o emissor do token (Issuer) é o esperado, aumentando a segurança.
                    ValidateIssuer = true,
                    // Valida se o público do token (Audience) é o esperado, garantindo que o token é destinado à aplicação correta.
                    ValidateAudience = true,
                    // Valida se o token ainda está dentro do período de validade (expiração).
                    ValidateLifetime = true,
                    // Define o emissor válido esperado para os tokens JWT.
                    ValidIssuer = configuration["JWT:Issuer"],
                    // Define o público válido esperado para os tokens JWT.
                    ValidAudience = configuration["JWT:Audience"],
                    // Define a chave de assinatura utilizada para validar a integridade do token JWT.
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]!))
                };
            }).AddCookie(
                options =>
                {
                    // Define o cookie como HttpOnly, impedindo acesso via JavaScript e aumentando a segurança contra ataques XSS.
                    options.Cookie.HttpOnly = true;
                    // Garante que o cookie só será transmitido em conexões HTTPS.
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Força o uso de cookies seguros
                    // Restringe o envio do cookie apenas para requisições do mesmo site, prevenindo ataques CSRF.
                    options.Cookie.SameSite = SameSiteMode.Strict;
                    // Define o tempo de expiração do cookie para 2 dias.
                    options.ExpireTimeSpan = TimeSpan.FromDays(2);
                    // Habilita a renovação automática do tempo de expiração do cookie a cada requisição válida.
                    options.SlidingExpiration = true;
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

        public static void AddInjections(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IAuthService, AuthService>();
        }

        public static void AddRepositories(this WebApplicationBuilder builder)
        {
            // Registra os repositórios necessários para acessar as entidades do banco de dados
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IWorkSpaceRepository, WorkSpaceRepository>();

        }
    }
}