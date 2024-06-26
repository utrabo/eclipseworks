﻿using EclipseWorks.TaskManagementSystem.Application.Interfaces;
using EclipseWorks.TaskManagementSystem.Domain;
using EclipseWorks.TaskManagementSystem.Domain.Entities;
using EclipseWorks.TaskManagementSystem.Domain.Interfaces;

namespace EclipseWorks.TaskManagementSystem.Application.Services;
public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly IUserRepository _userRepository;

    public ProjectService(IProjectRepository projectRepository, IUserRepository userRepository)
    {
        _projectRepository = projectRepository;
        _userRepository = userRepository;
    }

    public async Task<Project> CreateProjectAsync(Project project)
    {
        return await _projectRepository.CreateProjectAsync(project);
    }

    public async Task<ProjectTask> CreateTaskAsync(ProjectTask projectTask)
    {
        var project = await _projectRepository.GetProjectByIdAsync(projectTask.ProjectId);
        if (project == null)
            throw new InvalidOperationException("Project not found.");

        var tasks = await _projectRepository.GetTasksByProjectIdAsync(projectTask.ProjectId);
        if (tasks.Count >= Constant.MaxTasksPerProject)
            throw new InvalidOperationException("Project has reached the maximum number of tasks.");

        var user = await _userRepository.GetUserByIdAsync(projectTask.AssignedToUserAccountId);
        if (user == null)
            throw new InvalidOperationException("User not found.");

        return await _projectRepository.CreateTaskAsync(projectTask);
    }

    public async Task DeleteProjectAsync(int projectId)
    {
        var tasks = await _projectRepository.GetTasksByProjectIdAsync(projectId);
        if (tasks.Any(task => task.Status == ProjectTaskStatus.Pending))
        {
            throw new InvalidOperationException("Project has pending tasks. Complete or remove the tasks first.");
        }

        var projectTasks = await _projectRepository.GetTasksByProjectIdAsync(projectId);
        foreach (var task in projectTasks)
        {
            await _projectRepository.DeleteTaskAsync(task.Id);
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

    public async Task<Project> UpdateProjectAsync(Project project)
    {
        return await _projectRepository.UpdateProjectAsync(project);
    }

    public async Task<ProjectTask> UpdateTaskAsync(int userId, ProjectTask task)
    {
        return await _projectRepository.UpdateTaskAsync(userId, task);
    }

    public async Task<ProjectTaskComment> AddTaskCommentAsync(ProjectTaskComment projectTaskComment)
    {
        return await _projectRepository.AddTaskCommentAsync(projectTaskComment);
    }

    public async Task<int> GetCompletedTasksPerUserLast30Days(int authenticatedUserId, int userId)
    {
        var authenticatedUser = await _userRepository.GetUserByIdAsync(authenticatedUserId);
        if (authenticatedUser == null)
            throw new InvalidOperationException("User not found.");
        
        if (authenticatedUser.Role != UserAccountRole.Manager)
            throw new InvalidOperationException("Only managers can view the performance of other users.");

        var fromDate = DateTime.UtcNow.AddDays(-30);
        var completedTasks = await _projectRepository.GetCompletedTasksByUserIdSinceAsync(userId, fromDate);
        return completedTasks;
    }

    public async Task<ProjectTask?> GetTaskByIdAsync(int taskId)
    { 
        var task = await _projectRepository.GetTaskByIdAsync(taskId);
        return task;
    }

    public async Task<Project?> GetProjectByIdAsync(int projectId)
    {
        var project = await _projectRepository.GetProjectByIdAsync(projectId);
        return project;
    }
}
