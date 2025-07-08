using Application.UserCQ.Commands;
using Application.UserCQ.ViewModels;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace APITasksApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController(IMediator mediator, IConfiguration configuration, IMapper mapper) : ControllerBase
    {
        // O construtor recebe uma instância de IMediator, que é usada para enviar comandos e consultas.
        private readonly IMediator _mediator = mediator;

        private readonly IConfiguration _configuration = configuration;

        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// Rota responsavel por criar um novo usuário.
        /// </summary>
        /// <param name="command">
        /// Um objeto CreateUserCommand
        /// </param>
        /// <returns>Os dados usuário criado</returns>
        /// <remarks>
        /// Exemplo de request:
        /// ```
        /// POST /User/Create-User
        /// {
        ///  "name": "Caio",
        ///  "surname": "Godinho",   
        ///  "username": "caiosig",
        ///  "email": "teste@teste.com",
        ///  "password": "123456"
        /// }
        /// ```
        ///  </remarks>
        ///  <response code="200">Retorna os dados do usuário criado</response>
        ///  <response code="400">Se houver erros de validação no comando</response>
        [HttpPost("Create-User")]
        [ProducesResponseType(typeof(UserInfoViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserInfoViewModel>> CreateUser(CreateUserCommand command)
        {
            // Envia o comando para criar o usuário via MediatR e aguarda a resposta.
            var request = await _mediator.Send(command);

            // Verifica se não houve informações de erro na resposta (ResponseInfo é nulo).
            if (request.ResponseInfo is null)
            {
                // Obtém os dados do usuário criado a partir da resposta.
                var userInfo = request.Value;

                // Se o usuário foi criado com sucesso (userInfo não é nulo).
                if (userInfo is not null)
                {
                    // Configura as opções do cookie para o JWT: HttpOnly, seguro e expira em 2 dias.
                    var cookieOptionsToken = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        Expires = DateTime.UtcNow.AddDays(2)
                    };

                    // Obtém o tempo de expiração do refresh token a partir da configuração.
                    _ = int.TryParse(configuration["JWT:RefreshTokenExpirationTimeInDays"], out int refreshTokenExpirationTimeInDays);

                    // Configura as opções do cookie para o refresh token: HttpOnly, seguro e expira conforme configuração.
                    var cookieOptionsRefreshToken = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        Expires = DateTime.UtcNow.AddDays(refreshTokenExpirationTimeInDays)
                    };

                    // Adiciona o cookie do JWT à resposta HTTP.
                    Response.Cookies.Append("jwt", request.Value!.TokenJWT!, cookieOptionsToken);
                    // Adiciona o cookie do refresh token à resposta HTTP.
                    Response.Cookies.Append("refreshToken", request.Value!.RefreshToken!, cookieOptionsRefreshToken);

                    // Retorna status 200 OK com os dados do usuário criado.
                    return Ok(_mapper.Map<UserInfoViewModel>(request.Value));
                }
            }

            // Se houve erro na criação do usuário, retorna status 400 Bad Request com detalhes.
            return BadRequest(request);
        }
    }
}
