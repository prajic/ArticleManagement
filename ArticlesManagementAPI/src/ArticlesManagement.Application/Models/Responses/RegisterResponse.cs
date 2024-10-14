using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticlesManagement.Application.Models.Responses
{
    public class RegisterResponse
    {
        public bool Success { get; set; }
        public string? VerificationCode { get; set; } // Include verification code
        public IEnumerable<string>? Errors { get; set; }
    }
}
