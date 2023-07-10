﻿// <auto-generated />
using System;
using LIMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LIMS.Infrastructure.Migrations
{
    [DbContext(typeof(LimsContext))]
    [Migration("20230710094342_Update_Records_ForPlaybacks")]
    partial class Update_Records_ForPlaybacks
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("LIMS.Domain.Entities.Meeting", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("AttendeePassword")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("EndDateTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsRecord")
                        .HasColumnType("bit");

                    b.Property<bool>("IsRunning")
                        .HasColumnType("bit");

                    b.Property<int>("LimitCapacity")
                        .HasColumnType("int");

                    b.Property<string>("MeetingId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ModeratorPassword")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ServerId")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDateTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ServerId");

                    b.ToTable("Meetings");
                });

            modelBuilder.Entity("LIMS.Domain.Entities.MemberShip", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("JoinedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<long>("MeetingId")
                        .HasColumnType("bigint");

                    b.Property<bool>("UserExited")
                        .HasColumnType("bit");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<bool>("UserRejected")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("MeetingId");

                    b.HasIndex("UserId");

                    b.ToTable("MemberShips");
                });

            modelBuilder.Entity("LIMS.Domain.Entities.Playback", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("Length")
                        .HasColumnType("bigint");

                    b.Property<long>("RecordId")
                        .HasColumnType("bigint");

                    b.Property<long>("Size")
                        .HasColumnType("bigint");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("RecordId");

                    b.ToTable("Playback");
                });

            modelBuilder.Entity("LIMS.Domain.Entities.Record", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("EndDateTime")
                        .HasColumnType("datetime2");

                    b.Property<long>("MeetingId")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Published")
                        .HasColumnType("bit");

                    b.Property<DateTime>("StartDataTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MeetingId");

                    b.ToTable("Record");
                });

            modelBuilder.Entity("LIMS.Domain.Entities.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Alias")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("RoleId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("LIMS.Domain.Entities.UserRole", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("LIMS.Domain.Entity.Server", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int>("ServerLimit")
                        .HasColumnType("int");

                    b.Property<string>("ServerUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SharedSecret")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Servers");
                });

            modelBuilder.Entity("MeetingUser", b =>
                {
                    b.Property<long>("MeetingId")
                        .HasColumnType("bigint");

                    b.Property<long>("UsersId")
                        .HasColumnType("bigint");

                    b.HasKey("MeetingId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("MeetingUser");
                });

            modelBuilder.Entity("LIMS.Domain.Entities.Meeting", b =>
                {
                    b.HasOne("LIMS.Domain.Entity.Server", "Server")
                        .WithMany("Meetings")
                        .HasForeignKey("ServerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Server");
                });

            modelBuilder.Entity("LIMS.Domain.Entities.MemberShip", b =>
                {
                    b.HasOne("LIMS.Domain.Entities.Meeting", "Meeting")
                        .WithMany("MemberShips")
                        .HasForeignKey("MeetingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LIMS.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Meeting");

                    b.Navigation("User");
                });

            modelBuilder.Entity("LIMS.Domain.Entities.Playback", b =>
                {
                    b.HasOne("LIMS.Domain.Entities.Record", "Record")
                        .WithMany("Playbacks")
                        .HasForeignKey("RecordId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Record");
                });

            modelBuilder.Entity("LIMS.Domain.Entities.Record", b =>
                {
                    b.HasOne("LIMS.Domain.Entities.Meeting", "Meeting")
                        .WithMany("Records")
                        .HasForeignKey("MeetingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Meeting");
                });

            modelBuilder.Entity("LIMS.Domain.Entities.User", b =>
                {
                    b.HasOne("LIMS.Domain.Entities.UserRole", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("MeetingUser", b =>
                {
                    b.HasOne("LIMS.Domain.Entities.Meeting", null)
                        .WithMany()
                        .HasForeignKey("MeetingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LIMS.Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("LIMS.Domain.Entities.Meeting", b =>
                {
                    b.Navigation("MemberShips");

                    b.Navigation("Records");
                });

            modelBuilder.Entity("LIMS.Domain.Entities.Record", b =>
                {
                    b.Navigation("Playbacks");
                });

            modelBuilder.Entity("LIMS.Domain.Entities.UserRole", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("LIMS.Domain.Entity.Server", b =>
                {
                    b.Navigation("Meetings");
                });
#pragma warning restore 612, 618
        }
    }
}
