using EclipseWorks.TaskManagementSystem.Domain.Entities;

namespace EclipseWorks.TaskManagementSystem.API.Requests;

public class ProjectTaskUpdateRequestDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? CompletionDate { get; set; } = null;
    public ProjectTaskStatus Status { get; set; }
}
