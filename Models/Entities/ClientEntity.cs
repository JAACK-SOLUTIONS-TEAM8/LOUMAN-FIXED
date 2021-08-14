using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.Entities
{
    [Table("Client")]
    public class ClientEntity
    {
        [Key]
        public int ClientId{get;set;}

        public int UserId{get;set;}
    }
}
