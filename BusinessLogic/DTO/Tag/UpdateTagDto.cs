using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.DTO.Tag;

public class UpdateTagDto
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
}