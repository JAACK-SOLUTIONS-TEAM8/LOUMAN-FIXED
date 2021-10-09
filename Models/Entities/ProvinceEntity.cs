using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.Entities
{
    [Table("Province")]
    public class ProvinceEntity
    {
        [Key]
        public int ProvinceId { get; set; }
        public string ProvinceName { get; set; }
    }
}
