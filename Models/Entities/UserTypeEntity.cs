using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.Entities
{
    [Table("UserType")]
    public class UserTypeEntity
    {
        [Key]

        public int UserTypeId { get; set; }
        public string UserTypeDescription { get; set; }
        public bool isDeleted { get; set; }
    }
}
