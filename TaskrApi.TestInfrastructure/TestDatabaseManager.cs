using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Respawn;
using TaskrApi.Data;
using Testcontainers.MsSql;

/// <summary>
/// Manages the TestContainers MSSQL Database. Has a single Respawn checkpoint.
/// </summary>
public class TestDatabaseManager
{
  private readonly MsSqlContainer _msSqlContainer;
  private Respawner? _checkpoint;
  public string ConnectionString => _msSqlContainer.GetConnectionString();

  public TestDatabaseManager()
  {
    _msSqlContainer = new MsSqlBuilder("mcr.microsoft.com/mssql/server:2025-CU7-ubuntu-24.04")
      .WithPassword("My_strong_password123!") // Not a real password, only used within the docker container during testing runs.
      .Build();
  }

  /// <summary>
  /// Spins up the TestContainer.
  /// </summary>
  public async Task StartContainerAsync()
  {
    await _msSqlContainer.StartAsync();
  }

  /// <summary>
  /// Loads the database schema into the container.
  /// </summary>
  /// <remarks>
  /// Requires the container to be started.
  /// </remarks>
  public async Task SetupDatabaseAsync()
  {
    var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseSqlServer(ConnectionString)
        .Options;

    await using var dbContext = new ApplicationDbContext(options);

    // Ensure database + tables exist
    await dbContext.Database.EnsureCreatedAsync();
  }

  /// <summary>
  /// Sets a checkpoint to later be reset to using <see cref="RestoreCheckpointAsync"/>.
  /// </summary>
  public async Task SetCheckpointAsync()
  {
    await using var connection = new SqlConnection(ConnectionString);
    await connection.OpenAsync();

    _checkpoint = await Respawner.CreateAsync(connection, new RespawnerOptions
    {
      SchemasToInclude = ["dbo"],
    });
  }

  /// <summary>
  /// Restores the database to the checkpoint set by <see cref="SetCheckpointAsync"/>.
  /// </summary>
  public async Task RestoreCheckpointAsync()
  {
    await using var connection = new SqlConnection(ConnectionString);
    await connection.OpenAsync();
    await _checkpoint!.ResetAsync(connection);
  }

  public async ValueTask DisposeAsync()
  {
    await _msSqlContainer.DisposeAsync();
  }
}