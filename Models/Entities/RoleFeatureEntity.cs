using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Louman.Models.Entities
{
    [Table("RoleFeature")]
    public class RoleFeatureEntity
    {
        [Key]
        public int RoleFeatureId { get; set; }
        public int FeatureId { get; set; }
        public int RoleId { get; set; }
        public bool isActive { get; set; }
    }
}
