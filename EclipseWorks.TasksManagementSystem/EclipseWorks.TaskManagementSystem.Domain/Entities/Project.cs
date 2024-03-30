using System.Text.Json.Serialization;

namespace EclipseWorks.TaskManagementSystem.Domain.Entities;
public class Project
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int UserAccountId { get; set; }

    [JsonIgnore]
    public UserAccount? UserAccount { get; set; } = null!;
    [JsonIgnore]
    public List<ProjectTask> Tasks { get; set; } = new List<ProjectTask>();
}
