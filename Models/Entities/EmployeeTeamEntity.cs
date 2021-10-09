using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.Entities
{
    [Table("EmployeeTeam")]
     public class EmployeeTeamEntity
    {
        [Key]
      public int EmployeeTeamId{get;set;}
      public int EmployeeId{get;set;}
      public int TeamId{get;set;}
    }
}
