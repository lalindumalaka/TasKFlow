namespace TaskFlow.Shared.Entities;

public class TimeEntry
{
    public int Id { get; set; }
    public int TaskItemId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public TimeSpan Duration { get; set; }
    
    // Navigation property
    public TaskItem TaskItem { get; set; } = null!;
}

