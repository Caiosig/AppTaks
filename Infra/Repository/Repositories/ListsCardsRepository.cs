using Domain.Entity;
using Infra.Persistence;
using Infra.Repository.IRepositories;
using Infra.Repository.UnitOfWork;

namespace Infra.Repository.Repositories
{
    public class ListsCardsRepository(TasksDbContext context) : BaseRepository<ListCard>(context), IListsCardsRepository
    {
    }
}
