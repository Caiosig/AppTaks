using Application.Response;
using Application.Utils;
using Application.WorkspaceCQ.Queries;
using Application.WorkspaceCQ.ViewModels;
using AutoMapper;
using Infra.Repository.UnitOfWork;
using MediatR;

namespace Application.WorkspaceCQ.Handlers
{
    /// <summary>
    /// Handler responsável por processar a consulta paginada de todos os workspaces de um usuário.
    /// Utiliza o padrão Unit of Work para acessar os repositórios e o AutoMapper para mapear entidades para ViewModels.
    /// </summary>
    public class GetAllWorkspaceQueryHandler : IRequestHandler<GetAllWorkspaceQuery, ResponseBase<PaginatedList<WorkspaceViewModel>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        /// <summary>
        /// Construtor que injeta as dependências necessárias para o handler.
        /// </summary>
        /// <param name="unitOfWork">Unit of Work para acesso aos repositórios.</param>
        /// <param name="mapper">Instância do AutoMapper para conversão de entidades.</param>
        public GetAllWorkspaceQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Manipula a consulta para obter todos os workspaces de um usuário, de forma paginada.
        /// </summary>
        /// <param name="request">Query contendo o Id do usuário, página e tamanho da página.</param>
        /// <param name="cancellationToken">Token de cancelamento da operação.</param>
        /// <returns>Resposta paginada com os workspaces encontrados ou erro de validação.</returns>
        public async Task<ResponseBase<PaginatedList<WorkspaceViewModel>>> Handle(GetAllWorkspaceQuery request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.Get(x => x.Id == request.UserId);

            if (user is null)
            {
                return new ResponseBase<PaginatedList<WorkspaceViewModel>>
                {
                    ResponseInfo = new ResponseInfo
                    {
                        Title = "Usuário não encontrado.",
                        ErrorDescription = "nenhum usuário encontrado com o 'id' informado.",
                        HTTPStatus = 404
                    },
                    Value = null
                };
            }
            ;

            var workspaces = await _unitOfWork.WorkSpaceRepository.GetAllWorkspacesAndUser(user.Id);

            return new ResponseBase<PaginatedList<WorkspaceViewModel>>
            {
                Value = new PaginatedList<WorkspaceViewModel>(items: _mapper.Map<List<WorkspaceViewModel>>(workspaces),request.PageIndex, request.PageSize),
                ResponseInfo = null
            };
        }
    }
}
