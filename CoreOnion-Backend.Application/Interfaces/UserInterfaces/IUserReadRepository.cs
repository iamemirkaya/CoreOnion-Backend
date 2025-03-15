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
    }
}
