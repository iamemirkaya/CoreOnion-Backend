using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreOnion_Backend.RabbitMQ.model
{
    public class MailSendMessage
    {
        public string To { get; set; }
        public string[] Tos { get; set; }  
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsBodyHtml { get; set; } = true;

        public string? PasswordResetUserId { get; set; }
        public string? PasswordResetToken { get; set; }
    }
}
