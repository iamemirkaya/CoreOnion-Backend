using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreOnion_Backend.Application.DTOs
{
    public class PasswordResetMailDto
    {
        public string Email { get; set; }
        public string UserId { get; set; }
        public string ResetToken { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
