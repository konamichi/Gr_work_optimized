using Microsoft.EntityFrameworkCore;
namespace NewsReader.Data
{
    public class DataContext : DbContext
    {
        private readonly IConfiguration _config;

        public DataContext(IConfiguration config)
        {
            _config = config;
        }

        public DbSet<Article> Articles { get; set; }
        public DbSet<Category> Categories { get; set;}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connection = _config["DbConnection"];

            optionsBuilder.UseNpgsql(connection);
        }
    }
}