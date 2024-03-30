using EclipseWorks.TaskManagementSystem.API.Requests;
using EclipseWorks.TaskManagementSystem.Application.Interfaces;
using EclipseWorks.TaskManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EclipseWorks.TaskManagementSystem.API.Controllers;
[Route("api")]
[ApiController]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _projectService;
    private readonly IUserService _userService;

    public ProjectController(IProjectService projectService, IUserService userService)
    {
        _projectService = projectService;
        _userService = userService;
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
        if (string.IsNullOrWhiteSpace(project.Name))
            return BadRequest("Project name is required.");

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
    public async Task<ActionResult<ProjectTask>> CreateTask(int projectId, ProjectTask task)
    {
        if (projectId != task.ProjectId)
            return BadRequest("Mismatch between route project ID and task project ID.");

        if (string.IsNullOrWhiteSpace(task.Title))
            return BadRequest("Task title is required.");

        var newTask = await _projectService.CreateTaskAsync(task);
        return Ok(newTask);
    }

    [HttpPut("projects/{projectId}/tasks/{taskId}")]
    public async Task<ActionResult<ProjectTask>> UpdateTask(int userId, int projectId, int taskId, [FromBody] ProjectTaskUpdateRequestDto taskUpdateDto)
    {
        var originalTask = await _projectService.GetTaskByIdAsync(taskId);
        if (originalTask == null)
            return NotFound("Task not found.");

        if (originalTask.ProjectId != projectId)
            return BadRequest("Mismatch between route project ID and task project ID.");

        originalTask.Title = taskUpdateDto.Title ?? originalTask.Title;
        originalTask.Description = taskUpdateDto.Description ?? originalTask.Description;

        if (taskUpdateDto.CompletionDate.HasValue)
            originalTask.CompletionDate = taskUpdateDto.CompletionDate;

        if (taskUpdateDto.DueDate.HasValue)
            originalTask.DueDate = taskUpdateDto.DueDate.Value;

        originalTask.Status = taskUpdateDto.Status;

        var updatedTask = await _projectService.UpdateTaskAsync(userId, originalTask);
        return Ok(updatedTask);
    }

    [HttpDelete("projects/{projectId}/tasks/{taskId}")]
    public async Task<ActionResult> DeleteTask(int taskId)
    {
        await _projectService.DeleteTaskAsync(taskId);
        return Ok();
    }

    [HttpPut("projects/{projectId}")]
    public async Task<ActionResult<Project>> UpdateProject(int projectId, [FromBody] ProjectUpdateRequestDto projectUpdateDto)
    {
        var originalProject = await _projectService.GetProjectByIdAsync(projectId);
        if (originalProject == null)
            return NotFound("Project not found.");

        originalProject.Name = projectUpdateDto.Name ?? originalProject.Name;

        var updatedProject = await _projectService.UpdateProjectAsync(originalProject);
        return Ok(updatedProject);
    }

    [HttpPost("tasks/{taskId}/comments")]
    public async Task<ActionResult<ProjectTaskComment>> AddTaskComment(int taskId, ProjectTaskCommentAddRequestDto commentDto)
    {
        if (commentDto == null || string.IsNullOrWhiteSpace(commentDto.Comment))
            return BadRequest("Comment is required.");

        var task = await _projectService.GetTaskByIdAsync(taskId);
        if (task == null)
            return NotFound("Task not found.");

        var user = await _userService.GetUserByIdAsync(commentDto.UserId);
        if (user == null)
            return NotFound("User not found.");

        var comment = new ProjectTaskComment
        {
            ProjectTaskId = task.Id,
            UserAccountId = user.Id,
            Comment = commentDto.Comment,
            SentAt = DateTime.UtcNow
        };

        var newComment = await _projectService.AddTaskCommentAsync(comment);
        return Ok(newComment);
    }

    [HttpGet("dashboard/{managerUserId}/performance/{userId}")]
    public async Task<ActionResult<double>> GetCompletedTasksPerUserLast30Days(int managerUserId, int userId)
    {
        var completedTasks = await _projectService.GetCompletedTasksPerUserLast30Days(managerUserId, userId);
        return Ok(completedTasks);
    }


}
