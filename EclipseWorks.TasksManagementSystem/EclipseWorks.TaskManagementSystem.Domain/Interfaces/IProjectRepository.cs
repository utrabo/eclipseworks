using EclipseWorks.TaskManagementSystem.Domain.Entities;

namespace EclipseWorks.TaskManagementSystem.Domain.Interfaces;
public interface IProjectRepository
{
    Task<Project> CreateProjectAsync(Project project);
    Task<ProjectTask> CreateTaskAsync(ProjectTask projectTask);
    Task DeleteProjectAsync(int projectId);
    Task<List<Project>> GetProjectsByUserIdAsync(int userId);
    Task<List<ProjectTask>> GetTasksByProjectIdAsync(int projectId);
    Task<ProjectTask> UpdateTaskAsync(ProjectTask task);
}
