using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.Entities
{
    [Table("Address")]
    public class AddressEntity
    {
        [Key]
        public int AddressId { get; set; }
        public int StreetNumber { get; set; }
        public string StreetName { get; set; }
        public string CityName { get; set; }
        public string CityCode { get; set; }
    }
}
