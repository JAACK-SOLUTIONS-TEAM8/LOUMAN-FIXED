using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.Entities
{
    [Table("Location")]
    public class LocationEntity
    {
        [Key]
        public int LocationId{get;set;}
        public string LocationArea{get;set;}
        public string LocationProvince{get;set;}
        public bool isDeleted{get;set;}
    }
}
