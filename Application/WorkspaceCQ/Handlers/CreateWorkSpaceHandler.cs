using Application.Response;
using Application.WorkspaceCQ.Commands;
using Application.WorkspaceCQ.ViewModels;
using AutoMapper;
using Domain.Entity;
using Infra.Repository.UnitOfWork;
using MediatR;

namespace Application.WorkspaceCQ.Handlers
{
    /// <summary>
    /// Handler responsável por processar o comando de criação de um novo workspace.
    /// Utiliza o padrão Unit of Work para acessar os repositórios e o AutoMapper para mapear entidades para ViewModels.
    /// </summary>
    public class CreateWorkSpaceHandler : IRequestHandler<CreateWorkspaceCommand, ResponseBase<WorkspaceViewModel>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        /// <summary>
        /// Construtor que injeta as dependências necessárias para o handler.
        /// </summary>
        /// <param name="unitOfWork">Unit of Work para acesso aos repositórios.</param>
        /// <param name="mapper">Instância do AutoMapper para conversão de entidades.</param>
        public CreateWorkSpaceHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Manipula o comando de criação de workspace.
        /// Valida o usuário, cria o workspace e retorna o resultado mapeado para o ViewModel.
        /// </summary>
        /// <param name="request">Comando contendo os dados para criação do workspace.</param>
        /// <param name="cancellationToken">Token de cancelamento da operação.</param>
        /// <returns>Resposta contendo o ViewModel do workspace criado ou informações de erro.</returns>
        public async Task<ResponseBase<WorkspaceViewModel>> Handle(CreateWorkspaceCommand request, CancellationToken cancellationToken)
        {
            // Busca o usuário pelo Id informado no comando.
            var user = await _unitOfWork.UserRepository.Get(x => x.Id == request.UserId);

            // Se o usuário não for encontrado, retorna resposta de erro.
            if (user == null) 
            {
                return new ResponseBase<WorkspaceViewModel>
                {
                    ResponseInfo = new ResponseInfo
                    {
                        ErrorDescription = "Nenhum usuário encontrado com o Id informado",
                        HTTPStatus = 400,
                        Title = "Usuário não encontrado"
                    }
                };
            }

            // Cria uma nova instância de WorkSpace associada ao usuário.
            var workspace = new WorkSpace()
            {
                User = user,
                Title = request.Title,
            };

            // Persiste o novo workspace no banco de dados.
            await _unitOfWork.WorkSpaceRepository.Create(workspace);
            _unitOfWork.Commit();

            // Retorna o workspace criado, mapeado para o ViewModel.
            return new ResponseBase<WorkspaceViewModel>
            {
                ResponseInfo = null,
                Value = _mapper.Map<WorkspaceViewModel>(workspace)
            };
        }
    }
}
