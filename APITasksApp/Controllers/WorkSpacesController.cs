using Application.UserCQ.Commands;
using Application.WorkspaceCQ.Commands;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace APITasksApp.Controllers
{
    public static class WorkSpacesController
    {

        public static void WorkSpacesRoutes(this WebApplication app)
        {
            var group = app.MapGroup("Workspaces").WithTags("WorkSpaces").WithName("WorkSpaces");

            group.MapPost("create-workspace", () => CreateWorkspace).RequireAuthorization();
            group.MapPut("edit-workspace", () => EditWorkspace);
            group.MapDelete("delete-workspace", () => DeleteWorkspace);
            group.MapGet("get-workspace", () => GetWorkspace);
            group.MapGet("get-all-workspace", () => GetAllWorkspace);

        }

        public static async Task<IResult> CreateWorkspace([FromServices] IMediator _mediator, [FromBody] CreateWorkspaceCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.ResponseInfo is null)
            {
                return Results.Ok(result.Value);
            }

            return Results.BadRequest(result.ResponseInfo);
        }

        public static async Task<IResult> EditWorkspace([FromServices] IMediator _mediator)
        {

        }

        public static async Task<IResult> DeleteWorkspace([FromServices] IMediator _mediator)
        {

        }

        public static async Task<IResult> GetWorkspace([FromServices] IMediator _mediator)
        {

        }

        public static async Task<IResult> GetAllWorkspace([FromServices] IMediator _mediator)
        {

        }
    }
}
