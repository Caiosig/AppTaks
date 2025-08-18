using Application.Response;
using Application.WorkspaceCQ.Commands;
using Application.WorkspaceCQ.ViewModels;
using AutoMapper;
using Infra.Repository.UnitOfWork;
using MediatR;

namespace Application.WorkspaceCQ.Handlers
{
    /// <summary>
    /// Handler responsável por processar o comando de edição de um workspace.
    /// Permite atualizar o título e o status de um workspace existente, garantindo a persistência das alterações.
    /// Utiliza o padrão Unit of Work para acesso ao repositório e o AutoMapper para conversão de entidades.
    /// </summary>
    public class EditWorkspaceCommandHandler : IRequestHandler<EditWorkspaceCommand, ResponseBase<WorkspaceViewModel>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        /// <summary>
        /// Construtor que injeta as dependências necessárias para o handler.
        /// </summary>
        /// <param name="unitOfWork">Unit of Work para acesso aos repositórios.</param>
        /// <param name="mapper">Instância do AutoMapper para conversão de entidades.</param>
        public EditWorkspaceCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Manipula o comando de edição de workspace.
        /// Busca o workspace pelo Id, atualiza os campos informados e persiste as alterações.
        /// </summary>
        /// <param name="request">Comando contendo os dados para edição do workspace.</param>
        /// <param name="cancellationToken">Token de cancelamento da operação.</param>
        /// <returns>Resposta contendo o ViewModel do workspace atualizado ou informações de erro.</returns>
        public async Task<ResponseBase<WorkspaceViewModel>> Handle(EditWorkspaceCommand request, CancellationToken cancellationToken)
        {
            // Busca o workspace e o usuário associado pelo Id informado.
            var workspace = await _unitOfWork.WorkSpaceRepository.GetWorkspaceAndUser(request.Id);

            // Se não encontrar o workspace, retorna resposta de erro.
            if (workspace == null)
            {
                return new ResponseBase<WorkspaceViewModel>
                {
                    ResponseInfo = new()
                    {
                        Title = "Workspace não encontrado",
                        ErrorDescription = "Nenhum workspace encontrado com o 'Id' informado",
                        HTTPStatus = 404
                    },
                    Value = null
                };
            }

            // Atualiza o título do workspace se informado.
            if (request.Title != null)
                workspace.Title = request.Title;

            // Atualiza o status do workspace se informado.
            if (request.Status != null)
                workspace.Status = request.Status;

            // Persiste as alterações no banco de dados.
            _unitOfWork.Commit();

            // Retorna o workspace atualizado, mapeado para o ViewModel.
            return new ResponseBase<WorkspaceViewModel>
            {
                ResponseInfo = null,
                Value = _mapper.Map<WorkspaceViewModel>(workspace)
            };
        }
    }
}
