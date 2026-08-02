using System.Net.Http.Json;

[Collection("TasksTests")]
public class TasksApiTests(TestApiFactory factory)
{
  [Fact]
  public async Task Can_Create_Task()
  {
    var response = await factory.HttpClient.PostAsJsonAsync("/api/tasks", new { Name = "Cadeautje kopen" });
    response.EnsureSuccessStatusCode();

    var tasks = await factory.HttpClient.GetFromJsonAsync<List<TaskrApi.Models.Task>>("/api/tasks");

    Assert.Contains(tasks, t => t.Name == "Cadeautje kopen");
  }
}