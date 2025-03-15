using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreOnion_Backend.Application.Interfaces.AuthService
{
    public interface IAuthService
    {
        Task PasswordResetAsnyc(string email);

        Task<bool> VerifyResetTokenAsync(string resetToken, string userId);
    }
}
