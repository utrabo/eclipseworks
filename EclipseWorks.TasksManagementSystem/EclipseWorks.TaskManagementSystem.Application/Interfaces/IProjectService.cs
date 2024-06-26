﻿using EclipseWorks.TaskManagementSystem.Domain.Entities;

namespace EclipseWorks.TaskManagementSystem.Application.Interfaces;
public interface IProjectService
{
    Task<ProjectTaskComment> AddTaskCommentAsync(ProjectTaskComment projectTaskComment);
    Task<Project> CreateProjectAsync(Project project);
    Task<ProjectTask> CreateTaskAsync(ProjectTask projectTask);
    Task DeleteProjectAsync(int projectId);
    Task DeleteTaskAsync(int taskId);
    Task<int> GetCompletedTasksPerUserLast30Days(int authenticatedUserId, int userId);
    Task<List<Project>> GetProjectsByUserIdAsync(int userId);
    Task<List<ProjectTask>> GetTasksByProjectIdAsync(int projectId);
    Task<Project> UpdateProjectAsync(Project project);
    Task<ProjectTask> UpdateTaskAsync(int userId, ProjectTask task);
    Task<ProjectTask?> GetTaskByIdAsync(int taskId);
    Task<Project?> GetProjectByIdAsync(int projectId);
}
