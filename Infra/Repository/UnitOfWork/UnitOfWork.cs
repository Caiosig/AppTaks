using Infra.Persistence;
using Infra.Repository.IRepositories;
using Infra.Repository.Repositories;

namespace Infra.Repository.UnitOfWork
{
    /// <summary>
    /// Implementação do padrão Unit of Work.
    /// Gerencia o contexto do banco de dados e os repositórios, permitindo o controle transacional das operações.
    /// O método Commit garante que todas as alterações realizadas nas entidades sejam persistidas de forma atômica.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TasksDbContext _context;

        /// <summary>
        /// Construtor que injeta o contexto do banco de dados e o repositório de usuários.
        /// </summary>
        /// <param name="context">Instância do TasksDbContext.</param>
        /// <param name="userRepository">Instância do repositório de usuários.</param>
        public UnitOfWork(TasksDbContext context, IUserRepository userRepository)
        {
            _context = context;
            UserRepository = userRepository ?? new UserRepository(context);
        }

        /// <summary>
        /// Repositório de usuários utilizado para operações relacionadas à entidade User.
        /// </summary>
        public IUserRepository UserRepository { get; }

        /// <summary>
        /// Persiste todas as alterações realizadas no contexto do banco de dados.
        /// Garante que as operações sejam executadas de forma transacional.
        /// </summary>
        public void Commit()
        {
            _context.SaveChanges();
        }
    }
}
