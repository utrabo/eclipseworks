using EclipseWorks.TaskManagementSystem.Application;
using EclipseWorks.TaskManagementSystem.Application.Interfaces;
using EclipseWorks.TaskManagementSystem.Application.Services;
using EclipseWorks.TaskManagementSystem.Domain.Entities;
using EclipseWorks.TaskManagementSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EclipseWorks.TaskManagementSystem.Tests.UnitTests;
public class ProjectServiceTests
{
    [Fact]
    public async Task DeleteProjectAsync_WithPendingTasks_ThrowsException()
    {
        // Arrange
        var projectId = 1;
        var mockProjectRepository = new Mock<IProjectRepository>();
        mockProjectRepository.Setup(repo => repo.GetTasksByProjectIdAsync(projectId))
                             .ReturnsAsync(new List<ProjectTask>
                             {
                            new ProjectTask(ProjectTaskPriority.Low) { Title = "Task 1", Description = "Task 1", DueDate = DateTime.Now, Status = ProjectTaskStatus.Pending, ProjectId = projectId }
                             });

        var service = new ProjectService(mockProjectRepository.Object);

        // Act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => service.DeleteProjectAsync(projectId));

        // Assert
        Assert.Equal("Project has pending tasks. Complete or remove the tasks first.", exception.Message);
    }

    [Fact]
    public async Task CreateTaskAsync_WithMoreThan20Tasks_ThrowsException()
    {
        // Arrange
        var projectId = 1;
        var mockProjectRepository = new Mock<IProjectRepository>();
        mockProjectRepository.Setup(repo => repo.GetTasksByProjectIdAsync(projectId))
                             .ReturnsAsync(Enumerable.Range(1, 20).Select(i => new ProjectTask(ProjectTaskPriority.Low) { Title = $"Task {i}", Description = $"Task {i}", DueDate = DateTime.Now, Status = ProjectTaskStatus.Pending, ProjectId = projectId }).ToList());

        var service = new ProjectService(mockProjectRepository.Object);

        // Act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateTaskAsync(new ProjectTask(ProjectTaskPriority.Low) { Title = "Task 21", Description = "Task 21", DueDate = DateTime.Now, Status = ProjectTaskStatus.Pending, ProjectId = projectId }));

        // Assert
        Assert.Equal("Project has reached the maximum number of tasks.", exception.Message);
    }
}
