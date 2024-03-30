﻿using EclipseWorks.TaskManagementSystem.Application.Interfaces;
using EclipseWorks.TaskManagementSystem.Domain;
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

    public async Task<Project> CreateProjectAsync(Project project)
    {
        return await _projectRepository.CreateProjectAsync(project);
    }

    public async Task<ProjectTask> CreateTaskAsync(ProjectTask projectTask)
    {
        var tasks = await _projectRepository.GetTasksByProjectIdAsync(projectTask.ProjectId);
        if (tasks.Count >= Constant.MaxTasksPerProject)
        {
            throw new InvalidOperationException("Project has reached the maximum number of tasks.");
        }

        return await _projectRepository.CreateTaskAsync(projectTask);
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

    public async Task DeleteTaskAsync(int taskId)
    {
        await _projectRepository.DeleteTaskAsync(taskId);
    }

    public async Task<List<Project>> GetProjectsByUserIdAsync(int userId)
    {
        return await _projectRepository.GetProjectsByUserIdAsync(userId);
    }

    public async Task<List<ProjectTask>> GetTasksByProjectIdAsync(int projectId)
    {
        return await _projectRepository.GetTasksByProjectIdAsync(projectId);
    }

    public Task<Project> UpdateProjectAsync(Project project)
    {
        throw new NotImplementedException();
    }

    public async Task<ProjectTask> UpdateTaskAsync(int userId, ProjectTask task)
    {
        return await _projectRepository.UpdateTaskAsync(userId, task);
    }

    public async Task<ProjectTaskComment> AddTaskCommentAsync(ProjectTaskComment projectTaskComment)
    {
        return await _projectRepository.AddTaskCommentAsync(projectTaskComment);
    }
}
