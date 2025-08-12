using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using sr.Models;
using System.Drawing;

namespace sr.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationApplicationUser> UserNotifications { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<NotificationApplicationUser>(entity =>
            {
                // PK composta
                entity.HasKey(k => new { k.NotificationId, k.ApplicationUserId });

                // Força tipo exato que existe no banco
                entity.Property(p => p.ApplicationUserId)
                      .HasColumnType("int")      // garante que é int
                      .IsRequired();             // evita nullable se no banco não é

                // Relação com Notification
                entity.HasOne(n => n.Notification)
                      .WithMany(n => n.NotificationApplicationUsers)
                      .HasForeignKey(n => n.NotificationId);

                // Relação com ApplicationUser
                entity.HasOne(n => n.ApplicationUser)
                      .WithMany(u => u.NotificationApplicationUsers)
                      .HasForeignKey(n => n.ApplicationUserId);
            });

            base.OnModelCreating(builder);
        }

    }
}
