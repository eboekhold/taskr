using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using TaskrApi.Data;

[Collection("TasksTests")]
public class TasksApiTests(CustomWebApplicationFactory factory) : IAsyncLifetime
{
  public ValueTask InitializeAsync() => ValueTask.CompletedTask;
  public async ValueTask DisposeAsync() => await factory.ResetDatabaseAsync();

  [Fact]
  public async Task GetTasks_WhenNoneExist_ReturnsEmptyList()
  {
    var response = await factory.HttpClient.GetAsync("/api/tasks", TestContext.Current.CancellationToken);
    response.EnsureSuccessStatusCode();

    var tasks = await response.Content.ReadFromJsonAsync<List<TaskrApi.Models.Task>>(TestContext.Current.CancellationToken);
    Assert.NotNull(tasks);
    Assert.Empty(tasks);
  }

  [Fact]
  public async Task GetTasks_WhenOneExists_ReturnsOneTask()
  {
    // Arrange
    using var scope = factory.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    db.Tasks.Add(new TaskrApi.Models.Task { Name = "Cadeautje kopen" });
    await db.SaveChangesAsync(TestContext.Current.CancellationToken);

    // Act & Assert
    var response = await factory.HttpClient.GetAsync("/api/tasks", TestContext.Current.CancellationToken);
    response.EnsureSuccessStatusCode();

    var tasks = await response.Content.ReadFromJsonAsync<List<TaskrApi.Models.Task>>(TestContext.Current.CancellationToken);
    Assert.NotNull(tasks);
    Assert.Contains(tasks, t => t.Name == "Cadeautje kopen");
  }

  [Fact]
  public async Task CreateTask()
  {
    // Act
    var response = await factory.HttpClient.PostAsJsonAsync("/api/tasks", new { Name = "Cadeautje kopen" }, TestContext.Current.CancellationToken);
    var tasks = await factory.HttpClient.GetFromJsonAsync<List<TaskrApi.Models.Task>>("/api/tasks", TestContext.Current.CancellationToken);

    // Assert
    response.EnsureSuccessStatusCode();
    Assert.NotNull(tasks);
    Assert.Contains(tasks, t => t.Name == "Cadeautje kopen");
    Assert.True(tasks.Count == 1);
  }
}