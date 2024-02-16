namespace TimeTracker.Models;

public partial class Task
{
    public string TaskName { get; set; }

    public string Description { get; set; }

    public int Id { get; set; }

    public int ProjectId { get; set; }

    public virtual Project Project { get; set; }

    public virtual ICollection<WorkLog> WorkLogs { get; set; } = new List<WorkLog>();
}
