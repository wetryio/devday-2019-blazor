using BoardApp.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace BoardApp.Functions.Infrastructure
{
    public class BoardContext : DbContext
    {
        public DbSet<BoardItem> BoardItem { get; set; }

        public BoardContext(DbContextOptions<BoardContext> options)
            : base(options)
        {

        }
    }
}
