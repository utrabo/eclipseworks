using EclipseWorks.TaskManagementSystem.Domain.Entities;
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

    public async Task<List<Project>> GetProjectsByUserIdAsync(int userId)
    {
        return await _context.Project.Where(project => project.UserAccountId == userId).ToListAsync();
    }

    public async Task<List<ProjectTask>> GetTasksByProjectIdAsync(int projectId)
    {
        return await _context.ProjectTask.Where(task => task.ProjectId == projectId).ToListAsync();
    }
}
