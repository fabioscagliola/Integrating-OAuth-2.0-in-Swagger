using com.fabioscagliola.OAuthSwagger.WebApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Data.Common;

namespace com.fabioscagliola.OAuthSwagger.WebApiTest;

public class WebApiTestWebApplicationFactory<T> : WebApplicationFactory<T> where T : class
{
    protected override void ConfigureWebHost(IWebHostBuilder webHostBuilder)
    {
        webHostBuilder.ConfigureServices(configureServices =>
        {
            configureServices.Remove(configureServices.Single(d => d.ServiceType == typeof(DbContextOptions<WebApiDbContext>)));

            configureServices.AddSingleton((Func<IServiceProvider, DbConnection>)(implementationFactory =>
            {
                SqliteConnection sqliteConnection = new(Settings.Instance.SqliteConnectionString);
                sqliteConnection.Open();
                return sqliteConnection;
            }));

            configureServices.AddDbContext<WebApiDbContext>((serviceProvider, dbContextOptionBuilder) =>
            {
                DbConnection dbConnection = serviceProvider.GetRequiredService<DbConnection>();
                dbContextOptionBuilder.UseSqlite(dbConnection);
            });

            ServiceProvider serviceProvider = configureServices.BuildServiceProvider();
            IServiceScope serviceScope = serviceProvider.CreateScope();
            WebApiDbContext webApiDbContext = serviceScope.ServiceProvider.GetRequiredService<WebApiDbContext>();
            webApiDbContext.Database.EnsureCreated();
        });
    }
}
