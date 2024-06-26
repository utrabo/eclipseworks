﻿using EclipseWorks.TaskManagementSystem.Domain.Entities;
using EclipseWorks.TaskManagementSystem.Domain.Interfaces;
using EclipseWorks.TaskManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EclipseWorks.TaskManagementSystem.Infrastructure.Repositories;
public class ProjectRepository : IProjectRepository
{
    private readonly EclipseWorksDbContext _context;

    public ProjectRepository(EclipseWorksDbContext context)
    {
        _context = context;
    }

    public async Task<ProjectTaskComment> AddTaskCommentAsync(ProjectTaskComment projectTaskComment)
    {
        _context.ProjectTaskComment.Add(projectTaskComment);

        var history = new ProjectTaskHistory
        {
            ProjectTaskComment = projectTaskComment,
            ProjectTaskId = projectTaskComment.ProjectTaskId,
            ChangedAt = DateTime.UtcNow,
            ChangedByUserId = projectTaskComment.UserAccountId,
        };

        _context.ProjectTaskHistory.Add(history);

        await _context.SaveChangesAsync();

        return projectTaskComment;
    }

    public async Task<Project> CreateProjectAsync(Project project)
    {
        await _context.Project.AddAsync(project);
        await _context.SaveChangesAsync();
        return project;
    }

    public async Task<ProjectTask> CreateTaskAsync(ProjectTask projectTask)
    {
        await _context.ProjectTask.AddAsync(projectTask);
        await _context.SaveChangesAsync();
        return projectTask;
    }

    public async Task DeleteProjectAsync(int projectId)
    {
        var project = await _context.Project.FindAsync(projectId);
        if (project == null)
        {
            throw new InvalidOperationException("Project not found.");
        }

        _context.Project.Remove(project);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteTaskAsync(int taskId)
    {
        var task = await _context.ProjectTask.FindAsync(taskId);
        if (task == null)
        {
            throw new InvalidOperationException("Task not found.");
        }

        var comments = await _context.ProjectTaskComment.Where(comment => comment.ProjectTaskId == taskId).ToListAsync();
        if (comments.Any())
            _context.ProjectTaskComment.RemoveRange(comments);

        var history = await _context.ProjectTaskHistory.Where(history => history.ProjectTaskId == taskId).ToListAsync();
        if (history.Any())
            _context.ProjectTaskHistory.RemoveRange(history);

        _context.ProjectTask.Remove(task);

        await _context.SaveChangesAsync();
    }

    public async Task<int> GetCompletedTasksByUserIdSinceAsync(int userId, DateTime fromDate)
    {
        return await _context.ProjectTask
            .CountAsync(task => task.AssignedToUserAccountId == userId && task.Status == ProjectTaskStatus.Completed && task.CompletionDate >= fromDate);
    }

    public async Task<Project?> GetProjectByIdAsync(int projectId)
    {
        return await _context.Project.FindAsync(projectId);
    }

    public async Task<List<Project>> GetProjectsByUserIdAsync(int userId)
    {
        return await _context.Project.Where(project => project.UserAccountId == userId).ToListAsync();
    }

    public async Task<ProjectTask?> GetTaskByIdAsNoTrackingAsync(int taskId)
    {
        return await _context.ProjectTask.AsNoTracking().FirstOrDefaultAsync(task => task.Id == taskId);
    }

    public async Task<ProjectTask?> GetTaskByIdAsync(int taskId)
    { 
        return await _context.ProjectTask.FindAsync(taskId);
    }

    public async Task<List<ProjectTask>> GetTasksByProjectIdAsync(int projectId)
    {
        return await _context.ProjectTask.Where(task => task.ProjectId == projectId).ToListAsync();
    }

    public async Task<Project> UpdateProjectAsync(Project project)
    {
        var originalProject = await _context.Project.FindAsync(project.Id);
        if (originalProject == null)
        {
            throw new InvalidOperationException("Project not found.");
        }

        _context.Entry(originalProject).CurrentValues.SetValues(project);

        await _context.SaveChangesAsync();

        return originalProject;
    }

    public async Task<ProjectTask> UpdateTaskAsync(int userId, ProjectTask task)
    {
        var originalTask = await GetTaskByIdAsNoTrackingAsync(task.Id);
        if (originalTask == null)
        {
            throw new InvalidOperationException("Task not found.");
        }

        if (originalTask.Status != ProjectTaskStatus.Completed && task.Status == ProjectTaskStatus.Completed)
            task.CompletionDate = DateTime.UtcNow;

        _context.ProjectTask.Attach(task);
        _context.Entry(task).State = EntityState.Modified;
        
        PrepareTaskUpdateHistoryAsync(originalTask, task, userId);


        await _context.SaveChangesAsync();

        return task;
    }

    private void PrepareTaskUpdateHistoryAsync(ProjectTask originalTask, ProjectTask updatedTask, int changedByUserId)
    {
        var propertiesToExclude = new HashSet<string>
    {
        "Id",
        "ProjectId",
        "Priority",
        "Project",
        "ProjectTaskComment",
        "ProjectTaskHistory"
    };

        var properties = typeof(ProjectTask).GetProperties()
            .Where(prop => !propertiesToExclude.Contains(prop.Name) && prop.PropertyType.IsPrimitive ||
                           prop.PropertyType == typeof(string) ||
                           prop.PropertyType == typeof(DateTime));

        foreach (var property in properties)
        {
            var originalValue = property.GetValue(originalTask)?.ToString();
            var currentValue = property.GetValue(updatedTask)?.ToString();

            if (originalValue != currentValue)
            {
                _context.ProjectTaskHistory.Add(new ProjectTaskHistory
                {
                    ProjectTaskId = originalTask.Id,
                    PropertyName = property.Name,
                    OriginalValue = originalValue ?? string.Empty,
                    CurrentValue = currentValue ?? string.Empty,
                    ChangedAt = DateTime.UtcNow,
                    ChangedByUserId = changedByUserId
                });
            }
        }
    }

}
