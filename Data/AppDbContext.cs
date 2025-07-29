using FindARoomate.Models;
using Microsoft.EntityFrameworkCore;

namespace FindARoomate.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Post> posts { get; set; }
        public DbSet<PostImage> postImages { get; set; }
    }
}