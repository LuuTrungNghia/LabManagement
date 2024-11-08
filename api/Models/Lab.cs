using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    [Table("Labs")]
    public class Lab
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string LabName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public bool IsAvailable { get; set; }
    }
}
