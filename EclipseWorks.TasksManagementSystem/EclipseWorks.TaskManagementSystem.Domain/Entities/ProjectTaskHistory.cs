using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EclipseWorks.TaskManagementSystem.Domain.Entities;
public class ProjectTaskHistory
{
    public int Id { get; set; }
    public int ProjectTaskId { get; set; }
    public string PropertyName { get; set; } = null!;
    public string OriginalValue { get; set; } = null!;
    public string CurrentValue { get; set; } = null!;
    public DateTime ChangedAt { get; set; }
    public int ChangedByUserId { get; set; }
    public int? ProjectTaskCommentId { get; set; }

    // Navigation properties
    public ProjectTask ProjectTask { get; set; } = null!;
    public UserAccount ChangedByUser { get; set; } = null!;
    public ProjectTaskComment? ProjectTaskComment { get; set; }
}
