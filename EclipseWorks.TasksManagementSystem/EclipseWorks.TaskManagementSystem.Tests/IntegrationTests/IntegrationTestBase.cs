using EclipseWorks.TaskManagementSystem.Domain.Interfaces;
using EclipseWorks.TaskManagementSystem.Infrastructure.Data;
using EclipseWorks.TaskManagementSystem.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EclipseWorks.TaskManagementSystem.Tests.IntegrationTests;
public class IntegrationTestBase : IDisposable
{
    protected readonly EclipseWorksDbContext _context;
    protected readonly IConfigurationRoot _configuration;
    protected readonly IProjectRepository _repository;
    protected readonly IDbContextTransaction _transaction;

    public IntegrationTestBase()
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

    protected virtual void SeedDatabase()
    {
    }

    public void Dispose()
    {
        _transaction.Rollback();
        _context.Dispose();
    }
}

[CollectionDefinition("Sequential")]
public class SequentialCollection : ICollectionFixture<IntegrationTestBase> { }