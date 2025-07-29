using FindARoomate.Areas.Identity.Pages.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MyMvcApp.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<City> Cities { get; set; }
    public DbSet<Gander> Ganders  { get; set; }

    public DbSet<Conversation> conversations { get; set; }
    public DbSet<Message> messages { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>()
            .HasOne(a => a.cities)
            .WithMany(c => c.Users)
            .HasForeignKey(a => a.CityId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<ApplicationUser>()
            .HasOne(a => a.ganders)
            .WithMany(g => g.Users)
            .HasForeignKey(a => a.GanderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<City>().HasData(
        new City { Id = 1, Name = "جدة" },
        new City { Id = 2, Name = "مكة" },
        new City { Id = 3, Name = "الرياض" },
        new City { Id = 4, Name = "ابها" },
        new City { Id = 5, Name = "جازان" }
        );
        builder.Entity<Gander>().HasData(
        new Gander { Id = 1, Name = "ذكر" },
        new Gander { Id = 2, Name = "انثى" }
        );

        builder.Entity<Conversation>()
        .HasOne(c => c.User1)
        .WithMany()
        .HasForeignKey(c => c.User1Id)
        .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

        builder.Entity<Conversation>()
        .HasOne(c => c.User2)
        .WithMany()
        .HasForeignKey(c => c.User2Id)
        .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

        builder.Entity<Message>()
        .HasOne(m => m.Sender)
        .WithMany()
        .HasForeignKey(m => m.SenderId)
        .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

        builder.Entity<Message>()
        .HasOne(m => m.Conversation)
        .WithMany(c => c.messages)
        .HasForeignKey(m => m.ConversationId)
        .OnDelete(DeleteBehavior.Cascade); // This one can remain

        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
