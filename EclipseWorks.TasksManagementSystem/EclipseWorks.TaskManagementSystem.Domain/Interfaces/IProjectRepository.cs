﻿using EclipseWorks.TaskManagementSystem.Domain.Entities;

namespace EclipseWorks.TaskManagementSystem.Domain.Interfaces;
public interface IProjectRepository
{
    Task DeleteProjectAsync(int projectId);
    Task<List<Project>> GetProjectsByUserIdAsync(int userId);
    Task<List<ProjectTask>> GetTasksByProjectIdAsync(int projectId);
}
