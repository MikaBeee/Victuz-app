using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Diagnostics;
using Victuz.Models.Businesslayer;

namespace Victuz.Data
{
    public class VictuzDB : DbContext
    {
        public DbSet<Gathering> gathering { get; set; }
        public DbSet<GatheringRegistration> gatheringRegistration { get; set; }
        public DbSet<Category> categorie { get; set; }
        public DbSet<Forum> forum { get; set; }
        public DbSet<Location> location { get; set; }
        public DbSet<Post> post { get; set; }
        public DbSet<Role> role { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<Vote> votes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connection = "Data Source=.;Initial Catalog=victuz;Integrated Security=True;Trust Server Certificate=True";
            optionsBuilder.UseSqlServer(connection);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GatheringRegistration>()
                .HasKey(gr => new { gr.UserId, gr.GatheringId});

            modelBuilder.Entity<Vote>()
                .HasKey(v => new { v.UserId, v.GatheringId });

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

            modelBuilder.Entity<Vote>()
                .HasOne(v => v.User)
                .WithMany(u => u.Votes)
                .HasForeignKey(v => v.UserId);

            modelBuilder.Entity<Vote>()
                .HasOne(v => v.Gathering)
                .WithMany(u => u.Votes)
                .HasForeignKey(v => v.GatheringId);

            modelBuilder.Entity<Role>().HasData(
                new Role
                {
                    RoleId = 1,
                    RoleName = "admin"
                },
                new Role
                {
                    RoleId = 2,
                    RoleName = "user"
                },
                new Role
                {
                    RoleId = 3,
                    RoleName = "guest"
                });

            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    CatId = 1,
                    CatName = "feest"
                },
                new Category
                {
                    CatId = 2,
                    CatName = "bijeenkomst"
                },
                new Category
                {
                    CatId = 3,
                    CatName = "event"
                });

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    UserName = "admin",
                    Password = "AQAAAAIAAYagAAAAEBvheVwG4KNnrDnEPSA0V126sHMEXyRza1BFh8fw9M/IHFui7ETKRuHJL6lQp6OUWA==",
                    RoleId = 1
                },
                new User
                {
                    UserId = 2,
                    UserName = "mika",
                    Password = "AQAAAAIAAYagAAAAEBvheVwG4KNnrDnEPSA0V126sHMEXyRza1BFh8fw9M/IHFui7ETKRuHJL6lQp6OUWA==",
                    RoleId = 2
                },
                new User
                {
                    UserId = 3,
                    UserName = "sven",
                    Password = "AQAAAAIAAYagAAAAEBvheVwG4KNnrDnEPSA0V126sHMEXyRza1BFh8fw9M/IHFui7ETKRuHJL6lQp6OUWA==",
                    RoleId = 2
                },
                new User
                {
                    UserId = 4,
                    UserName = "charlotte",
                    Password = "AQAAAAIAAYagAAAAEBvheVwG4KNnrDnEPSA0V126sHMEXyRza1BFh8fw9M/IHFui7ETKRuHJL6lQp6OUWA==",
                    RoleId = 2
                });
        }
    }
}
