using CoreOnion_Backend.Application.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreOnion_Backend.Application.Features.Auth.Exceptions
{
    public class RefreshTokenShouldNotBeExpiredException : BaseException
    {
        public RefreshTokenShouldNotBeExpiredException() : base("Oturum süresi sona ermiştir. Lütfen tekrar giriş yapın.") { }
    }
}
