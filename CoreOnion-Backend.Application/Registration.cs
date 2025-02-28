using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CoreOnion_Backend.Application
{
    public static class Registration
    {
        public static void AddApplication(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();



            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

        }
    }
}
