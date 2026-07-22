using Testcontainers.MsSql;

public class SqlServerContainerFixture : IAsyncLifetime
{
  public MsSqlContainer _msSqlContainer { get; }

  public SqlServerContainerFixture()
  {
    _msSqlContainer = new MsSqlBuilder("mcr.microsoft.com/mssql/server:2025-CU7-ubuntu-24.04")
      .WithPassword("My_strong_password123!")
      .Build();
  }

  public async ValueTask InitializeAsync()
  {
    await _msSqlContainer.StartAsync();
  }

  public async ValueTask DisposeAsync()
  {
    await _msSqlContainer.DisposeAsync();
  }
}