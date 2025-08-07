using System.Linq.Expressions;

namespace Infra.Repository.UnitOfWork
{
    /// <summary>
    /// Interface genérica para operações básicas de repositório.
    /// Define métodos para manipulação de entidades no banco de dados, como consulta, criação, atualização e exclusão.
    /// Utilizada no padrão Unit of Work para abstrair o acesso aos dados.
    /// </summary>
    /// <typeparam name="T">Tipo da entidade manipulada pelo repositório.</typeparam>
    public interface IBaseRepository<T> where T : class
    {
        /// <summary>
        /// Obtém uma entidade que satisfaça a expressão informada.
        /// </summary>
        /// <param name="expression">Expressão lambda para filtro (ex: x => x.Id == id).</param>
        /// <returns>Entidade encontrada ou null.</returns>
        Task<T?> Get(Expression<Func<T,bool>> expression);

        /// <summary>
        /// Obtém todas as entidades do tipo T.
        /// </summary>
        /// <returns>Enumerable de entidades ou null.</returns>
        IEnumerable<T>? GetAll();

        /// <summary>
        /// Cria uma nova entidade no banco de dados.
        /// </summary>
        /// <param name="commandCreate">Entidade a ser criada.</param>
        /// <returns>Entidade criada.</returns>
        Task<T> Create(T commandCreate);

        /// <summary>
        /// Atualiza uma entidade existente no banco de dados.
        /// </summary>
        /// <param name="commandUpdate">Entidade com dados atualizados.</param>
        /// <returns>Entidade atualizada.</returns>
        Task<T> Update(T commandUpdate);

        /// <summary>
        /// Exclui uma entidade pelo identificador.
        /// </summary>
        /// <param name="id">Identificador único da entidade.</param>
        Task Delete(Guid id);
    }
}
