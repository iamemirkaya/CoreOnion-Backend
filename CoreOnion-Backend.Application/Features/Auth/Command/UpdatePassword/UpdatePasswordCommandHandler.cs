using CoreOnion_Backend.Application.Exceptions;
using CoreOnion_Backend.Application.Interfaces.UserInterfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreOnion_Backend.Application.Features.Auth.Command.UpdatePassword
{
    public class UpdatePasswordCommandHandler : IRequestHandler<UpdatePasswordCommandRequest, UpdatePasswordCommandResponse>
    {
        readonly IUserWriteRepository _userService;

        public UpdatePasswordCommandHandler(IUserWriteRepository userService)
        {
            _userService = userService;
        }

        public async Task<UpdatePasswordCommandResponse> Handle(UpdatePasswordCommandRequest request, CancellationToken cancellationToken)
        {
            if (!request.Password.Equals(request.PasswordConfirm))
                throw new PasswordChangeFailedException("Lütfen şifreyi birebir doğrulayınız.");

            await _userService.UpdatePasswordAsync(request.UserId, request.ResetToken, request.Password);
            return new();
        }
    }
}
