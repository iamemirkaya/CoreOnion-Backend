using CoreOnion_Backend.Application.Interfaces.UserInterfaces;
using CoreOnion_Backend.Domain.Entities;
using CoreOnion_Backend.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreOnion_Backend.Persistence.Repositories.UserRepositories
{
    public class UserReadRepository : IUserReadRepository
    {
        private readonly AppDbContext _context;

        public UserReadRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
