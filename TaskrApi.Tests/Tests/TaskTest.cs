using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using FluentAssertions;
using TaskrApi.Data;

[Collection("TasksTests")]
public class TasksApiTests(CustomWebApplicationFactory factory) : IAsyncLifetime
{
  public ValueTask InitializeAsync() => ValueTask.CompletedTask;
  public async ValueTask DisposeAsync() => await factory.ResetDatabaseAsync();

  [Fact]
  public async Task GetTasks_WhenNoneExist_ReturnsEmptyList()
  {
    // Act
    var response = await factory.HttpClient.GetAsync("/api/tasks", TestContext.Current.CancellationToken);

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.OK);

    var tasks = await response.Content.ReadFromJsonAsync<List<TaskrApi.Models.Task>>(TestContext.Current.CancellationToken);
    tasks.Should().NotBeNull();
    tasks.Should().BeEmpty();
  }

  [Fact]
  public async Task GetTasks_WhenOneExists_ReturnsOneTask()
  {
    // Arrange
    using var scope = factory.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    db.Tasks.Add(new TaskrApi.Models.Task { Name = "Cadeautje kopen" });
    await db.SaveChangesAsync(TestContext.Current.CancellationToken);

    // Act
    var response = await factory.HttpClient.GetAsync("/api/tasks", TestContext.Current.CancellationToken);

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.OK);

    var tasks = await response.Content.ReadFromJsonAsync<List<TaskrApi.Models.Task>>(TestContext.Current.CancellationToken);
    tasks.Should().NotBeNull();
    tasks.Should().ContainSingle();
    tasks.Should().OnlyContain(t => t.Name == "Cadeautje kopen");
  }

  [Fact]
  public async Task PutTask_WhenOneExists_ReturnsUpdatedTask()
  {
    // Arrange
    using var scope = factory.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    var existingTask = db.Tasks.Add(new TaskrApi.Models.Task { Name = "Cadeautje kopen" });
    await db.SaveChangesAsync(TestContext.Current.CancellationToken);

    // Act
    var taskId = existingTask.Entity.Id;
    var response = await factory.HttpClient.PutAsJsonAsync($"/api/tasks/{taskId}", new { Id = taskId, Name = "Cadeautjes kopen" }, TestContext.Current.CancellationToken);

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.OK);

    existingTask.Reload();
    existingTask.Entity.Name.Should().Be("Cadeautjes kopen");

    var responseTask = await response.Content.ReadFromJsonAsync<TaskrApi.Models.Task>(TestContext.Current.CancellationToken);
    responseTask.Should().NotBeNull();
    responseTask.Name.Should().Be("Cadeautjes kopen");
  }

  [Fact]
  public async Task PutTask_WhenNoneExists_ReturnsError()
  {
    // Act
    var response = await factory.HttpClient.PutAsJsonAsync("/api/tasks/1", new { Id = 1, Name = "Cadeautje kopen" }, TestContext.Current.CancellationToken);

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.NotFound);
  }

  [Fact]
  public async Task CreateTask()
  {
    // Act
    var response = await factory.HttpClient.PostAsJsonAsync("/api/tasks", new { Name = "Cadeautje kopen" }, TestContext.Current.CancellationToken);

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.Created);

    var task = await response.Content.ReadFromJsonAsync<TaskrApi.Models.Task>(TestContext.Current.CancellationToken);
    task.Should().NotBeNull();
    task.Name.Should().Be("Cadeautje kopen");
  }
}