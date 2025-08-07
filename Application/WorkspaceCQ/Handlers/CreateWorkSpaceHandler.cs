using Application.Response;
using Application.WorkspaceCQ.Commands;
using Application.WorkspaceCQ.ViewModels;
using AutoMapper;
using Domain.Entity;
using Infra.Repository.UnitOfWork;
using MediatR;

namespace Application.WorkspaceCQ.Handlers
{
    public class CreateWorkSpaceHandler : IRequestHandler<CreateWorkspaceCommand, ResponseBase<CreateWorkspaceViewModel>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateWorkSpaceHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseBase<CreateWorkspaceViewModel>> Handle(CreateWorkspaceCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.Get(x => x.Id == request.UserId);

            if (user == null) 
            {
                return new ResponseBase<CreateWorkspaceViewModel>
                {
                    ResponseInfo = new ResponseInfo
                    {
                        ErrorDescription = "Nenhum usuário encontrado com o Id informado",
                        HTTPStatus = 400,
                        Title = "Usuário não encontrado"
                    }
                };
            }

            var workspace = new WorkSpace()
            {
                User = user,
                Title = request.Title,
            };

            await _unitOfWork.WorkSpaceRepository.Create(workspace);
            _unitOfWork.Commit();

            return new ResponseBase<CreateWorkspaceViewModel>
            {
                ResponseInfo = null,
                Value = _mapper.Map<CreateWorkspaceViewModel>(workspace)
            };
        }
    }
}
