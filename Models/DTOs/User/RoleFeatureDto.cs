using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.DTOs.User
{
    public class RoleFeatureDto
    {
        public int RoleFeatureId { get; set; }
        public int FeatureId { get; set; }
        public int RoleId { get; set; }
        public bool isActive { get; set; }
    }
}
