using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Louman.Models.Entities
{
    [Table("Feature")]
    public class FeatureEntity
    {
        [Key]
        public int FeatureId { get; set; }
        public string FeatureName { get; set; }
    }
    
}
