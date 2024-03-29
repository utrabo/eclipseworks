namespace EclipseWorks.TaskManagementSystem.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new() { Title = "EclipseWorks.TaskManagementSystem.API", Version = "v1" });
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EclipseWorks.TaskManagementSystem.API v1"));

        app.MapControllers();

        app.Run();
    }
}
