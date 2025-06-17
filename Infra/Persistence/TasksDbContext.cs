using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infra.Persistence
{
    public class TasksDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<WorkSpace> WorkSpaces { get; set; }
        public DbSet<ListCard> ListsCards { get; set; }
        public DbSet<Cards> Cards { get; set; }
    }
}
