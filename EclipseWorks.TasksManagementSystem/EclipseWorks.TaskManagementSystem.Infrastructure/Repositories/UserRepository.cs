using EclipseWorks.TaskManagementSystem.Domain.Entities;
using EclipseWorks.TaskManagementSystem.Domain.Interfaces;
using EclipseWorks.TaskManagementSystem.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EclipseWorks.TaskManagementSystem.Infrastructure.Repositories;
public class UserRepository : IUserRepository
{
    private readonly EclipseWorksDbContext _context;

    public UserRepository(EclipseWorksDbContext context)
    {
        _context = context;
    }

    public async Task<UserAccount?> GetUserByIdAsync(int userId)
    {
        return await _context.UserAccount.FindAsync(userId);
    }
}
