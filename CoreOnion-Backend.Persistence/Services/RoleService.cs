using CoreOnion_Backend.Application.Interfaces.RoleServices;
using CoreOnion_Backend.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreOnion_Backend.Persistence.Services
{
    public class RoleService : IRoleService
    {
        readonly RoleManager<Role> _roleManager;

        public RoleService(RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<bool> CreateRole(string name)
        {
            IdentityResult result = await _roleManager.CreateAsync(new() { Id = Guid.NewGuid(), Name = name });

            return result.Succeeded;
        }

        public async Task<bool> DeleteRole(Guid id)
        {
            Role role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null) return false; 

            IdentityResult result = await _roleManager.DeleteAsync(role);
            return result.Succeeded;
        }

        public (object, int) GetAllRoles(int page, int size)
        {
            var query = _roleManager.Roles;

            IQueryable<Role> rolesQuery = null;

            if (page != -1 && size != -1)
                rolesQuery = query.Skip(page * size).Take(size);
            else
                rolesQuery = query;

            return (rolesQuery.Select(r => new { r.Id, r.Name }), query.Count());
        }

        public async Task<(Guid id, string name)> GetRoleById(Guid id)
        {
            Role role = await _roleManager.FindByIdAsync(id.ToString());

            if (role != null)
            {
                return (role.Id, role.Name);
            }
            return (Guid.Empty, null);
        }

        public async Task<bool> UpdateRole(Guid id, string name)
        {
            Role role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null) return false; 

            role.Name = name;
            IdentityResult result = await _roleManager.UpdateAsync(role);
            return result.Succeeded;
        }
    }
}
