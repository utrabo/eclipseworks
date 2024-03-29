using EclipseWorks.TaskManagementSystem.Application.Interfaces;
using EclipseWorks.TaskManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EclipseWorks.TaskManagementSystem.API.Controllers;
[Route("api")]
[ApiController]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpGet("users/{userId}/projects")]
    public async Task<ActionResult<List<Project>>> GetProjects(int userId)
    {
        var projects = await _projectService.GetProjectsByUserIdAsync(userId);
        return Ok(projects);
    }

    [HttpGet("projects/{projectId}/tasks")]
    public async Task<ActionResult<List<ProjectTask>>> GetTasks(int projectId)
    {
        var tasks = await _projectService.GetTasksByProjectIdAsync(projectId);
        return Ok(tasks);
    }

    [HttpPost("projects")]
    public async Task<ActionResult<Project>> CreateProject(Project project)
    {
        var newProject = await _projectService.CreateProjectAsync(project);
        return Ok(newProject);
    }

    [HttpDelete("projects/{projectId}")]
    public async Task<ActionResult> DeleteProject(int projectId)
    {
        await _projectService.DeleteProjectAsync(projectId);
        return Ok();
    }

    [HttpPost("projects/{projectId}/tasks")]
    public async Task<ActionResult<ProjectTask>> CreateTask(ProjectTask task)
    {
        var newTask = await _projectService.CreateTaskAsync(task);
        return Ok(newTask);
    }

    [HttpPut("projects/{projectId}/tasks/{taskId}")]
    public async Task<ActionResult<ProjectTask>> UpdateTask(ProjectTask task)
    {
        var updatedTask = await _projectService.UpdateTaskAsync(task);
        return Ok(updatedTask);
    }

    [HttpDelete("projects/{projectId}/tasks/{taskId}")]
    public async Task<ActionResult> DeleteTask(int taskId)
    {
        await _projectService.DeleteTaskAsync(taskId);
        return Ok();
    }

    [HttpPut("projects/{projectId}")]
    public async Task<ActionResult<Project>> UpdateProject(Project project)
    {
        var updatedProject = await _projectService.UpdateProjectAsync(project);
        return Ok(updatedProject);
    }

    [HttpPost("tasks/{taskId}/comments")]
    public async Task<ActionResult<ProjectTaskComment>> AddTaskComment(ProjectTaskComment comment)
    {
        var newComment = await _projectService.AddTaskCommentAsync(comment);
        return Ok(newComment);
    }
}
