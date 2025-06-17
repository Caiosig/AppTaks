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

        [HttpPost("Create-User")]
        public async Task<ActionResult<UserInfoViewModel>> CreateUser(CreateUserCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(); // Substitua isso pela lógica real de criação de usuário.
        }
    }
}
