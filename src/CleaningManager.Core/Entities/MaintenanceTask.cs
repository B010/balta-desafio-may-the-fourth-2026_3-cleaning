namespace CleaningManager.Core.Entities;

public class MaintenanceTask
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public int IntervalDays { get; init; }
    public DateOnly? LastDone { get; set; }

    public bool IsDue(DateOnly today) =>
        LastDone is null || today >= LastDone.Value.AddDays(IntervalDays);

    public DateOnly? NextDue() =>
        LastDone?.AddDays(IntervalDays);
}
