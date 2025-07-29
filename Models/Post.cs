using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
namespace FindARoomate.Models;

public class Post
{
    [Key]
    public int Id { get; set; }
    [Required]
    [StringLength(280, ErrorMessage = "Post cannot exceed 280 characters")]
    public required String Content { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Required]
    public required String UserId { get; set; }
    public required String UserName { get; set; }

    // Navigation property for multiple images
    public required List<PostImage> Images { get; set; }
}