using Infra.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infra.Repository.UnitOfWork
{
    /// <summary>
    /// Implementação genérica do padrão repositório para operações básicas de acesso a dados.
    /// Permite consultar, criar, atualizar e excluir entidades do banco de dados usando o Entity Framework.
    /// Utilizada em conjunto com o padrão Unit of Work para abstrair a persistência.
    /// </summary>
    /// <typeparam name="T">Tipo da entidade manipulada pelo repositório.</typeparam>
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly TasksDbContext _context;

        /// <summary>
        /// Construtor que injeta o contexto do banco de dados.
        /// </summary>
        /// <param name="context">Instância do TasksDbContext.</param>
        public BaseRepository(TasksDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém uma entidade que satisfaça a expressão informada.
        /// </summary>
        /// <param name="expression">Expressão lambda para filtro (ex: x => x.Id == id).</param>
        /// <returns>Entidade encontrada ou null.</returns>
        public async Task<T?> Get(Expression<Func<T, bool>> expression)
        {
           return await _context.Set<T>().FirstOrDefaultAsync(expression);
        }

        /// <summary>
        /// Obtém todas as entidades do tipo T.
        /// </summary>
        /// <returns>Enumerable de entidades ou null.</returns>
        public IEnumerable<T>? GetAll()
        {
            return [.. _context.Set<T>().ToList()];
        }

        /// <summary>
        /// Cria uma nova entidade no banco de dados.
        /// </summary>
        /// <param name="commandCreate">Entidade a ser criada.</param>
        /// <returns>Entidade criada.</returns>
        public async Task<T> Create(T commandCreate)
        {
            await _context.Set<T>().AddAsync(commandCreate);
            return commandCreate;
        }

        /// <summary>
        /// Atualiza uma entidade existente no banco de dados.
        /// </summary>
        /// <param name="commandUpdate">Entidade com dados atualizados.</param>
        /// <returns>Entidade atualizada.</returns>
        public async Task<T> Update(T commandUpdate)
        {
            _context.Set<T>().Update(commandUpdate);
            return commandUpdate;
        }

        /// <summary>
        /// Exclui uma entidade pelo identificador.
        /// </summary>
        /// <param name="id">Identificador único da entidade.</param>
        public Task Delete(Guid id)
        {
            _context.Remove(id);
            return Task.CompletedTask;
        }
    }
}
