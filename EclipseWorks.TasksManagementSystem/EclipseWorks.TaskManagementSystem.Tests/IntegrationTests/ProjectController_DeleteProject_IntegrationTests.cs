using EclipseWorks.TaskManagementSystem.API.Controllers;
using EclipseWorks.TaskManagementSystem.Application.Services;
using EclipseWorks.TaskManagementSystem.Domain.Entities;
using EclipseWorks.TaskManagementSystem.Domain.Interfaces;
using EclipseWorks.TaskManagementSystem.Infrastructure.Data;
using EclipseWorks.TaskManagementSystem.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EclipseWorks.TaskManagementSystem.Tests.IntegrationTests;
public class ProjectControllerIntegrationTests
{
    public class ProjectController_DeleteProject_IntegrationTests : IDisposable
    {

        private readonly EclipseWorksDbContext _context;
        private readonly IConfigurationRoot _configuration;
        private readonly IProjectRepository _repository;
        private readonly IDbContextTransaction _transaction;

        public ProjectController_DeleteProject_IntegrationTests()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var options = new DbContextOptionsBuilder<EclipseWorksDbContext>()
                .UseSqlServer(_configuration.GetConnectionString("EclipseWorksDatabase"))
                .Options;

            _context = new EclipseWorksDbContext(options);
            _repository = new ProjectRepository(_context);

            _transaction = _context.Database.BeginTransaction();

            SeedDatabase();
        }

        
        private void SeedDatabase()
        {
            var user = new UserAccount { Name = "Test User" };
            _context.UserAccount.Add(user);
            _context.SaveChanges();

            var project = new Project { Name = "Test Project", UserAccountId = user.Id };
            _context.Project.Add(project);
            _context.SaveChanges();

            var task = new ProjectTask("Test Task", "Description", DateTime.UtcNow, ProjectTaskStatus.Pending, ProjectTaskPriority.High) { ProjectId = project.Id };
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

        public void Dispose()
        {
            _transaction.Rollback();
            _context.Dispose();
        }
    }
}