using EclipseWorks.TaskManagementSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EclipseWorks.TaskManagementSystem.Domain.Interfaces;
public interface IUserRepository
{
    Task<UserAccount?> GetUserByIdAsync(int userId);
}
