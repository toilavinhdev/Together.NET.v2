﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Together.Persistence;

#nullable disable

namespace Together.Persistence.Migrations
{
    [DbContext(typeof(TogetherContext))]
    partial class TogetherContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Together.Domain.Aggregates.ForumAggregate.Forum", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnOrder(0);

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CreatedById")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("ModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("ModifiedById")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("SubId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnOrder(1);

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("SubId"));

                    b.HasKey("Id");

                    b.HasIndex("SubId")
                        .IsUnique();

                    b.ToTable("Forums");
                });

            modelBuilder.Entity("Together.Domain.Aggregates.PostAggregate.Post", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnOrder(0);

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CreatedById")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ForumId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("ModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("ModifiedById")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("PrefixId")
                        .HasColumnType("uuid");

                    b.Property<long>("SubId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnOrder(1);

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("SubId"));

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("TopicId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("ForumId");

                    b.HasIndex("PrefixId");

                    b.HasIndex("SubId")
                        .IsUnique();

                    b.HasIndex("TopicId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("Together.Domain.Aggregates.PostAggregate.PostVote", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnOrder(0);

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CreatedById")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("ModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("ModifiedById")
                        .HasColumnType("uuid");

                    b.Property<Guid>("PostId")
                        .HasColumnType("uuid");

                    b.Property<long>("SubId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnOrder(1);

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("SubId"));

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("PostId");

                    b.HasIndex("SubId")
                        .IsUnique();

                    b.ToTable("PostVotes");
                });

            modelBuilder.Entity("Together.Domain.Aggregates.PostAggregate.Prefix", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnOrder(0);

                    b.Property<string>("Background")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CreatedById")
                        .HasColumnType("uuid");

                    b.Property<string>("Foreground")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset?>("ModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("ModifiedById")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("SubId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnOrder(1);

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("SubId"));

                    b.HasKey("Id");

                    b.HasIndex("SubId")
                        .IsUnique();

                    b.ToTable("Prefixes");
                });

            modelBuilder.Entity("Together.Domain.Aggregates.ReplyAggregate.Reply", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnOrder(0);

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CreatedById")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("ModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("ModifiedById")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ParentId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("PostId")
                        .HasColumnType("uuid");

                    b.Property<long>("SubId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnOrder(1);

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("SubId"));

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("ParentId");

                    b.HasIndex("PostId");

                    b.HasIndex("SubId")
                        .IsUnique();

                    b.ToTable("Replies");
                });

            modelBuilder.Entity("Together.Domain.Aggregates.ReplyAggregate.ReplyVote", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnOrder(0);

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CreatedById")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("ModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("ModifiedById")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ReplyId")
                        .HasColumnType("uuid");

                    b.Property<long>("SubId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnOrder(1);

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("SubId"));

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("ReplyId");

                    b.HasIndex("SubId")
                        .IsUnique();

                    b.ToTable("ReplyVotes");
                });

            modelBuilder.Entity("Together.Domain.Aggregates.RoleAggregate.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnOrder(0);

                    b.Property<List<string>>("Claims")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CreatedById")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<bool>("IsDefault")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("ModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("ModifiedById")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("SubId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnOrder(1);

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("SubId"));

                    b.HasKey("Id");

                    b.HasIndex("SubId")
                        .IsUnique();

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Together.Domain.Aggregates.TopicAggregate.Topic", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnOrder(0);

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CreatedById")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ForumId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("ModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("ModifiedById")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("SubId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnOrder(1);

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("SubId"));

                    b.HasKey("Id");

                    b.HasIndex("ForumId");

                    b.HasIndex("SubId")
                        .IsUnique();

                    b.ToTable("Topics");
                });

            modelBuilder.Entity("Together.Domain.Aggregates.UserAggregate.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnOrder(0);

                    b.Property<string>("Avatar")
                        .HasColumnType("text");

                    b.Property<string>("Biography")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FullName")
                        .HasColumnType("text");

                    b.Property<int>("Gender")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset?>("ModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<long>("SubId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnOrder(1);

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("SubId"));

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("SubId")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Together.Domain.Aggregates.UserAggregate.UserRole", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("Together.Domain.Aggregates.PostAggregate.Post", b =>
                {
                    b.HasOne("Together.Domain.Aggregates.UserAggregate.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Together.Domain.Aggregates.ForumAggregate.Forum", "Forum")
                        .WithMany("Posts")
                        .HasForeignKey("ForumId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Together.Domain.Aggregates.PostAggregate.Prefix", "Prefix")
                        .WithMany("Posts")
                        .HasForeignKey("PrefixId");

                    b.HasOne("Together.Domain.Aggregates.TopicAggregate.Topic", "Topic")
                        .WithMany("Posts")
                        .HasForeignKey("TopicId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CreatedBy");

                    b.Navigation("Forum");

                    b.Navigation("Prefix");

                    b.Navigation("Topic");
                });

            modelBuilder.Entity("Together.Domain.Aggregates.PostAggregate.PostVote", b =>
                {
                    b.HasOne("Together.Domain.Aggregates.UserAggregate.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Together.Domain.Aggregates.PostAggregate.Post", "Post")
                        .WithMany("PostVotes")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CreatedBy");

                    b.Navigation("Post");
                });

            modelBuilder.Entity("Together.Domain.Aggregates.ReplyAggregate.Reply", b =>
                {
                    b.HasOne("Together.Domain.Aggregates.UserAggregate.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Together.Domain.Aggregates.ReplyAggregate.Reply", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId");

                    b.HasOne("Together.Domain.Aggregates.PostAggregate.Post", "Post")
                        .WithMany("Replies")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CreatedBy");

                    b.Navigation("Parent");

                    b.Navigation("Post");
                });

            modelBuilder.Entity("Together.Domain.Aggregates.ReplyAggregate.ReplyVote", b =>
                {
                    b.HasOne("Together.Domain.Aggregates.UserAggregate.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Together.Domain.Aggregates.ReplyAggregate.Reply", "Reply")
                        .WithMany("ReplyVotes")
                        .HasForeignKey("ReplyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CreatedBy");

                    b.Navigation("Reply");
                });

            modelBuilder.Entity("Together.Domain.Aggregates.TopicAggregate.Topic", b =>
                {
                    b.HasOne("Together.Domain.Aggregates.ForumAggregate.Forum", "Forum")
                        .WithMany("Topics")
                        .HasForeignKey("ForumId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Forum");
                });

            modelBuilder.Entity("Together.Domain.Aggregates.UserAggregate.UserRole", b =>
                {
                    b.HasOne("Together.Domain.Aggregates.RoleAggregate.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Together.Domain.Aggregates.UserAggregate.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Together.Domain.Aggregates.ForumAggregate.Forum", b =>
                {
                    b.Navigation("Posts");

                    b.Navigation("Topics");
                });

            modelBuilder.Entity("Together.Domain.Aggregates.PostAggregate.Post", b =>
                {
                    b.Navigation("PostVotes");

                    b.Navigation("Replies");
                });

            modelBuilder.Entity("Together.Domain.Aggregates.PostAggregate.Prefix", b =>
                {
                    b.Navigation("Posts");
                });

            modelBuilder.Entity("Together.Domain.Aggregates.ReplyAggregate.Reply", b =>
                {
                    b.Navigation("Children");

                    b.Navigation("ReplyVotes");
                });

            modelBuilder.Entity("Together.Domain.Aggregates.RoleAggregate.Role", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("Together.Domain.Aggregates.TopicAggregate.Topic", b =>
                {
                    b.Navigation("Posts");
                });

            modelBuilder.Entity("Together.Domain.Aggregates.UserAggregate.User", b =>
                {
                    b.Navigation("UserRoles");
                });
#pragma warning restore 612, 618
        }
    }
}
