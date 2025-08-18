using Application.Response;
using Application.WorkspaceCQ.Commands;
using Infra.Repository.UnitOfWork;
using MediatR;

namespace Application.WorkspaceCQ.Handlers
{   
    /// <summary>
    /// Handler responsável por processar o comando de exclusão de um workspace.
    /// Remove o workspace e todas as listas/cartões associados, garantindo a integridade dos dados.
    /// Utiliza o padrão Unit of Work para acesso aos repositórios e persistência transacional.
    /// </summary>
    public class DeleteWorkspaceCommandHandler : IRequestHandler<DeleteWorkSpaceCommand, ResponseBase<DeleteWorkSpaceCommand>>
    {
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Construtor que injeta a dependência do Unit of Work.
        /// </summary>
        /// <param name="unitOfWork">Instância do Unit of Work para acesso aos repositórios.</param>
        public DeleteWorkspaceCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Manipula o comando de exclusão de workspace.
        /// Busca o workspace pelo Id, remove listas/cartões associados e exclui o workspace do banco de dados.
        /// </summary>
        /// <param name="request">Comando contendo o Id do workspace a ser excluído.</param>
        /// <param name="cancellationToken">Token de cancelamento da operação.</param>
        /// <returns>Resposta indicando sucesso ou erro na exclusão do workspace.</returns>
        public async Task<ResponseBase<DeleteWorkSpaceCommand>> Handle(DeleteWorkSpaceCommand request, CancellationToken cancellationToken)
        {
            // Busca o workspace pelo Id informado.
            var workspace = await _unitOfWork.WorkSpaceRepository.Get(x => x.Id == request.Id);

            // Se o workspace não for encontrado, retorna resposta de erro.
            if (workspace == null) 
            { 
                return new ResponseBase<DeleteWorkSpaceCommand>
                {
                    ResponseInfo = new ResponseInfo
                    {
                        Title = "Workspace não encontrado",
                        ErrorDescription = "Nenhum workspace encontrado com Id informado",
                        HTTPStatus = 404
                    }, 
                    Value = null
                };
            }

            // Busca todas as listas/cartões associadas ao workspace.
            var listsCards = _unitOfWork.ListsCardsRepository.GetAll().Where(x => x.Workspace == workspace).ToList();

            // Remove todas as listas/cartões associadas ao workspace.
            await _unitOfWork.ListsCardsRepository.DeleteRange(listsCards);

            // Remove o workspace do banco de dados.
            await _unitOfWork.WorkSpaceRepository.Delete(workspace);
            _unitOfWork.Commit();

            // Retorna resposta de sucesso.
            return new ResponseBase<DeleteWorkSpaceCommand>
            {
                ResponseInfo = null,
                Value = request
            };
        }
    }
}
