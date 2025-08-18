using Domain.Entity;
using Infra.Persistence;
using Infra.Repository.IRepositories;
using Infra.Repository.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repository.Repositories
{
    /// <summary>
    /// Repositório responsável por operações específicas relacionadas à entidade WorkSpace.
    /// Herda as operações básicas do BaseRepository e implementa métodos adicionais para consulta de workspaces com seus usuários.
    /// Utiliza o Entity Framework para acesso ao banco de dados.
    /// </summary>
    public class WorkSpaceRepository : BaseRepository<WorkSpace>, IWorkSpaceRepository
    {
        private readonly TasksDbContext _context;

        /// <summary>
        /// Construtor que injeta o contexto do banco de dados.
        /// </summary>
        /// <param name="context">Instância do TasksDbContext.</param>
        public WorkSpaceRepository(TasksDbContext context) : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém um workspace pelo Id, incluindo o usuário associado.
        /// </summary>
        /// <param name="workSpaceId">Identificador do workspace.</param>
        /// <returns>Workspace encontrado ou null.</returns>
        public async Task<WorkSpace?> GetWorkspaceAndUser(Guid workSpaceId)
        {
            return await _context.WorkSpaces.Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == workSpaceId);
        }

        /// <summary>
        /// Obtém todos os workspaces de um usuário específico, incluindo o usuário associado.
        /// </summary>
        /// <param name="userId">Identificador do usuário.</param>
        /// <returns>Lista de workspaces encontrados.</returns>
        public async Task<List<WorkSpace>> GetAllWorkspacesAndUser(Guid userId)
        {
            return await _context.WorkSpaces.Include(x => x.User)
                .Where(x => x.User!.Id == userId).ToListAsync();
        }
    }
}
