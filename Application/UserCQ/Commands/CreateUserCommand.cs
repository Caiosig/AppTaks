using Application.Response;
using Application.UserCQ.ViewModels;
using MediatR;

namespace Application.UserCQ.Commands
{
    public record CreateUserCommand : IRequest<ResponseBase<UserInfoViewModel>>
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Username { get; set; }
    }
}
