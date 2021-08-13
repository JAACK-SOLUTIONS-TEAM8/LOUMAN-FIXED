using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.Entities
{
    [Table("User")]
    public class UserEntity
    {
        [Key]
        public int UserId { get; set; }
        public int UserTypeId { get; set; }
        public int? AddressId { get; set; }

        public string IdNumber { get; set; }

        public string Initials { get; set; }
        public string Surname{ get; set; }
        public string CellNumber{ get; set; }
        public string Email{ get; set; }
        public string UserName{ get; set; }
        public string Password{ get; set; }
        public bool isDeleted{ get; set; }
    }
}
