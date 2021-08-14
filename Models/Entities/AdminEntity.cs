using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.Entities
{
    [Table("Admin")]
    public class AdminEntity
    {
        [Key]
        public int AdminId { get; set; }
        public int UserId { get; set; }
    }
}
