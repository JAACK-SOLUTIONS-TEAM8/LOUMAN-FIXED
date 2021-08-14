using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.Entities
{
    [Table("Title")]
    public class TitleEntity
    {
        [Key]
        public int TitleId { get; set; }
        public string TitleDescription { get; set; }
    }
}
