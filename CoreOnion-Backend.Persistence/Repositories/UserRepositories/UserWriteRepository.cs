using CoreOnion_Backend.Application.Exceptions;
using CoreOnion_Backend.Application.Helpers;
using CoreOnion_Backend.Application.Interfaces.UserInterfaces;
using CoreOnion_Backend.Domain.Entities;
using CoreOnion_Backend.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreOnion_Backend.Persistence.Repositories.UserRepositories
{
    public class UserWriteRepository : IUserWriteRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public async Task UpdatePasswordAsync(string userId, string resetToken, string newPassword)
        {
            User user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                resetToken = resetToken.UrlDecode();
                IdentityResult result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
                if (result.Succeeded)
                    await _userManager.UpdateSecurityStampAsync(user);
                else
                    throw new PasswordChangeFailedException();
            }
        }
    }
}
