using CoreOnion_Backend.Application.Interfaces.Configurations;
using CoreOnion_Backend.Application.Interfaces.IAuthorizationServices;
using CoreOnion_Backend.Application.Interfaces.UnitOfWorks;
using CoreOnion_Backend.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CoreOnion_Backend.Persistence.Services
{
    public class AuthorizationEndpointService : IAuthorizationEndpointService
    {
        readonly IApplicationService _applicationService;
        readonly RoleManager<Role> _roleManager;
        readonly IUnitOfWork _unitOfWork;

        public AuthorizationEndpointService(IApplicationService applicationService, IUnitOfWork unitOfWork, RoleManager<Role> roleManager)
        {
            _applicationService = applicationService;
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
        }

        public async Task AssignRoleEndpointAsync(string[] roles, string menu, string code, Type type)
        {
            var menuReadRepo = _unitOfWork.GetReadRepository<Menu>();
            var endpointReadRepo = _unitOfWork.GetReadRepository<Endpoint>();

            Menu _menu = await menuReadRepo.GetAsync(m => m.Name == menu); 
            if (_menu == null)
            {
                _menu = new() { Id = Guid.NewGuid(), Name = menu };
                await _unitOfWork.GetWriteRepository<Menu>().AddAsync(_menu);
            }

            Endpoint? endpoint = await endpointReadRepo.GetAsync(e => e.Code == code && e.Menu.Name == menu, include => include.Include(e => e.Menu).Include(e => e.Roles));

            if (endpoint == null)
            {
                var action = _applicationService.GetAuthorizeDefinitionEndpoints(type).FirstOrDefault(m => m.Name == menu)?.Actions.FirstOrDefault(e => e.Code == code);

                endpoint = new()
                {
                    Code = action.Code,
                    ActionType = action.ActionType,
                    HttpType = action.HttpType,
                    Definition = action.Definition,
                    Id = Guid.NewGuid(),
                    Menu = _menu
                };

                await _unitOfWork.GetWriteRepository<Endpoint>().AddAsync(endpoint);
            }

            endpoint.Roles.Clear();
            var appRoles = await _roleManager.Roles.Where(r => roles.Contains(r.Name)).ToListAsync();
            foreach (var role in appRoles)
                endpoint.Roles.Add(role);
            await _unitOfWork.SaveAsync();

        }

        public async Task<List<string>> GetRolesToEndpointAsync(string code, string menu)
        {
            var endpointReadRepo = _unitOfWork.GetReadRepository<Endpoint>();

            Endpoint? endpoint = await endpointReadRepo.GetAsync(e => e.Code == code && e.Menu.Name == menu,
        include => include.Include(e => e.Roles).Include(e => e.Menu));
            return endpoint?.Roles.Select(r => r.Name).ToList() ?? new List<string>();
        }
    }
}
