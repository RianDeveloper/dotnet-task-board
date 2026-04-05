using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SharpPet.Infrastructure.Persistence;

namespace SharpPet.Tests.Integration;

public sealed class ApiFixture : WebApplicationFactory<Program>, IAsyncLifetime
{
    private SqliteConnection? _connection;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureServices(services =>
        {
            var remove = services.Where(d =>
                d.ServiceType == typeof(DbContextOptions<AppDbContext>) ||
                d.ServiceType == typeof(DbContextOptions)).ToList();
            foreach (var d in remove)
                services.Remove(d);

            var ctx = services.SingleOrDefault(d => d.ServiceType == typeof(AppDbContext));
            if (ctx is not null)
                services.Remove(ctx);

            _connection = new SqliteConnection("DataSource=:memory:;Cache=Shared");
            _connection.Open();
            services.AddDbContext<AppDbContext>(o => o.UseSqlite(_connection));
        });
    }

    public async Task InitializeAsync()
    {
        CreateClient();
        using var scope = Services.CreateScope();
        await scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.MigrateAsync();
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await base.DisposeAsync();
        _connection?.Dispose();
    }
}
