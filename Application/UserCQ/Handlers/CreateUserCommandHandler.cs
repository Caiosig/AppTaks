using Application.Response;
using Application.UserCQ.Commands;
using Application.UserCQ.ViewModels;
using AutoMapper;
using Azure;
using Domain.Abstractions;
using Domain.Entity;
using Domain.Enum;
using Infra.Persistence;
using MediatR;

namespace Application.UserCQ.Handlers
{
    public class CreateUserCommandHandler(TasksDbContext context, IMapper mapper, IAuthService authService) : IRequestHandler<CreateUserCommand, ResponseBase<RefreshTokenViewModel?>>
    {
        // Contexto do banco de dados para acessar as entidades de usuário.
        private readonly TasksDbContext _context = context;
        // Mapeador para converter entre entidades e ViewModels.
        private readonly IMapper _mapper = mapper;

        // Serviço de autenticação injetado via injeção de dependência.
        // Permite gerar tokens JWT e acessar funcionalidades relacionadas à autenticação de usuários.
        private readonly IAuthService _authService = authService;

        // Método responsável por tratar o comando de criação de usuário.
        // Recebe o comando CreateUserCommand e retorna um RefreshTokenViewModel.
        // Deve ser implementado para realizar a lógica de criação de usuário.
        async Task<ResponseBase<RefreshTokenViewModel>> IRequestHandler<CreateUserCommand, ResponseBase<RefreshTokenViewModel>>.Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var isUniqueEmailAndUsername = _authService.UniqueEmailAbdUserName(request.Email!, request.Username!);

            if (isUniqueEmailAndUsername is ValidationFielUserEnum.EmailUnavailable)
            {
                return new ResponseBase<RefreshTokenViewModel>
                {
                    ResponseInfo = new ()
                    {
                        Title = "Email já cadastrado.",
                        ErrorDescription = "O email apresentado já esta sendo utilizado, por favor, tente outro e-mail.",
                        HTTPStatus = 400
                    },
                    Value = null
                };
            }

            if (isUniqueEmailAndUsername is ValidationFielUserEnum.UsernameUnavailable)
            {
                return new ResponseBase<RefreshTokenViewModel>
                {
                    ResponseInfo = new()
                    {
                        Title = "UserName já cadastrado.",
                        ErrorDescription = "O UserName apresentado já esta sendo utilizado, por favor, tente outro username.",
                        HTTPStatus = 400
                    },
                    Value = null
                };
            }

            if (isUniqueEmailAndUsername is ValidationFielUserEnum.UsernameAndEmailUnavailable)
            {
                return new ResponseBase<RefreshTokenViewModel>
                {
                    ResponseInfo = new()
                    {
                        Title = "UserName e e-mail indisponíveis.",
                        ErrorDescription = "O UserName e e-mail apresentados já estão sendo utilizado, por favor, tente outros.",
                        HTTPStatus = 400
                    },
                    Value = null
                };
            }
            // Dados de entrada retornados do request de criação de usuário.
            var user = _mapper.Map<User>(request);

            // Gera um refresh token seguro e aleatório para o novo usuário, utilizado para renovação de autenticação.
            user.RefreshToken = _authService.GenerateRefreshJWT();

            //Passando a senha para o metodo, onde irá fazer a criptografia da senha do usuário.
            user.PasswordHash = _authService.HashingPassword(request.Password!);

            // Adiciona o novo usuário ao contexto do banco de dados.
            _context.Users.Add(user);
            // Persiste as alterações no banco de dados, salvando o novo usuário.
            _context.SaveChanges();

            // Mapeia a entidade User para a ViewModel RefreshTokenViewModel, que será retornada na resposta.
            var refreshTokenVM = _mapper.Map<RefreshTokenViewModel>(user);
            // Gera um token JWT para o usuário recém-criado e atribui à ViewModel.
            refreshTokenVM.TokenJWT = _authService.GenerateJWT(user.Email!, user.UserName!);

            // Dados de saída que serão retornados após a criação do usuário.
            return new ResponseBase<RefreshTokenViewModel>
            {
                ResponseInfo = null,
                // Mapeia o usuário criado para a ViewModel RefreshTokenViewModel.
                Value = refreshTokenVM
            };
        }
    }
}
