﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Victuz.Data;

#nullable disable

namespace Victuz.Migrations
{
    [DbContext(typeof(VictuzDB))]
    partial class VictuzDBModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Victuz.Models.Businesslayer.Category", b =>
                {
                    b.Property<int>("CatId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CatId"));

                    b.Property<string>("CatName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CatId");

                    b.ToTable("categorie");
                });

            modelBuilder.Entity("Victuz.Models.Businesslayer.Forum", b =>
                {
                    b.Property<int>("ForumId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ForumId"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ForumId");

                    b.ToTable("forum");
                });

            modelBuilder.Entity("Victuz.Models.Businesslayer.Gathering", b =>
                {
                    b.Property<int>("GatheringId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("GatheringId"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("GatheringDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GatheringTitle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("LocationId")
                        .HasColumnType("int");

                    b.Property<int>("MaxParticipants")
                        .HasColumnType("int");

                    b.Property<string>("Photopath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("GatheringId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("LocationId");

                    b.ToTable("gathering");
                });

            modelBuilder.Entity("Victuz.Models.Businesslayer.GatheringRegistration", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("GatheringId")
                        .HasColumnType("int");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("datetime2");

                    b.HasKey("UserId", "GatheringId");

                    b.HasIndex("GatheringId");

                    b.ToTable("activitieregistration");
                });

            modelBuilder.Entity("Victuz.Models.Businesslayer.Location", b =>
                {
                    b.Property<int>("LocId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LocId"));

                    b.Property<string>("LocName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LocId");

                    b.ToTable("location");
                });

            modelBuilder.Entity("Victuz.Models.Businesslayer.Post", b =>
                {
                    b.Property<int>("PostId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PostId"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ForumId")
                        .HasColumnType("int");

                    b.Property<DateTime>("PostedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("PostId");

                    b.HasIndex("ForumId");

                    b.HasIndex("UserId");

                    b.ToTable("post");
                });

            modelBuilder.Entity("Victuz.Models.Businesslayer.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RoleId"));

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RoleId");

                    b.ToTable("role");
                });

            modelBuilder.Entity("Victuz.Models.Businesslayer.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.HasIndex("RoleId");

                    b.ToTable("users");
                });

            modelBuilder.Entity("Victuz.Models.Businesslayer.Gathering", b =>
                {
                    b.HasOne("Victuz.Models.Businesslayer.Category", "Category")
                        .WithMany("Gatherings")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Victuz.Models.Businesslayer.Location", "Location")
                        .WithMany("Gatherings")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Location");
                });

            modelBuilder.Entity("Victuz.Models.Businesslayer.GatheringRegistration", b =>
                {
                    b.HasOne("Victuz.Models.Businesslayer.Gathering", "Gathering")
                        .WithMany("GatheringRegistrations")
                        .HasForeignKey("GatheringId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Victuz.Models.Businesslayer.User", "User")
                        .WithMany("GatheringRegistrations")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Gathering");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Victuz.Models.Businesslayer.Post", b =>
                {
                    b.HasOne("Victuz.Models.Businesslayer.Forum", "Forum")
                        .WithMany("Posts")
                        .HasForeignKey("ForumId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Victuz.Models.Businesslayer.User", "User")
                        .WithMany("Posts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Forum");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Victuz.Models.Businesslayer.User", b =>
                {
                    b.HasOne("Victuz.Models.Businesslayer.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Victuz.Models.Businesslayer.Category", b =>
                {
                    b.Navigation("Gatherings");
                });

            modelBuilder.Entity("Victuz.Models.Businesslayer.Forum", b =>
                {
                    b.Navigation("Posts");
                });

            modelBuilder.Entity("Victuz.Models.Businesslayer.Gathering", b =>
                {
                    b.Navigation("GatheringRegistrations");
                });

            modelBuilder.Entity("Victuz.Models.Businesslayer.Location", b =>
                {
                    b.Navigation("Gatherings");
                });

            modelBuilder.Entity("Victuz.Models.Businesslayer.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("Victuz.Models.Businesslayer.User", b =>
                {
                    b.Navigation("GatheringRegistrations");

                    b.Navigation("Posts");
                });
#pragma warning restore 612, 618
        }
    }
}
