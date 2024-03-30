using EclipseWorks.TaskManagementSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EclipseWorks.TaskManagementSystem.Application.Interfaces;
public interface IUserService
{
    Task<UserAccount?> GetUserByIdAsync(int userId);
}
