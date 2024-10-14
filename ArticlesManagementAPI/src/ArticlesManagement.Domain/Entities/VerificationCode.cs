using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticlesManagement.Domain.Entities
{
    public class VerificationCode
    {
        public int UserId { get; set; }                     
        public required string Code { get; set; }        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 
        public DateTime ExpiresAt { get; set; }              
        public bool IsUsed { get; set; } = false;           

    }
}
