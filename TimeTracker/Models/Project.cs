namespace TimeTracker.Models;

public partial class Project
{
    public string ProjectName { get; set; }

    public int Id { get; set; }

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
