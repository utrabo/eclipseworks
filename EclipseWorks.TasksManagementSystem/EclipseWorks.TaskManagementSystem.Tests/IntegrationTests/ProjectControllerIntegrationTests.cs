using EclipseWorks.TaskManagementSystem.API.Controllers;
using EclipseWorks.TaskManagementSystem.API.Requests;
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
            var userId = _context.UserAccount.OrderByDescending(t => t.Id).First().Id;

            var projService = new ProjectService(_projectRepository, _userRepository);
            var userService = new UserService(_userRepository);
            var controller = new ProjectController(projService, userService);

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

            var task1 = new ProjectTask(ProjectTaskPriority.High) { Title = "Test Task 1", Description = "Description", DueDate = DateTime.UtcNow, Status = ProjectTaskStatus.Pending, ProjectId = project.Id, AssignedToUserAccount = user };
            _context.ProjectTask.Add(task1);
            _context.SaveChanges();

            var task2 = new ProjectTask(ProjectTaskPriority.High) { Title = "Test Task 2", Description = "Description", DueDate = DateTime.UtcNow, Status = ProjectTaskStatus.Pending, ProjectId = project.Id , AssignedToUserAccount = user };
            _context.ProjectTask.Add(task2);
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetTasks_ByProjectId_ReturnsTaskList()
        {
            // Arrange
            var projectId = _context.Project.OrderByDescending(t => t.Id).First().Id;

            var projService = new ProjectService(_projectRepository, _userRepository);
            var userService = new UserService(_userRepository);
            var controller = new ProjectController(projService, userService);

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

            var task = new ProjectTask(ProjectTaskPriority.High) { Title = "Test Task", Description = "Description", DueDate = DateTime.UtcNow, Status = ProjectTaskStatus.Pending, ProjectId = project.Id , AssignedToUserAccount = user };
            _context.ProjectTask.Add(task);
            _context.SaveChanges();
        }

        [Fact]
        public async Task DeleteProject_WhenProjectHasPendingTasks_ShouldNotAllowDeletion()
        {
            // Arrange
            var projectId = _context.Project.OrderByDescending(t => t.Id).First().Id;

            var projService = new ProjectService(_projectRepository, _userRepository);
            var userService = new UserService(_userRepository);
            var controller = new ProjectController(projService, userService);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => projService.DeleteProjectAsync(projectId));
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

            var projService = new ProjectService(_projectRepository, _userRepository);
            var userService = new UserService(_userRepository);
            var controller = new ProjectController(projService, userService);

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
    public class ProjectController_UpdateProject : IntegrationTestBase
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
        public async Task UpdateProject_ReturnsProject()
        {
            // Arrange
            var project = _context.Project.OrderByDescending(t=> t.Id).First();
            var projectUpdateDto = new ProjectUpdateRequestDto { Name = "Updated Project" };

            var projService = new ProjectService(_projectRepository, _userRepository);
            var userService = new UserService(_userRepository);
            var controller = new ProjectController(projService, userService);

            // Act
            var result = await controller.UpdateProject(project.Id, projectUpdateDto);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);

            var updatedProject = Assert.IsType<Project>(actionResult.Value);
            Assert.Equal(project.Name, updatedProject.Name);
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
            var user = _context.UserAccount.OrderByDescending(t => t.Id).First();
            var project = _context.Project.OrderByDescending(t => t.Id).First();
            var task = new ProjectTask(ProjectTaskPriority.Medium) { Title = "Test Task", Description = "Description", DueDate = DateTime.UtcNow, Status = ProjectTaskStatus.Pending, ProjectId = project.Id, AssignedToUserAccountId = user.Id };

            var projService = new ProjectService(_projectRepository, _userRepository);
            var userService = new UserService(_userRepository);
            var controller = new ProjectController(projService, userService);

            // Act
            var result = await controller.CreateTask(project.Id, task);

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

            var task = new ProjectTask(ProjectTaskPriority.High) { Title = "Test Task", Description = "Description", DueDate = DateTime.UtcNow, Status = ProjectTaskStatus.Pending, ProjectId = project.Id, AssignedToUserAccount = user };
            _context.ProjectTask.Add(task);
            _context.SaveChanges();
        }

        [Fact]
        public async Task UpdateTask_ReturnsTask()
        {
            // Arrange
            var project = _context.Project.OrderByDescending(t => t.Id).First();
            var task = _context.ProjectTask.OrderByDescending(t => t.Id).First();

            var taskUpdateDto = new ProjectTaskUpdateRequestDto
            {
                Title = "Updated Task",
                Description = "Updated Description",
                DueDate = DateTime.UtcNow.AddDays(1),
                Status = ProjectTaskStatus.Completed
            };

            var user = _context.UserAccount.OrderByDescending(t => t.Id).First();

            var projService = new ProjectService(_projectRepository, _userRepository);
            var userService = new UserService(_userRepository);
            var controller = new ProjectController(projService, userService);

            // Act
            var result = await controller.UpdateTask(user.Id, project.Id, task.Id, taskUpdateDto);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);

            var updatedTask = Assert.IsType<ProjectTask>(actionResult.Value);
            Assert.Equal(task.Title, updatedTask.Title);
            Assert.Equal(task.Description, updatedTask.Description);
            Assert.Equal(task.DueDate, updatedTask.DueDate);
            Assert.Equal(task.Status, updatedTask.Status);
            Assert.Equal(task.ProjectId, updatedTask.ProjectId);

            var history = _context.ProjectTaskHistory.FirstOrDefault(h => h.ProjectTaskId == task.Id);
            Assert.NotNull(history);
            Assert.Equal("Title", history.PropertyName);
            Assert.Equal("Test Task", history.OriginalValue);
            Assert.Equal("Updated Task", history.CurrentValue);
        }
    }

    [Collection("Sequential")]
    public class ProjectController_DeleteTask : IntegrationTestBase
    {
        protected override void SeedDatabase()
        {
            var user = new UserAccount { Name = "Test User" };
            _context.UserAccount.Add(user);
            _context.SaveChanges();

            var project = new Project { Name = "Test Project", UserAccountId = user.Id };
            _context.Project.Add(project);
            _context.SaveChanges();

            var task = new ProjectTask(ProjectTaskPriority.High) { Title = "Test Task", Description = "Description", DueDate = DateTime.UtcNow, Status = ProjectTaskStatus.Pending, ProjectId = project.Id, AssignedToUserAccount = user };
            _context.ProjectTask.Add(task);
            _context.SaveChanges();
        }

        [Fact]
        public async Task DeleteTask_ReturnsOk()
        {
            // Arrange
            var taskId = _context.ProjectTask.OrderByDescending(t=> t.Id).First().Id;
            var project = _context.Project.OrderByDescending(t => t.Id).First();

            var projService = new ProjectService(_projectRepository, _userRepository);
            var userService = new UserService(_userRepository);
            var controller = new ProjectController(projService, userService);

            // Act
            var result = await controller.DeleteTask(project.Id, taskId);

            // Assert
            Assert.IsType<OkResult>(result);
        }
    }

    [Collection("Sequential")]
    public class ProjectController_AddTaskComment : IntegrationTestBase
    {
        protected override void SeedDatabase()
        {
            var user = new UserAccount { Name = "Test User" };
            _context.UserAccount.Add(user);
            _context.SaveChanges();

            var project = new Project { Name = "Test Project", UserAccountId = user.Id };
            _context.Project.Add(project);
            _context.SaveChanges();

            var task = new ProjectTask(ProjectTaskPriority.High) { Title = "Test Task", Description = "Description", DueDate = DateTime.UtcNow, Status = ProjectTaskStatus.Pending, ProjectId = project.Id, AssignedToUserAccount = user };
            _context.ProjectTask.Add(task);
            _context.SaveChanges();
        }

        [Fact]
        public async Task AddTaskComment_ReturnsTaskComment()
        {
            // Arrange
            var user = _context.UserAccount.OrderByDescending(t => t.Id).First();
            var task = _context.ProjectTask.OrderByDescending(t => t.Id).First();

            var taskComment = new ProjectTaskComment { Comment = "Test Comment", ProjectTaskId = task.Id, SentAt = DateTime.UtcNow, UserAccount = user };
            var taskCommentDto = new ProjectTaskCommentAddRequestDto { Comment = "Test Comment", UserId = user.Id };

            var projService = new ProjectService(_projectRepository, _userRepository);
            var userService = new UserService(_userRepository);
            var controller = new ProjectController(projService, userService);

            // Act
            var result = await controller.AddTaskComment(task.Id, taskCommentDto);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);

            var createdComment = Assert.IsType<ProjectTaskComment>(actionResult.Value);
            Assert.Equal(taskComment.Comment, createdComment.Comment);
            Assert.Equal(taskComment.ProjectTaskId, createdComment.ProjectTaskId);

            var history = _context.ProjectTaskHistory.FirstOrDefault(h => h.ProjectTaskCommentId == createdComment.Id);
            Assert.NotNull(history);
        }
    }

}