using Infra.Repository.IRepositories;

namespace Infra.Repository.UnitOfWork
{
    /// <summary>
    /// Interface do padrão Unit of Work.
    /// Centraliza o gerenciamento dos repositórios e garante a persistência transacional das operações no banco de dados.
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Repositório de usuários, responsável pelas operações de acesso e manipulação da entidade User.
        /// </summary>
        IUserRepository UserRepository { get; }

        IWorkSpaceRepository WorkSpaceRepository { get; }

        /// <summary>
        /// Persiste todas as alterações realizadas no contexto do banco de dados de forma atômica.
        /// </summary>
        void Commit();
    }
}
