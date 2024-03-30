using EclipseWorks.TaskManagementSystem.API.Controllers;
using EclipseWorks.TaskManagementSystem.API.Requests;
using EclipseWorks.TaskManagementSystem.Application.Interfaces;
using EclipseWorks.TaskManagementSystem.Application.Services;
using EclipseWorks.TaskManagementSystem.Domain.Entities;
using System.Threading.Tasks;

namespace EclipseWorks.TaskManagementSystem.Tests.UnitTests
{
    public class ProjectControllerTests
    {
        [Fact]
        public async Task GetProjects_ForUser_ReturnsProjectList()
        {
            // Arrange
            var userId = 1;
            var mockProjectService = new Mock<IProjectService>();
            mockProjectService.Setup(service => service.GetProjectsByUserIdAsync(It.IsAny<int>()))
                              .ReturnsAsync(GetTestProjects());

            var mockUserService = new Mock<IUserService>();
            var controller = new ProjectController(mockProjectService.Object, mockUserService.Object);

            // Act
            var result = await controller.GetProjects(userId);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);

            var projectList = Assert.IsAssignableFrom<IEnumerable<Project>>(actionResult.Value);
            var projectListCount = Assert.IsAssignableFrom<List<Project>>(projectList);
            Assert.Equal(2, projectListCount.Count);
        }

        [Fact]
        public async Task GetTasks_ByProjectId_ReturnsTaskList()
        {
            // Arrange
            var projectId = 1;
            var mockProjectTaskService = new Mock<IProjectService>();
            mockProjectTaskService.Setup(service => service.GetTasksByProjectIdAsync(It.IsAny<int>()))
                                  .ReturnsAsync(GetTestTasks());

            var mockUserService = new Mock<IUserService>();
            var controller = new ProjectController(mockProjectTaskService.Object, mockUserService.Object);

            // Act
            var result = await controller.GetTasks(projectId);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);

            var taskList = Assert.IsAssignableFrom<IEnumerable<ProjectTask>>(actionResult.Value);
            Assert.NotEmpty(taskList);
            Assert.Equal(2, taskList.Count());
        }

        [Fact]
        public async Task CreateProject_ReturnsProject()
        {
            // Arrange
            var project = new Project { Id = 3, Name = "Project 3", UserAccountId = 1 };
            var mockProjectService = new Mock<IProjectService>();
            mockProjectService.Setup(service => service.CreateProjectAsync(It.IsAny<Project>()))
                              .ReturnsAsync(project);

            var mockUserService = new Mock<IUserService>();
            var controller = new ProjectController(mockProjectService.Object, mockUserService.Object);

            // Act
            var result = await controller.CreateProject(project);

            // Assert
            var actionResult = ((ObjectResult)result.Result!).Value;

            var createdProject = Assert.IsType<Project>(actionResult);
            Assert.Equal(project.Id, createdProject.Id);
            Assert.Equal(project.Name, createdProject.Name);
            Assert.Equal(project.UserAccountId, createdProject.UserAccountId);
        }

        [Fact]
        public async Task UpdateProject_ReturnsProject()
        {
            // Arrange
            var project = new Project { Id = 1, Name = "Project 1", UserAccountId = 1 };
            var projectUpdateDto = new ProjectUpdateRequestDto { Name = "Project 1" };
            var mockProjectService = new Mock<IProjectService>();
            mockProjectService.Setup(service => service.GetProjectByIdAsync(It.IsAny<int>()))
                              .ReturnsAsync(project);
            mockProjectService.Setup(service => service.UpdateProjectAsync(It.IsAny<Project>()))
                              .ReturnsAsync(project);

            var mockUserService = new Mock<IUserService>();
            var controller = new ProjectController(mockProjectService.Object, mockUserService.Object);

            // Act
            var result = await controller.UpdateProject(project.Id, projectUpdateDto);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);

            var updatedProject = Assert.IsType<Project>(actionResult.Value);
            Assert.Equal(project.Id, updatedProject.Id);
            Assert.Equal(project.Name, updatedProject.Name);
            Assert.Equal(project.UserAccountId, updatedProject.UserAccountId);
        }

        [Fact]
        public async Task DeleteProject_ReturnsOk()
        {
            // Arrange
            var projectId = 1;
            var mockProjectService = new Mock<IProjectService>();
            mockProjectService.Setup(service => service.DeleteProjectAsync(It.IsAny<int>()))
                              .Returns(Task.CompletedTask);

            var mockUserService = new Mock<IUserService>();
            var controller = new ProjectController(mockProjectService.Object, mockUserService.Object);

            // Act
            var result = await controller.DeleteProject(projectId);

            // Assert
            var actionResult = Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task CreateTask_ReturnsTask()
        {
            // Arrange
            var task = new ProjectTask(ProjectTaskPriority.Medium) { Title = "Task 1", Description = "Task 1", DueDate = DateTime.Now, Status = ProjectTaskStatus.Pending, ProjectId = 1 };
            var mockProjectService = new Mock<IProjectService>();
            mockProjectService.Setup(service => service.CreateTaskAsync(It.IsAny<ProjectTask>()))
                              .ReturnsAsync(task);

            var mockUserService = new Mock<IUserService>();
            var controller = new ProjectController(mockProjectService.Object, mockUserService.Object);

            // Act
            var result = await controller.CreateTask(1, task);

            // Assert
            var actionResult = ((ObjectResult)result.Result!).Value;

            var createdTask = Assert.IsType<ProjectTask>(actionResult);
            Assert.Equal(task.Id, createdTask.Id);
            Assert.Equal(task.ProjectId, createdTask.ProjectId);
            Assert.Equal(task.Title, createdTask.Title);
            Assert.Equal(task.Status, createdTask.Status);
        }

        [Fact]
        public async Task UpdateTask_ReturnsTask()
        {
            // Arrange
            var task = new ProjectTask(ProjectTaskPriority.Medium) { Id = 1, Title = "Task 1", Description = "Task 1", DueDate = DateTime.Now, Status = ProjectTaskStatus.Pending, ProjectId = 1 };
            var taskDto = new ProjectTaskUpdateRequestDto { Title = "Task 1", Description = "Task 1", DueDate = DateTime.Now, Status = ProjectTaskStatus.Pending };
            var mockProjectService = new Mock<IProjectService>();
            mockProjectService.Setup(service => service.GetTaskByIdAsync(It.IsAny<int>()))
                              .ReturnsAsync(task);
            mockProjectService.Setup(service => service.UpdateTaskAsync(It.IsAny<int>(), It.IsAny<ProjectTask>()))
                              .ReturnsAsync(task);

            var mockUserService = new Mock<IUserService>();
            var controller = new ProjectController(mockProjectService.Object, mockUserService.Object);

            // Act
            var result = await controller.UpdateTask(1, 1, 1, taskDto);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);

            var updatedTask = Assert.IsType<ProjectTask>(actionResult.Value);
            Assert.Equal(task.Id, updatedTask.Id);
            Assert.Equal(task.ProjectId, updatedTask.ProjectId);
            Assert.Equal(task.Title, updatedTask.Title);
            Assert.Equal(task.Status, updatedTask.Status);
        }

        [Fact]
        public async Task DeleteTask_ReturnsOk()
        {
            // Arrange
            var taskId = 1;
            var mockProjectService = new Mock<IProjectService>();
            mockProjectService.Setup(service => service.DeleteTaskAsync(It.IsAny<int>()))
                              .Returns(Task.CompletedTask);

            var mockUserService = new Mock<IUserService>();
            var controller = new ProjectController(mockProjectService.Object, mockUserService.Object);

            // Act
            var result = await controller.DeleteTask(taskId);

            // Assert
            var actionResult = Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task AddTaskComment_ReturnsTaskComment()
        {
            // Arrange
            var taskComment = new ProjectTaskComment { Id = 1, Comment = "Comment 1", ProjectTaskId = 1 };
            var taskCommentDto = new ProjectTaskCommentAddRequestDto { Comment = "Comment 1" };
            var mockProjectService = new Mock<IProjectService>();
            mockProjectService.Setup(service => service.GetTaskByIdAsync(It.IsAny<int>()))
                              .ReturnsAsync(new ProjectTask(ProjectTaskPriority.Medium) { Id = 1, Title = "Task 1", Description = "Task 1", DueDate = DateTime.Now, Status = ProjectTaskStatus.Pending, ProjectId = 1 });
            mockProjectService.Setup(service => service.AddTaskCommentAsync(It.IsAny<ProjectTaskComment>()))
                              .ReturnsAsync(taskComment);

            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(service => service.GetUserByIdAsync(It.IsAny<int>()))
                          .ReturnsAsync(new UserAccount { Id = 1 });
            var controller = new ProjectController(mockProjectService.Object, mockUserService.Object);

            // Act
            var result = await controller.AddTaskComment(1, taskCommentDto);

            // Assert
            var actionResult = ((ObjectResult)result.Result!).Value;

            var addedTaskComment = Assert.IsType<ProjectTaskComment>(actionResult);
            Assert.Equal(taskComment.Id, addedTaskComment.Id);
            Assert.Equal(taskComment.Comment, addedTaskComment.Comment);
            Assert.Equal(taskComment.ProjectTaskId, addedTaskComment.ProjectTaskId);
        }

        [Fact]
        public async Task GetCompletedTasksPerUserLast30Days_ReturnsInt()
        {
            // Arrange
            var managerUserId = 1;
            var userId = 2;
            var completedTasks = 10;
            var mockProjectService = new Mock<IProjectService>();
            mockProjectService.Setup(service => service.GetCompletedTasksPerUserLast30Days(It.IsAny<int>(), It.IsAny<int>()))
                              .ReturnsAsync(completedTasks);

            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(service => service.GetUserByIdAsync(managerUserId))
                          .ReturnsAsync(new UserAccount { Id = managerUserId, Role = UserAccountRole.Manager });

            var controller = new ProjectController(mockProjectService.Object, mockUserService.Object);

            // Act
            var result = await controller.GetCompletedTasksPerUserLast30Days(managerUserId, userId);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);

            var completedTasksCount = Assert.IsType<int>(actionResult.Value);
            Assert.Equal(completedTasks, completedTasksCount);
        }

        private List<Project> GetTestProjects()
        {
            return new List<Project>
            {
                new Project { Id = 1, Name = "Project 1", UserAccountId = 1 },
                new Project { Id = 2, Name = "Project 2", UserAccountId = 1 }
            };
        }

        private List<ProjectTask> GetTestTasks()
        {
            return new List<ProjectTask>
            {
                new ProjectTask(ProjectTaskPriority.Medium) { Title = "Task 1", Description = "Task 1", DueDate = DateTime.Now, Status = ProjectTaskStatus.Pending, ProjectId = 1 },
                new ProjectTask(ProjectTaskPriority.Medium) { Title = "Task 2", Description = "Task 2", DueDate = DateTime.Now, Status = ProjectTaskStatus.Pending, ProjectId = 1 }
            };
        }
    }
}