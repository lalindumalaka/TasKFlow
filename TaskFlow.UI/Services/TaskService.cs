using System.Net.Http.Json;
using TaskFlow.Shared.Entities;

namespace TaskFlow.UI.Services;

public class TaskService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<TaskService> _logger;

    public TaskService(HttpClient httpClient, ILogger<TaskService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<List<TaskItem>> GetTasksAsync()
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<List<TaskItem>>("api/tasks");
            return response ?? new List<TaskItem>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching tasks");
            return new List<TaskItem>();
        }
    }

    public async Task<TaskItem?> GetTaskAsync(int id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<TaskItem>($"api/tasks/{id}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching task {TaskId}", id);
            return null;
        }
    }

    public async Task<TaskItem?> CreateTaskAsync(TaskItem task)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/tasks", task);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TaskItem>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating task");
            return null;
        }
    }

    public async Task<bool> UpdateTaskAsync(TaskItem task)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"api/tasks/{task.Id}", task);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating task {TaskId}", task.Id);
            return false;
        }
    }

    public async Task<bool> DeleteTaskAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/tasks/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting task {TaskId}", id);
            return false;
        }
    }
}
