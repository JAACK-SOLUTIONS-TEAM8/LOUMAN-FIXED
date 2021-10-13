using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.DTOs.User
{
    public class UserWithRolesDto
    {
        public int UserId { get; set; }
        public int UserTypeId { get; set; }
        public int? AddressId { get; set; }

        public string IdNumber { get; set; }

        public string Initials { get; set; }
        public string Surname { get; set; }
        public string CellNumber { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }

        public List<string> UserRoles { get; set; }
    }
}
