using System.ComponentModel.DataAnnotations;

namespace FindARoomate.Models;

public class CreatePostView
{
    [Required]
    [StringLength(280, ErrorMessage = "Post cannot exceed 280 characters")]
    public required String Content { get; set; }

    public required List<IFormFile> Images { get; set; }
}