using EclipseWorks.TaskManagementSystem.Application.Interfaces;
using EclipseWorks.TaskManagementSystem.Application.Services;
using EclipseWorks.TaskManagementSystem.Domain.Interfaces;
using EclipseWorks.TaskManagementSystem.Infrastructure.Data;
using EclipseWorks.TaskManagementSystem.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EclipseWorks.TaskManagementSystem.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        DatabaseInitializer.InitializeDatabase(builder.Configuration.GetConnectionString("Master"));

        // Add services to the container.
        builder.Services.AddControllers();

        builder.Services.AddDbContext<EclipseWorksDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("EclipseWorksDatabase")));

        builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IProjectService, ProjectService>();
        builder.Services.AddScoped<IUserService, UserService>();

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new() { Title = "EclipseWorks.TaskManagementSystem.API", Version = "v1" });
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        //app.UseHttpsRedirection();
        app.UseAuthorization();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EclipseWorks.TaskManagementSystem.API v1"));

        app.MapControllers();

        app.Run();
    }
}
