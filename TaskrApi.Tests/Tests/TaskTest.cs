using System.Net.Http.Json;

[Collection("TasksTests")]
public class TasksApiTests(CustomWebApplicationFactory factory)
{
  [Fact]
  public async Task Can_Create_Task()
  {
    // Act
    var response = await factory.HttpClient.PostAsJsonAsync("/api/tasks", new { Name = "Cadeautje kopen" }, TestContext.Current.CancellationToken);
    var tasks = await factory.HttpClient.GetFromJsonAsync<List<TaskrApi.Models.Task>>("/api/tasks", TestContext.Current.CancellationToken);

    // Assert
    response.EnsureSuccessStatusCode();
    Assert.NotNull(tasks);
    Assert.Contains(tasks, t => t.Name == "Cadeautje kopen");
  }
}