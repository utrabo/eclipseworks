using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EclipseWorks.TaskManagementSystem.Domain.Entities;
public class ProjectTaskComment
{
    public int Id { get; set; }
    public int ProjectTaskId { get; set; }
    public int UserAccountId { get; set; }
    public string Comment { get; set; } = null!;
    public DateTime SentAt { get; set; }

    // Navigation properties
    public ProjectTask ProjectTask { get; set; } = null!;
    public UserAccount UserAccount { get; set; } = null!;
}

