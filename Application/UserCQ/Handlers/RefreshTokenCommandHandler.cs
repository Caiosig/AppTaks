using Application.Response;
using Application.UserCQ.Commands;
using Application.UserCQ.ViewModels;
using AutoMapper;
using Domain.Abstractions;
using Infra.Repository.UnitOfWork;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Application.UserCQ.Handlers
{
    /// <summary>
    /// Handler responsável por processar o comando de renovação de token (refresh token).
    /// Valida o refresh token recebido, gera novos tokens e retorna as informações do usuário.
    /// </summary>
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, ResponseBase<RefreshTokenViewModel>>
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
        public RefreshTokenCommandHandler(IUnitOfWork unitOfWork, IAuthService authService, IConfiguration configuration, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _authService = authService;
            _configuration = configuration;
            _mapper = mapper;
        }

        /// <summary>
        /// Processa o comando de renovação de token.
        /// Valida o refresh token, gera novos tokens e retorna as informações do usuário.
        /// </summary>
        /// <param name="request">Comando contendo o username e o refresh token.</param>
        /// <param name="cancellationToken">Token de cancelamento da operação.</param>
        /// <returns>Resposta contendo os novos tokens e dados do usuário, ou erro de validação.</returns>
        public async Task<ResponseBase<RefreshTokenViewModel>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            // Busca o usuário pelo nome de usuário informado.
            var user = await _unitOfWork.UserRepository.Get(x => x.UserName == request.Username);

            // Valida se o usuário existe, se o refresh token é válido e se não está expirado.
            if (user is null || user.RefreshToken != request.RefreshToken || user.RefreashTokenExpirationTime < DateTime.Now)
            {
                return new ResponseBase<RefreshTokenViewModel>
                {
                    ResponseInfo = new()
                    {
                        Title = "Token inválido.",
                        ErrorDescription = $"Refresh Token inválido ou expirado. Faça login novamente.",
                        HTTPStatus = 400
                    },
                    Value = null
                };
            }

            // Gera um novo refresh token para o usuário.
            user.RefreshToken = _authService.GenerateRefreshJWT();

            // Obtém o tempo de expiração do refresh token a partir da configuração.
            _ = int.TryParse(_configuration["JWT:RefreshTokenExpirationTimeInDays"], out int refreshTokenExpirationTimeInDays);

            // Atualiza o tempo de expiração do refresh token.
            user.RefreashTokenExpirationTime = DateTime.Now.AddDays(refreshTokenExpirationTimeInDays);
            _unitOfWork.Commit();

            // Mapeia o usuário para o ViewModel de resposta.
            var refreshTokenVM = _mapper.Map<RefreshTokenViewModel>(user);
            // Gera um novo token JWT para o usuário.
            refreshTokenVM.TokenJWT = _authService.GenerateJWT(user.Email!, user.UserName!);

            // Retorna resposta de sucesso com os novos tokens e dados do usuário.
            return new ResponseBase<RefreshTokenViewModel>
            {
                ResponseInfo = null,
                Value = refreshTokenVM
            };
        }
    }
}
