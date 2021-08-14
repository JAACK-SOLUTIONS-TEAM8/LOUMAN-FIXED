using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.Entities
{
    [Table("MeetingStatus")]
    public class MeetingStatusEntity
    {
        [Key]
      public int MeetingStatusId { get; set; }
      public string MeetingStatusDescription { get; set; }
}
}
