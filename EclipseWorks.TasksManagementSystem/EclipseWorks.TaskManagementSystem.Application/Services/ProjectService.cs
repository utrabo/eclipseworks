using EclipseWorks.TaskManagementSystem.Application.Interfaces;
using EclipseWorks.TaskManagementSystem.Domain.Entities;
using EclipseWorks.TaskManagementSystem.Domain.Interfaces;

namespace EclipseWorks.TaskManagementSystem.Application.Services;
public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;

    public ProjectService(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public Task<Project> CreateProjectAsync(Project project)
    {
        throw new NotImplementedException();
    }

    public Task<ProjectTask> CreateTaskAsync(ProjectTask projectTask)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteProjectAsync(int projectId)
    {
        var tasks = await _projectRepository.GetTasksByProjectIdAsync(projectId);
        if (tasks.Any(task => task.Status == ProjectTaskStatus.Pending))
        {
            throw new InvalidOperationException("Project has pending tasks. Complete or remove the tasks first.");
        }

        await _projectRepository.DeleteProjectAsync(projectId);
    }

    public Task DeleteTaskAsync(int taskId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Project>> GetProjectsByUserIdAsync(int userId)
    {
        throw new NotImplementedException();
    }

    public Task<List<ProjectTask>> GetTasksByProjectIdAsync(int projectId)
    {
        throw new NotImplementedException();
    }

    public Task<Project> UpdateProjectAsync(Project project)
    {
        throw new NotImplementedException();
    }

    public Task<ProjectTask> UpdateTaskAsync(ProjectTask task)
    {
        throw new NotImplementedException();
    }

    public Task<ProjectTaskComment> AddTaskCommentAsync(ProjectTaskComment projectTaskComment)
    {
        throw new NotImplementedException();
    }
}
