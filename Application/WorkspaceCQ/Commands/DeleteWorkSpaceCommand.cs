using Application.Response;
using MediatR;

namespace Application.WorkspaceCQ.Commands
{
    public record DeleteWorkSpaceCommand : IRequest<ResponseBase<DeleteWorkSpaceCommand>>
    {
        public Guid Id { get; set; }
    }
}
