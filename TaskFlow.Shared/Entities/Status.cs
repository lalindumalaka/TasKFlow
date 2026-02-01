namespace TaskFlow.Shared.Entities;

public class Status
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    
    // Navigation property
    public ICollection<TaskItem> TaskItems { get; set; } = new List<TaskItem>();
}

