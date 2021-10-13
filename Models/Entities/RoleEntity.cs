using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.Entities
{
    [Table("Role")]
    public class RoleEntity
    {
        [Key]
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
