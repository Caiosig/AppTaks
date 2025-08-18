using Application.WorkspaceCQ.Commands;
using Application.WorkspaceCQ.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace APITasksApp.Controllers
{
    /// <summary>
    /// Controlador de rotas para operações relacionadas a Workspaces.
    /// Define endpoints para criação, edição, exclusão, consulta e listagem de workspaces utilizando o padrão minimal API.
    /// Todas as operações utilizam o MediatR para delegar a lógica de negócio aos respectivos handlers de comando.
    /// </summary>
    public static class WorkSpacesController
    {
        /// <summary>
        /// Registra as rotas relacionadas a Workspaces no pipeline da aplicação.
        /// </summary>
        /// <param name="app">Instância do WebApplication.</param>
        public static void WorkSpacesRoutes(this WebApplication app)
        {
            var group = app.MapGroup("Workspaces").WithTags("WorkSpaces");

            group.MapPost("create-workspace", CreateWorkspace);
            group.MapPut("edit-workspace", EditWorkspace);
            group.MapDelete("delete-workspace/{workspaceId}", DeleteWorkspace);
            group.MapGet("get-workspace/{workspaceId}", GetWorkspace);
            group.MapGet("get-all-workspace", GetAllWorkspace);
        }

        /// <summary>
        /// Endpoint para criar um novo workspace.
        /// </summary>
        /// <param name="_mediator">Instância do MediatR para envio do comando.</param>
        /// <param name="command">Comando de criação do workspace.</param>
        /// <returns>Resultado da operação, incluindo o workspace criado ou erro de validação.</returns>
        public static async Task<IResult> CreateWorkspace([FromServices] IMediator _mediator, [FromBody] CreateWorkspaceCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.ResponseInfo is null)
            {
                return Results.Ok(result.Value);
            }

            return Results.BadRequest(result.ResponseInfo);
        }

        /// <summary>
        /// Endpoint para editar um workspace existente.
        /// </summary>
        /// <param name="_mediator">Instância do MediatR para envio do comando.</param>
        /// <param name="editWorkspaceCommand">Comando de edição do workspace.</param>
        /// <returns>Resultado da operação, incluindo o workspace editado ou erro de validação.</returns>
        public static async Task<IResult> EditWorkspace([FromServices] IMediator _mediator, [FromBody] EditWorkspaceCommand editWorkspaceCommand)
        {
            var result = await _mediator.Send(editWorkspaceCommand);

            if (result.ResponseInfo is null)
            {
                return Results.Ok(result.Value);
            }

            return Results.BadRequest(result.ResponseInfo);
        }

        /// <summary>
        /// Endpoint para excluir um workspace pelo identificador.
        /// </summary>
        /// <param name="_mediator">Instância do MediatR para envio do comando.</param>
        /// <param name="workspaceId">Identificador do workspace a ser excluído.</param>
        /// <returns>Resultado da operação, sem conteúdo em caso de sucesso ou erro de validação.</returns>
        public static async Task<IResult> DeleteWorkspace([FromServices] IMediator _mediator, Guid workspaceId)
        {
            var result = await _mediator.Send(new DeleteWorkSpaceCommand
            {
                Id = workspaceId
            });

            if (result.ResponseInfo is null)
            {
                return Results.NoContent();
            }

            return Results.BadRequest(result.ResponseInfo);
        }

        /// <summary>
        /// Endpoint para obter um workspace pelo identificador.
        /// </summary>
        /// <param name="_mediator">Instância do MediatR para envio do comando.</param>
        /// <param name="workspaceId">Identificador do workspace a ser consultado.</param>
        /// <returns>Resultado da operação, incluindo o workspace encontrado ou erro de validação.</returns>
        public static async Task<IResult> GetWorkspace([FromServices] IMediator _mediator, Guid workspaceId)
        {
            var result = await _mediator.Send(new GetWorkspaceQuery
            {
                Id = workspaceId
            });

            if (result.ResponseInfo is null)
            {
                return Results.Ok(result.Value);
            }

            return Results.BadRequest(result.ResponseInfo);
        }

        /// <summary>
        /// Endpoint para obter todos os workspaces.
        /// </summary>
        /// <param name="_mediator">Instância do MediatR para envio do comando.</param>
        /// <param name="userId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns>Resultado da operação, incluindo a lista de workspaces ou erro de validação.</returns>
        public static async Task<IResult> GetAllWorkspace([FromServices] IMediator _mediator,
            [FromQuery] Guid userId, [FromQuery] int pageSize, [FromQuery] int pageIndex)
        {
            var result = await _mediator.Send(new GetAllWorkspaceQuery
            {
                UserId = userId,
                PageIndex = pageIndex,
                PageSize = pageSize
            });

            if (result.ResponseInfo is null)
            {
                return Results.Ok(result.Value);
            }

            return Results.BadRequest(result.ResponseInfo);
        }
    }
}
