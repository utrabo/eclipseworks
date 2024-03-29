namespace EclipseWorks.TaskManagementSystem.Domain.Entities;
public class UserAccount
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public UserAccountRole Role { get; set; } = UserAccountRole.User;
}
