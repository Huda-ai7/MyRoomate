using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FindARoomate.Models;
public class PostImage
{
    [Key]
    public int Id { get; set; }

    [Required]
    public required String ImagePath { get; set; }
        
    public int PostId { get; set; }
    [ForeignKey("PostId")]
    public Post? posts { get; set; }
}