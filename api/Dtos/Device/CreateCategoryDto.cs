using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Device
{
    public class CreateCategoryDto
    {
        [Required]
        public string CategoryName { get; set; } = string.Empty;
    }
}
