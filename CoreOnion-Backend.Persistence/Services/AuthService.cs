using CoreOnion_Backend.Application.Helpers;
using CoreOnion_Backend.Application.Interfaces.AuthService;
using CoreOnion_Backend.Application.Interfaces.MailService;
using CoreOnion_Backend.Application.Interfaces.UserInterfaces;
using CoreOnion_Backend.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreOnion_Backend.Persistence.Services
{
    public class AuthService : IAuthService
    {
        readonly IMailService _mailService;
        private readonly IUserReadRepository _userReadRepository;
        private readonly UserManager<User> _userManager;

        public AuthService(IMailService mailService, IUserReadRepository userReadRepository, UserManager<User> userManager)
        {
            _mailService = mailService;
            _userReadRepository = userReadRepository;
            _userManager = userManager;
        }

        public async Task PasswordResetAsnyc(string email)
        {

            User user = await _userReadRepository.GetUserByEmailAsync(email);

            string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            resetToken = resetToken.UrlEncode();


            await _mailService.SendPasswordResetMailAsync(email, user.Id.ToString(), resetToken);
        }

        public async Task<bool> VerifyResetTokenAsync(string resetToken, string userId)
        {
            User user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {

                resetToken = resetToken.UrlDecode();

                return await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", resetToken);
            }
            return false;
        }

    }
}
