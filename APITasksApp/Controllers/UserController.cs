using Application.UserCQ.Commands;
using Application.UserCQ.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace APITasksApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController(IMediator mediator) : ControllerBase
    {
        // O construtor recebe uma instância de IMediator, que é usada para enviar comandos e consultas.
        private readonly IMediator _mediator = mediator;

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
        ///  <response code="200">Retorna os dados do usuário criado.</response>
        ///  <response code="400">Se houver erros de validação no comando.</response>
        [HttpPost("Create-User")]
        public async Task<ActionResult<UserInfoViewModel>> CreateUser(CreateUserCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(); // Substitua isso pela lógica real de criação de usuário.
        }
    }
}
