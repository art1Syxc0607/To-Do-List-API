using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.DTO.Category;

public class CreateCategoryDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    [MaxLength(7)]  // HEX цвет (#RRGGBB)
    public string? Color { get; set; }
}

