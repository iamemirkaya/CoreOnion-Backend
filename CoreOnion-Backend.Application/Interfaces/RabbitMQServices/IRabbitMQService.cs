using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreOnion_Backend.Application.Interfaces.RabbitMQServices
{
    public interface IRabbitMQService
    {
        void SendMessage(object message);
    }
}
