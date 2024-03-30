using EclipseWorks.TaskManagementSystem.Domain.Entities;

namespace EclipseWorks.TaskManagementSystem.Domain.Interfaces;
public interface IProjectRepository
{
    Task<ProjectTaskComment> AddTaskCommentAsync(ProjectTaskComment projectTaskComment);
    Task<Project> CreateProjectAsync(Project project);
    Task<ProjectTask> CreateTaskAsync(ProjectTask projectTask);
    Task DeleteProjectAsync(int projectId);
    Task DeleteTaskAsync(int taskId);
    Task<int> GetCompletedTasksByUserIdSinceAsync(int userId, DateTime fromDate);
    Task<Project?> GetProjectByIdAsync(int projectId);
    Task<List<Project>> GetProjectsByUserIdAsync(int userId);
    Task<ProjectTask?> GetTaskByIdAsNoTrackingAsync(int taskId);
    Task<ProjectTask?> GetTaskByIdAsync(int taskId);
    Task<List<ProjectTask>> GetTasksByProjectIdAsync(int projectId);
    Task<Project> UpdateProjectAsync(Project project);
    Task<ProjectTask> UpdateTaskAsync(int userId, ProjectTask task);
}
