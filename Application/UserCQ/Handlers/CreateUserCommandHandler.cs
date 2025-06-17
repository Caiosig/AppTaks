using Application.Response;
using Application.UserCQ.Commands;
using Application.UserCQ.ViewModels;
using Azure;
using Domain.Entity;
using Infra.Persistence;
using MediatR;

namespace Application.UserCQ.Handlers
{
    public class CreateUserCommandHandler(TasksDbContext context) : IRequestHandler<CreateUserCommand, ResponseBase<UserInfoViewModel?>>
    {
        // Contexto do banco de dados para acessar as entidades de usuário.
        private readonly TasksDbContext _context = context;

        // Método responsável por tratar o comando de criação de usuário.
        // Recebe o comando CreateUserCommand e retorna um UserInfoViewModel.
        // Deve ser implementado para realizar a lógica de criação de usuário.
        async Task<ResponseBase<UserInfoViewModel>> IRequestHandler<CreateUserCommand, ResponseBase<UserInfoViewModel>>.Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            // Dados de entrada retornados do request de criação de usuário.
            var user = new User()
            {
                Name = request.Name,
                SurName = request.Surname,
                Email = request.Email,
                PasswordHash = request.Password,
                UserName = request.Username,
                RefreshToken = Guid.NewGuid().ToString(),
                RefreashTokenExpirationTime = DateTime.Now.AddDays(5)
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            // Dados de saída que serão retornados após a criação do usuário.
            return new ResponseBase<UserInfoViewModel>
            {
                ResponseInfo = null,
                Value = new()
                {
                    Name = user.Name,
                    Surname = user.SurName,
                    Email = user.Email,
                    Username = user.UserName,
                    RefreshToken = user.RefreshToken,
                    RefreashTokenExpirationTime = user.RefreashTokenExpirationTime,
                    TokenJWT = Guid.NewGuid().ToString() // Simulando a geração de um token JWT 
                }
            };
        }
    }
}
