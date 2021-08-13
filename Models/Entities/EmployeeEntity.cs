using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.Entities
{
    [Table("Employee")]
    public class EmployeeEntity
    {
        [Key]
      public int EmployeeId {get;set;}
      public int UserId{get;set;}
      public DateTime? CommencementDate{get;set;}
      public DateTime? TerminationDate {get;set;}
      public string TerminationReason{get;set;}
      public string Image{get;set;}
     public string Document { get; set; }

    }
}
