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
        ///  <response code="200">Retorna os dados do usuário criado</response>
        ///  <response code="400">Se houver erros de validação no comando</response>
        [HttpPost("Create-User")]
        [ProducesResponseType(typeof(UserInfoViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserInfoViewModel>> CreateUser(CreateUserCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}
