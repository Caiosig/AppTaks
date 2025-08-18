using Application.Response;
using Application.WorkspaceCQ.Queries;
using Application.WorkspaceCQ.ViewModels;
using AutoMapper;
using Infra.Repository.UnitOfWork;
using MediatR;

namespace Application.WorkspaceCQ.Handlers
{
    /// <summary>
    /// Handler responsável por processar o comando de obtenção de um workspace específico.
    /// Utiliza o padrão Unit of Work para acessar o repositório de workspaces e o AutoMapper para mapear a entidade para o ViewModel.
    /// </summary>
    public record GetWorkspaceQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetWorkspaceQuery, ResponseBase<WorkspaceViewModel>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// Manipula o comando de busca de workspace pelo identificador.
        /// Retorna o workspace encontrado mapeado para o ViewModel ou uma resposta de erro caso não exista.
        /// </summary>
        /// <param name="request">Comando contendo o Id do workspace a ser buscado.</param>
        /// <param name="cancellationToken">Token de cancelamento da operação.</param>
        /// <returns>Resposta contendo o ViewModel do workspace ou informações de erro.</returns>
        public async Task<ResponseBase<WorkspaceViewModel>> Handle(GetWorkspaceQuery request, CancellationToken cancellationToken)
        {
            // Busca o workspace e o usuário associado pelo Id informado.
            var workspace = await _unitOfWork.WorkSpaceRepository.GetWorkspaceAndUser(request.Id);

            // Se não encontrar o workspace, retorna resposta de erro.
            if (workspace is null)
            {
                return new ResponseBase<WorkspaceViewModel>
                {
                    ResponseInfo = new ResponseInfo
                    {
                        Title = "Workspace não encontrado",
                        ErrorDescription="Nenhum workspace encontrado com o 'id' informado",
                        HTTPStatus = 404
                    },
                    Value = null
                };
            }

            // Retorna o workspace encontrado, mapeado para o ViewModel.
            return new ResponseBase<WorkspaceViewModel>
            {
                Value = _mapper.Map<WorkspaceViewModel>(workspace),
                ResponseInfo = null
            };
        }
    }
}
