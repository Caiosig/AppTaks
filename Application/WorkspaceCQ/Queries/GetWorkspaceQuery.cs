using Application.Response;
using Application.WorkspaceCQ.ViewModels;
using MediatR;

namespace Application.WorkspaceCQ.Queries
{
    public record GetWorkspaceQuery : IRequest<ResponseBase<WorkspaceViewModel>>
    {
        public Guid Id { get; init; }
    }
}
