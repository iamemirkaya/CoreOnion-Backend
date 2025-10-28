using CoreOnion_Backend.Application.DTOs;
using CoreOnion_Backend.Application.Interfaces.EndpointInterfaces;
using CoreOnion_Backend.Application.Interfaces.UserInterfaces;
using CoreOnion_Backend.Domain.Entities;
using CoreOnion_Backend.Persistence.Context;
using Microsoft.AspNetCore.Identity;
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

        private readonly UserManager<User> _userManager;

        readonly IEndpointReadRepository _endpointReadRepository;

        public UserReadRepository(AppDbContext context, UserManager<User> userManager, IEndpointReadRepository endpointReadRepository)
        {
            _context = context;
            _userManager = userManager;
            _endpointReadRepository = endpointReadRepository;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<List<ListUser>> GetAllUsersAsync(int page, int size)
        {
            var users = await _context.Users
                .Skip(page * size)
                .Take(size)
                .ToListAsync();

            return users.Select(user => new ListUser
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                UserName = user.UserName

            }).ToList();
        }

        public async Task<string[]> GetRolesToUserAsync(string userIdOrName)
        {
            User user = await _userManager.FindByIdAsync(userIdOrName);
            if (user == null)
                user = await _userManager.FindByNameAsync(userIdOrName);

            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                return userRoles.ToArray();
            }
            return new string[] { };
        }

        public async Task<bool> HasRolePermissionToEndpointAsync(string name, string code)
        {
            var userRoles = await GetRolesToUserAsync(name);

            if (!userRoles.Any())
                return false;

            Domain.Entities.Endpoint? endpoint = await _endpointReadRepository.Table
                     .Include(e => e.Roles)
                     .FirstOrDefaultAsync(e => e.Code == code);

            if (endpoint == null)
                return false;

            var hasRole = false;
            var endpointRoles = endpoint.Roles.Select(r => r.Name);

            foreach (var userRole in userRoles)
            {
                foreach (var endpointRole in endpointRoles)
                    if (userRole == endpointRole)
                        return true;
            }

            return false;
        }
    }
}
