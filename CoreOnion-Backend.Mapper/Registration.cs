using CoreOnion_Backend.Application.Interfaces.AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreOnion_Backend.Mapper
{
    public static class Registration
    {
        public static void AddCustomMapper(this IServiceCollection services)
        {
            services.AddSingleton<IMapper, AutoMapper.Mapper>();
        }
    }
}
