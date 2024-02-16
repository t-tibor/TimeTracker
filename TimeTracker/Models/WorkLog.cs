namespace TimeTracker.Models;

public partial class WorkLog
{
    public DateOnly WorkDay { get; set; }

    public double Duration { get; set; }

    public string Summary { get; set; }

    public string Description { get; set; }

    public int Id { get; set; }

    public int TaskId { get; set; }

    public virtual Task Task { get; set; }
}
