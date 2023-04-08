using com.fabioscagliola.OAuthSwagger.WebApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace com.fabioscagliola.OAuthSwagger.WebApiTest;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<WebApiDbContext>
{
    public WebApiDbContext CreateDbContext(string[] args)
    {
        DbContextOptionsBuilder dbContextOptionsBuilder = new DbContextOptionsBuilder<WebApiDbContext>();
        dbContextOptionsBuilder.UseSqlite(Settings.Instance.SqliteConnectionString, sqliteOptionsAction => sqliteOptionsAction.MigrationsAssembly("WebApiTest"));
        return new(dbContextOptionsBuilder.Options);
    }
}
