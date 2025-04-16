﻿// <auto-generated />
using System;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(LeaveManagementContext))]
    partial class LeaveManagementContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.0");

            modelBuilder.Entity("Domain.Entities.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Department")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("JoiningDate")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Employees");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Department = "Informatique",
                            FullName = "Aziz Ben Hmida",
                            JoiningDate = new DateTime(2020, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 2,
                            Department = "Comptabilité",
                            FullName = "Ahmed Gharbi",
                            JoiningDate = new DateTime(2019, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 3,
                            Department = "Ressources Humaines",
                            FullName = "Ons Trabelsi",
                            JoiningDate = new DateTime(2021, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("Domain.Entities.LeaveRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("LeaveType")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.ToTable("LeaveRequests");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedAt = new DateTime(2023, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EmployeeId = 1,
                            EndDate = new DateTime(2023, 7, 5, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LeaveType = 0,
                            Reason = "Vacances à Hammamet",
                            StartDate = new DateTime(2023, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Status = 1
                        },
                        new
                        {
                            Id = 2,
                            CreatedAt = new DateTime(2023, 9, 10, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EmployeeId = 1,
                            EndDate = new DateTime(2023, 9, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LeaveType = 1,
                            Reason = "Grippe saisonnière",
                            StartDate = new DateTime(2023, 9, 12, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Status = 1
                        },
                        new
                        {
                            Id = 3,
                            CreatedAt = new DateTime(2023, 7, 20, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EmployeeId = 2,
                            EndDate = new DateTime(2023, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LeaveType = 0,
                            Reason = "Voyage longue durée",
                            StartDate = new DateTime(2023, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Status = 2
                        },
                        new
                        {
                            Id = 4,
                            CreatedAt = new DateTime(2023, 10, 18, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EmployeeId = 2,
                            EndDate = new DateTime(2023, 10, 21, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LeaveType = 2,
                            Reason = "Urgence familiale",
                            StartDate = new DateTime(2023, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Status = 0
                        },
                        new
                        {
                            Id = 5,
                            CreatedAt = new DateTime(2023, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EmployeeId = 3,
                            EndDate = new DateTime(2023, 5, 4, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LeaveType = 1,
                            Reason = "Extraction dentaire",
                            StartDate = new DateTime(2023, 5, 3, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Status = 1
                        });
                });

            modelBuilder.Entity("Domain.Entities.LeaveRequest", b =>
                {
                    b.HasOne("Domain.Entities.Employee", "Employee")
                        .WithMany("LeaveRequests")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("Domain.Entities.Employee", b =>
                {
                    b.Navigation("LeaveRequests");
                });
#pragma warning restore 612, 618
        }
    }
}
