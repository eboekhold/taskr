using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaskrApi.Data;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
  private readonly TestDatabaseManager _dbManager = new();

  public HttpClient HttpClient { get; private set; } = null!;

  public async ValueTask InitializeAsync()
  {
    await _dbManager.StartContainerAsync();
    await _dbManager.SetupDatabaseAsync();
    await _dbManager.SetCheckpointAsync();

    HttpClient = CreateClient();
  }

  public new async Task DisposeAsync()
  {
    await _dbManager.DisposeAsync();
  }

  public async Task ResetDatabaseAsync()
  {
    await _dbManager.RestoreCheckpointAsync();
  }

  protected override void ConfigureWebHost(IWebHostBuilder builder)
  {
    builder.UseEnvironment("Testing");

    builder.ConfigureServices(services =>
    {
      // Remove existing DbContext
      services.Remove(services.Single(service => service.ServiceType == typeof(DbContextOptions<ApplicationDbContext>)));

      // Add Testcontainers MSSQL DbContext
      services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(_dbManager.ConnectionString));
    });
  }
}