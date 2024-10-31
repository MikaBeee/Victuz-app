using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Diagnostics;
using Victuz.Models.Businesslayer;

namespace Victuz.Data
{
    public class VictuzDB : DbContext
    {
        public DbSet<Gathering> gathering { get; set; }
        public DbSet<GatheringRegistration> activitieregistration { get; set; }
        public DbSet<Category> categorie { get; set; }
        public DbSet<Forum> forum { get; set; }
        public DbSet<Location> location { get; set; }
        public DbSet<Post> post { get; set; }
        public DbSet<Role> role { get; set; }
        public DbSet<User> users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connection = "Data Source=.\\SQLSERVERSVEN;Initial Catalog=victuz;Integrated Security=True;Trust Server Certificate=True";
            optionsBuilder.UseSqlServer(connection);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GatheringRegistration>()
                .HasKey(gr => new { gr.UserId, gr.GatheringId});

            modelBuilder.Entity<Gathering>()
                .HasOne(g => g.Location)
                .WithMany(l => l.Gatherings)
                .HasForeignKey(g => g.LocationId);

            modelBuilder.Entity<Gathering>()
                .HasOne(g => g.Category)
                .WithMany(c => c.Gatherings)
                .HasForeignKey(g => g.CategoryId);

            modelBuilder.Entity<GatheringRegistration>()
                .HasOne(gr => gr.Gathering)
                .WithMany(g => g.GatheringRegistrations)
                .HasForeignKey(gr => gr.GatheringId);

            modelBuilder.Entity<GatheringRegistration>()
                .HasOne(gr => gr.User)
                .WithMany(u => u.GatheringRegistrations)
                .HasForeignKey(gr => gr.UserId);

            modelBuilder.Entity<Post>()
                .HasOne(p => p.Forum)
                .WithMany(f => f.Posts)
                .HasForeignKey(p => p.ForumId);

            modelBuilder.Entity<Post>()
                .HasOne(p => p.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId);
        }
    }
}
