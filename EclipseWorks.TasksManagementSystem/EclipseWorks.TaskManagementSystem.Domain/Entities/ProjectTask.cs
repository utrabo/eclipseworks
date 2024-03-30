﻿namespace EclipseWorks.TaskManagementSystem.Domain.Entities;
public class ProjectTask
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime DueDate { get; set; }
    public DateTime? CompletionDate { get; set; } = null;
    public int ProjectId { get; set; }
    public int AssignedToUserAccountId { get; set; }
    public ProjectTaskStatus Status { get; set; }
    public ProjectTaskPriority Priority { get; private set; }

    public ProjectTask(ProjectTaskPriority priority)
    {
        Priority = priority;
    }

    // Navigation properties
    public Project Project { get; set; } = null!;
    public UserAccount AssignedToUserAccount { get; set; } = null!;
    public List<ProjectTaskComment> ProjectTaskComment { get; set; } = new List<ProjectTaskComment>();
    public List<ProjectTaskHistory> ProjectTaskHistory { get; set; } = new List<ProjectTaskHistory>();

}
