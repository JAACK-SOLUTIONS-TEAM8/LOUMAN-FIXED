using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.Entities
{
    [Table("CardDetail")]
    public class CardDetailEntity
    {
        [Key]
        public int CardDetailId { get; set; }
        public int ClientUserId { get; set; }
        public string HolderName {get;set;}
        public string CardNumber { get; set; }
        public string SecurityNumber { get; set; }
        public bool isDeleted { get; set; }
    }
}
