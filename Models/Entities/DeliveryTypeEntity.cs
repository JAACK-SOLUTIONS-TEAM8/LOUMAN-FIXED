﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.Entities
{
    [Table("DeliveryType")]
    public class DeliveryTypeEntity
    {
        [Key]
        public int DeliveryTypeId { get; set; }
        public string Description { get; set; }
        public bool isDeleted { get; set; }
    }
}
