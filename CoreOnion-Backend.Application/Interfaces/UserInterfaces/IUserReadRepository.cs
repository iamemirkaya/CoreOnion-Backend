using CoreOnion_Backend.Application.DTOs;
using CoreOnion_Backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreOnion_Backend.Application.Interfaces.UserInterfaces
{
    public interface IUserReadRepository
    {
        Task<User> GetUserByEmailAsync(string email);

        Task<List<ListUser>> GetAllUsersAsync(int page, int size);

        Task<string[]> GetRolesToUserAsync(string userIdOrName);

        Task<bool> HasRolePermissionToEndpointAsync(string name, string code);
    }
}
