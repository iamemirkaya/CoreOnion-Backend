using CoreOnion_Backend.Application.Interfaces.Repositories;
using CoreOnion_Backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreOnion_Backend.Application.Interfaces.EndpointInterfaces
{
    public interface IEndpointReadRepository : IReadRepository<Endpoint>
    {

    }
}
