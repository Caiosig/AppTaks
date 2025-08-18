using Application.Response;
using Application.Utils;
using Application.WorkspaceCQ.ViewModels;
using MediatR;

namespace Application.WorkspaceCQ.Queries
{
    public record GetAllWorkspaceQuery : QueryBase, IRequest<ResponseBase<PaginatedList<WorkspaceViewModel>>>
    {
        public Guid UserId { get; set; }
    }
}
