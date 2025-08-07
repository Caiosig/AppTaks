using Application.Response;
using Application.UserCQ.Commands;
using Application.UserCQ.ViewModels;
using AutoMapper;
using Domain.Abstractions;
using Infra.Persistence;
using Infra.Repository.UnitOfWork;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;

namespace Application.UserCQ.Handlers
{
    /// <summary>
    /// Handler responsável por processar o comando de login de usuário.
    /// Realiza validação de credenciais, gera tokens de autenticação e retorna informações do usuário.
    /// </summary>
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, ResponseBase<RefreshTokenViewModel>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        /// <summary>
        /// Construtor que injeta as dependências necessárias para o handler.
        /// </summary>
        /// <param name="context">Contexto do banco de dados.</param>
        /// <param name="authService">Serviço de autenticação.</param>
        /// <param name="configuration">Configurações da aplicação.</param>
        /// <param name="mapper">Mapeador para conversão de entidades.</param>
        public LoginUserCommandHandler(IUnitOfWork unitOfWork, IAuthService authService, IConfiguration configuration, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _authService = authService;
            _configuration = configuration;
            _mapper = mapper;
        }

        /// <summary>
        /// Processa o comando de login de usuário.
        /// Busca o usuário pelo e-mail, valida a senha, gera tokens e retorna as informações necessárias.
        /// </summary>
        /// <param name="request">Comando contendo e-mail e senha do usuário.</param>
        /// <param name="cancellationToken">Token de cancelamento da operação.</param>
        /// <returns>Resposta contendo informações do usuário e tokens ou detalhes do erro.</returns>
        public async Task<ResponseBase<RefreshTokenViewModel>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            // Busca o usuário pelo e-mail informado
            var user = await _unitOfWork.UserRepository.Get(x => x.Email == request.Email);

            // Se o usuário não for encontrado, retorna resposta de erro
            if (user is null)
            {
                return new ResponseBase<RefreshTokenViewModel>
                {
                    ResponseInfo = new()
                    {
                        Title = "Usuário não encontrado.",
                        ErrorDescription = "O e-mail informado não está cadastrado.",
                        HTTPStatus = 404
                    },
                    Value = null
                };
            }

            // Verifica se a senha informada corresponde ao hash armazenado
            var passwordHash = _authService.HashingPassword(request.Password!);
            if (user.PasswordHash != passwordHash)
            {
                return new ResponseBase<RefreshTokenViewModel>
                {
                    ResponseInfo = new()
                    {
                        Title = "Senha inválida.",
                        ErrorDescription = "A senha informada está incorreta.",
                        HTTPStatus = 404
                    },
                    Value = null
                };
            }
            
            _ = int.TryParse(_configuration["JWT:RefreshTokenExpirationTimeInDays"], out int refreshTokenExpirationTimeInDays);

            // Gera novo refresh token e JWT
            user.RefreshToken = _authService.GenerateRefreshJWT();
            user.RefreashTokenExpirationTime = DateTime.UtcNow.AddDays(refreshTokenExpirationTimeInDays);

            // Atualiza o usuário no repositório com o novo refresh token e data de expiração
            await _unitOfWork.UserRepository.Update(user);
            _unitOfWork.Commit();

            // Mapeia o usuário para o ViewModel de resposta
            RefreshTokenViewModel refreshTokenVM = _mapper.Map<RefreshTokenViewModel>(user);
            refreshTokenVM.TokenJWT = _authService.GenerateJWT(user.Email!, user.UserName!);

            // Retorna resposta de sucesso com os dados do usuário e tokens
            return new ResponseBase<RefreshTokenViewModel>()
            {
                ResponseInfo = null,
                Value = refreshTokenVM
            };
        }
    }
}
