using EclipseWorks.TaskManagementSystem.API.Controllers;
using EclipseWorks.TaskManagementSystem.Application.Services;
using EclipseWorks.TaskManagementSystem.Domain.Entities;

namespace EclipseWorks.TaskManagementSystem.Tests.IntegrationTests;
public class ProjectControllerIntegrationTests
{
    [Collection("Sequential")]
    public class ProjectController_GetProjects : IntegrationTestBase
    {
        protected override void SeedDatabase()
        {
            var user = new UserAccount { Name = "Test User" };
            _context.UserAccount.Add(user);
            _context.SaveChanges();

            var project1 = new Project { Name = "Test Project 1", UserAccountId = user.Id };
            _context.Project.Add(project1);
            _context.SaveChanges();

            var project2 = new Project { Name = "Test Project 2", UserAccountId = user.Id };
            _context.Project.Add(project2);
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetProjects_ForUser_ReturnsProjectList()
        {
            // Arrange
            var userId = _context.UserAccount.First().Id;

            var service = new ProjectService(_repository);
            var controller = new ProjectController(service);

            // Act
            var result = await controller.GetProjects(userId);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);

            var projectList = Assert.IsAssignableFrom<IEnumerable<Project>>(actionResult.Value);
            var projectListCount = Assert.IsAssignableFrom<List<Project>>(projectList);
            Assert.Equal(2, projectListCount.Count);
        }
    }

    // GetTasks
    [Collection("Sequential")]
    public class ProjectController_GetTasks : IntegrationTestBase
    {
        protected override void SeedDatabase()
        {
            var user = new UserAccount { Name = "Test User" };
            _context.UserAccount.Add(user);
            _context.SaveChanges();

            var project = new Project { Name = "Test Project", UserAccountId = user.Id };
            _context.Project.Add(project);
            _context.SaveChanges();

            var task1 = new ProjectTask(ProjectTaskPriority.High) { Title = "Test Task 1", Description = "Description", DueDate = DateTime.UtcNow, Status = ProjectTaskStatus.Pending, ProjectId = project.Id };
            _context.ProjectTask.Add(task1);
            _context.SaveChanges();

            var task2 = new ProjectTask(ProjectTaskPriority.High) { Title = "Test Task 2", Description = "Description", DueDate = DateTime.UtcNow, Status = ProjectTaskStatus.Pending, ProjectId = project.Id };
            _context.ProjectTask.Add(task2);
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetTasks_ByProjectId_ReturnsTaskList()
        {
            // Arrange
            var projectId = _context.Project.First().Id;

            var service = new ProjectService(_repository);
            var controller = new ProjectController(service);

            // Act
            var result = await controller.GetTasks(projectId);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);

            var taskList = Assert.IsAssignableFrom<IEnumerable<ProjectTask>>(actionResult.Value);
            Assert.NotEmpty(taskList);
            Assert.Equal(2, taskList.Count());
        }
    }

    [Collection("Sequential")]
    public class ProjectController_DeleteProject : IntegrationTestBase
    {
        protected override void SeedDatabase()
        {
            var user = new UserAccount { Name = "Test User" };
            _context.UserAccount.Add(user);
            _context.SaveChanges();

            var project = new Project { Name = "Test Project", UserAccountId = user.Id };
            _context.Project.Add(project);
            _context.SaveChanges();

            var task = new ProjectTask(ProjectTaskPriority.High) { Title = "Test Task", Description = "Description", DueDate = DateTime.UtcNow, Status = ProjectTaskStatus.Pending, ProjectId = project.Id };
            _context.ProjectTask.Add(task);
            _context.SaveChanges();
        }

        [Fact]
        public async Task DeleteProject_WhenProjectHasPendingTasks_ShouldNotAllowDeletion()
        {
            // Arrange
            var projectId = _context.Project.First().Id;

            var service = new ProjectService(_repository);
            var controller = new ProjectController(service);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => service.DeleteProjectAsync(projectId));
            Assert.Equal("Project has pending tasks. Complete or remove the tasks first.", exception.Message);
        }
    }

    [Collection("Sequential")]
    public class ProjectController_CreateProject : IntegrationTestBase
    {
        protected override void SeedDatabase()
        {
            var user = new UserAccount { Name = "Test User" };
            _context.UserAccount.Add(user);
            _context.SaveChanges();
        }

        [Fact]
        public async Task CreateProject_ReturnsProject()
        {
            // Arrange
            var project = new Project { Name = "Test Project", UserAccountId = _context.UserAccount.First().Id };

            var service = new ProjectService(_repository);
            var controller = new ProjectController(service);

            // Act
            var result = await controller.CreateProject(project);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);

            var createdProject = Assert.IsType<Project>(actionResult.Value);
            Assert.Equal(project.Name, createdProject.Name);
            Assert.Equal(project.UserAccountId, createdProject.UserAccountId);
        }
    }

    [Collection("Sequential")]
    public class ProjectController_CreateTask : IntegrationTestBase
    {
        protected override void SeedDatabase()
        {
            var user = new UserAccount { Name = "Test User" };
            _context.UserAccount.Add(user);
            _context.SaveChanges();

            var project = new Project { Name = "Test Project", UserAccountId = user.Id };
            _context.Project.Add(project);
            _context.SaveChanges();
        }

        [Fact]
        public async Task CreateTask_ReturnsTask()
        {
            // Arrange
            var task = new ProjectTask(ProjectTaskPriority.Medium) { Title = "Test Task", Description = "Description", DueDate = DateTime.UtcNow, Status = ProjectTaskStatus.Pending, ProjectId = _context.Project.First().Id };

            var service = new ProjectService(_repository);
            var controller = new ProjectController(service);

            // Act
            var result = await controller.CreateTask(task);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);

            var createdTask = Assert.IsType<ProjectTask>(actionResult.Value);
            Assert.Equal(task.Title, createdTask.Title);
            Assert.Equal(task.Description, createdTask.Description);
            Assert.Equal(task.DueDate, createdTask.DueDate);
            Assert.Equal(task.Status, createdTask.Status);
            Assert.Equal(task.ProjectId, createdTask.ProjectId);
        }
    }

    [Collection("Sequential")]
    public class ProjectController_UpdateTask : IntegrationTestBase
    {
        protected override void SeedDatabase()
        {
            var user = new UserAccount { Name = "Test User" };
            _context.UserAccount.Add(user);
            _context.SaveChanges();

            var project = new Project { Name = "Test Project", UserAccountId = user.Id };
            _context.Project.Add(project);
            _context.SaveChanges();

            var task = new ProjectTask(ProjectTaskPriority.High) { Title = "Test Task", Description = "Description", DueDate = DateTime.UtcNow, Status = ProjectTaskStatus.Pending, ProjectId = project.Id };
            _context.ProjectTask.Add(task);
            _context.SaveChanges();
        }

        [Fact]
        public async Task UpdateTask_ReturnsTask()
        {
            // Arrange
            var task = _context.ProjectTask.First();
            task.Title = "Updated Task";

            var service = new ProjectService(_repository);
            var controller = new ProjectController(service);

            // Act
            var result = await controller.UpdateTask(task);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);

            var updatedTask = Assert.IsType<ProjectTask>(actionResult.Value);
            Assert.Equal(task.Title, updatedTask.Title);
            Assert.Equal(task.Description, updatedTask.Description);
            Assert.Equal(task.DueDate, updatedTask.DueDate);
            Assert.Equal(task.Status, updatedTask.Status);
            Assert.Equal(task.ProjectId, updatedTask.ProjectId);
        }
    }
}