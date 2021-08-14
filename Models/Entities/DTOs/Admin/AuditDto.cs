
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.DTOs.Admin
{
    public class AuditDto
    {
        public int AuditId { get; set; }
        public int UserId { get; set; }
        public string UserName{get;set;}
        public DateTime Date { get; set; }
        public string Operation { get; set; }

    }
}
