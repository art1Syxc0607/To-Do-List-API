using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.DTO.Tag;

public class CreateTagDto
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
}