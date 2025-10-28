using CoreOnion_Backend.Application.Interfaces.AuthService;
using CoreOnion_Backend.Application.Interfaces.EndpointInterfaces;
using CoreOnion_Backend.Application.Interfaces.IAuthorizationServices;
using CoreOnion_Backend.Application.Interfaces.Repositories;
using CoreOnion_Backend.Application.Interfaces.RoleServices;
using CoreOnion_Backend.Application.Interfaces.UnitOfWorks;
using CoreOnion_Backend.Application.Interfaces.UserInterfaces;
using CoreOnion_Backend.Domain.Entities;
using CoreOnion_Backend.Persistence.Context;
using CoreOnion_Backend.Persistence.Repositories;
using CoreOnion_Backend.Persistence.Repositories.Endpoint;
using CoreOnion_Backend.Persistence.Repositories.UserRepositories;
using CoreOnion_Backend.Persistence.Services;
using CoreOnion_Backend.Persistence.UnitOfWorks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;



namespace CoreOnion_Backend.Persistence
{
    public static class Registration
    {
        public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(opt =>
            opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped(typeof(IReadRepository<>), typeof(ReadRepository<>));
            services.AddScoped(typeof(IWriteRepository<>), typeof(WriteRepository<>));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserWriteRepository, UserWriteRepository>();
            services.AddScoped<IUserReadRepository, UserReadRepository>();
            services.AddScoped<IEndpointReadRepository, EndpointReadRepository>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IAuthorizationEndpointService, AuthorizationEndpointService>();

            services.AddIdentityCore<User>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredLength = 2;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireDigit = false;
                opt.SignIn.RequireConfirmedEmail = false;
            })
                .AddRoles<Role>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();



        }
    }
}
