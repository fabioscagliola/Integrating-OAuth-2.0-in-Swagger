using Microsoft.EntityFrameworkCore;

namespace com.fabioscagliola.OAuthSwagger.WebApi;

public class WebApiDbContext : DbContext
{
    public WebApiDbContext(DbContextOptions options) : base(options) { }

    public DbSet<Person> People { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>().ToTable(nameof(Person));
    }
}
