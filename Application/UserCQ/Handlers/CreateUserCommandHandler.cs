using Application.Response;
using Application.UserCQ.Commands;
using Application.UserCQ.ViewModels;
using AutoMapper;
using Azure;
using Domain.Entity;
using Infra.Persistence;
using MediatR;

namespace Application.UserCQ.Handlers
{
    public class CreateUserCommandHandler(TasksDbContext context, IMapper mapper) : IRequestHandler<CreateUserCommand, ResponseBase<UserInfoViewModel?>>
    {
        // Contexto do banco de dados para acessar as entidades de usuário.
        private readonly TasksDbContext _context = context;
        // Mapeador para converter entre entidades e ViewModels.
        private readonly IMapper _mapper = mapper;

        // Método responsável por tratar o comando de criação de usuário.
        // Recebe o comando CreateUserCommand e retorna um UserInfoViewModel.
        // Deve ser implementado para realizar a lógica de criação de usuário.
        async Task<ResponseBase<UserInfoViewModel>> IRequestHandler<CreateUserCommand, ResponseBase<UserInfoViewModel>>.Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            // Dados de entrada retornados do request de criação de usuário.
            var user = _mapper.Map<User>(request);

            _context.Users.Add(user);
            _context.SaveChanges();

            // Dados de saída que serão retornados após a criação do usuário.
            return new ResponseBase<UserInfoViewModel>
            {
                ResponseInfo = null,
                // Mapeia o usuário criado para a ViewModel UserInfoViewModel.
                Value = _mapper.Map<UserInfoViewModel>(user)
            };
        }
    }
}
