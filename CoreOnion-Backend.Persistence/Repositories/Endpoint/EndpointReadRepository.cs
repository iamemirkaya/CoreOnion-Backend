using CoreOnion_Backend.Application.Interfaces.EndpointInterfaces;
using CoreOnion_Backend.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreOnion_Backend.Persistence.Repositories.Endpoint
{
    public class EndpointReadRepository : ReadRepository<Domain.Entities.Endpoint>, IEndpointReadRepository
    {
        public EndpointReadRepository(AppDbContext context) : base(context)
        {
        }
    }
}
