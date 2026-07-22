using System.Net.Http.Json;

public class TasksApiTests : IClassFixture<SqlServerContainerFixture>
{
  private readonly HttpClient _client;

  public TasksApiTests(SqlServerContainerFixture fixture)
  {
    var factory = new TestApiFactory(fixture._msSqlContainer.GetConnectionString());
    _client = factory.CreateClient();
  }

  [Fact]
  public async Task Can_Create_Task()
  {
    var response = await _client.PostAsJsonAsync("/api/tasks", new { Name = "Cadeautje kopen" });
    response.EnsureSuccessStatusCode();

    var tasks = await _client.GetFromJsonAsync<List<TaskrApi.Models.Task>>("/api/tasks");
    Assert.Contains(tasks, t => t.Name == "Cadeautje kopen");
  }
}