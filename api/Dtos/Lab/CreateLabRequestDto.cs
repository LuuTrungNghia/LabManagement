using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Lab
{
    public class CreateLabRequestDto
    {
        [Required]
        [MaxLength(100)]
        public string LabName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public bool IsAvailable { get; set; }
    }
}
