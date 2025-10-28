using CoreOnion_Backend.Application.Interfaces.UserInterfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreOnion_Backend.Application.Features.Users.Queries.GetRolesToUser
{
    public class GetRolesToUserQueryHandler : IRequestHandler<GetRolesToUserQueryRequest, GetRolesToUserQueryResponse>
    {
        readonly IUserReadRepository _userService;

        public GetRolesToUserQueryHandler(IUserReadRepository userService)
        {
            _userService = userService;
        }

        public async Task<GetRolesToUserQueryResponse> Handle(GetRolesToUserQueryRequest request, CancellationToken cancellationToken)
        {
            var userRoles = await _userService.GetRolesToUserAsync(request.UserId);
            return new()
            {
                UserRoles = userRoles
            };
        }
    }
}
