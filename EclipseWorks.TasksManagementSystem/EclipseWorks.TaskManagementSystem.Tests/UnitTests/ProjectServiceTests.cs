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
    public async void DeleteProjectAsync_WithPendingTasks_ThrowsException()
    {
        // Arrange
        var projectId = 1;
        var mockProjectRepository = new Mock<IProjectRepository>();
        mockProjectRepository.Setup(repo => repo.GetTasksByProjectIdAsync(projectId))
                             .ReturnsAsync(new List<ProjectTask>
                             {
                                new ProjectTask("Task 1", "Task 1", DateTime.Now, ProjectTaskStatus.Pending, ProjectTaskPriority.Low)
                             });

        IProjectService service = new ProjectService(mockProjectRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.DeleteProjectAsync(projectId));
    }
}
