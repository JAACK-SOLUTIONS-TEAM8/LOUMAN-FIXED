using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.DTOs
{
    public class UpsertAdminDto
    {
        public int AdminUserId { get; set; }
        public int AdminId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int UserTypeId { get; set; }
        public string Initials { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string IdNumber{get;set;}
        public string CellNumber { get; set; }
    }
}
