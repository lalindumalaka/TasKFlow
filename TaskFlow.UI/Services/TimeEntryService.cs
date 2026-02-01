using System.Net.Http.Json;
using TaskFlow.Shared.Entities;

namespace TaskFlow.UI.Services;

public class TimeEntryService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<TimeEntryService> _logger;

    public TimeEntryService(HttpClient httpClient, ILogger<TimeEntryService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<List<TimeEntry>> GetTimeEntriesAsync(int taskId)
    {
        try
        {
            var task = await _httpClient.GetFromJsonAsync<TaskItem>($"api/tasks/{taskId}");
            return task?.TimeEntries.ToList() ?? new List<TimeEntry>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching time entries for task {TaskId}", taskId);
            return new List<TimeEntry>();
        }
    }

    public async Task<bool> CreateTimeEntryAsync(TimeEntry timeEntry)
    {
        try
        {
            // Calculate duration if not set
            if (timeEntry.Duration == TimeSpan.Zero && timeEntry.EndTime > timeEntry.StartTime)
            {
                timeEntry.Duration = timeEntry.EndTime - timeEntry.StartTime;
            }

            var response = await _httpClient.PostAsJsonAsync("api/timeentries", timeEntry);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating time entry");
            return false;
        }
    }

    public async Task<bool> DeleteTimeEntryAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/timeentries/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting time entry {TimeEntryId}", id);
            return false;
        }
    }
}
