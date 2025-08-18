using Domain.Entity;
using Infra.Repository.UnitOfWork;

namespace Infra.Repository.IRepositories
{
    /// <summary>
    /// Interface de repositório para operações específicas da entidade WorkSpace.
    /// Herda as operações básicas de <see cref="IBaseRepository{WorkSpace}"/> e define métodos adicionais para consultas envolvendo workspaces e seus usuários.
    /// </summary>
    public interface IWorkSpaceRepository : IBaseRepository<WorkSpace>
    {
        /// <summary>
        /// Obtém um workspace pelo identificador, incluindo o usuário associado.
        /// </summary>
        /// <param name="workSpaceId">Identificador do workspace.</param>
        /// <returns>Workspace encontrado ou null.</returns>
        Task<WorkSpace?> GetWorkspaceAndUser(Guid workSpaceId);

        /// <summary>
        /// Obtém todos os workspaces de um usuário específico, incluindo o usuário associado.
        /// </summary>
        /// <param name="userId">Identificador do usuário.</param>
        /// <returns>Lista de workspaces encontrados.</returns>
        Task<List<WorkSpace>> GetAllWorkspacesAndUser(Guid userId);
    }
}
