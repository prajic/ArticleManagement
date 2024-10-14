using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticlesManagement.Application.Models.Requests
{
    public class VerifyEmailRequest
    {
        public required string Email { get; set; }

        public required string VerificationCode { get; set; }
    }
}
