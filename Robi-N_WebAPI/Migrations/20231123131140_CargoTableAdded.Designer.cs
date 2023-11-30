﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Robi_N_WebAPI.Utility;

#nullable disable

namespace Robi_N_WebAPI.Migrations
{
    [DbContext(typeof(AIServiceDbContext))]
    [Migration("20231123131140_CargoTableAdded")]
    partial class CargoTableAdded
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Robi_N_WebAPI.Model.ApiUsers", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool?>("active")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("add_date")
                        .HasColumnType("datetime2");

                    b.Property<int?>("expirationTime")
                        .HasColumnType("int");

                    b.Property<string>("password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("role")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("salt")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("update_date")
                        .HasColumnType("datetime2");

                    b.Property<string>("username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("RBN_AI_SERVICE_USERS");
                });

            modelBuilder.Entity("Robi_N_WebAPI.Utility.Tables.RBN_AI_SERVICE_ROLE", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("RoleName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("active")
                        .HasColumnType("bit");

                    b.Property<DateTime>("add_date")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("update_date")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("RBN_AI_SERVICE_ROLE");
                });

            modelBuilder.Entity("Robi_N_WebAPI.Utility.Tables.RBN_AI_SERVICE_ROLES_MAP", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<bool>("active")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("add_date")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("update_date")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("RBN_AI_SERVICE_ROLES_MAP");
                });

            modelBuilder.Entity("Robi_N_WebAPI.Utility.Tables.RBN_CARGO_COMPANY_LIST", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("active")
                        .HasColumnType("bit");

                    b.Property<DateTime>("addDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("cargoName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("updateDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("RBN_CARGO_COMPANY_LIST");
                });

            modelBuilder.Entity("Robi_N_WebAPI.Utility.Tables.RBN_EMPTOR_WaitingTicketHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool?>("Active")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("CallBackDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("MainUserId")
                        .HasColumnType("int");

                    b.Property<int>("TicketId")
                        .HasColumnType("int");

                    b.Property<string>("TicketIdDesc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("TransactionDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("WaitingReasonId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("RBN_EMPTOR_WaitingTicketHistory");
                });

            modelBuilder.Entity("Robi_N_WebAPI.Utility.Tables.RBN_IVR_HOLIDAY_DAYS", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("active")
                        .HasColumnType("bit");

                    b.Property<DateTime>("addDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("csq")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("displayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("endDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("holidayDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("holidayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("startDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("updateDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("years")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("RBN_IVR_HOLIDAY_DAYS");
                });

            modelBuilder.Entity("Robi_N_WebAPI.Utility.Tables.RBN_IVR_LOGS", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("active")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("addDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("log")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("logKey")
                        .HasColumnType("int");

                    b.Property<string>("uniqId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Id", "uniqId")
                        .IsUnique()
                        .HasFilter("[uniqId] IS NOT NULL");

                    b.ToTable("RBN_IVR_LOGS");
                });

            modelBuilder.Entity("Robi_N_WebAPI.Utility.Tables.RBN_SMS_TEMPLATES", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("active")
                        .HasColumnType("bit");

                    b.Property<DateTime>("addDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("updateDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("RBN_SMS_TEMPLATES");
                });

            modelBuilder.Entity("Robi_N_WebAPI.Utility.Tables.RBN_VOICE_SOUNDS", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("active")
                        .HasColumnType("bit");

                    b.Property<DateTime>("addDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("fileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("platform")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("soundBase64Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("soundContent")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("text")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("RBN_VOICE_SOUNDS");
                });

            modelBuilder.Entity("Robi_N_WebAPI.Utility.Tables.RBN_WAITING_TIMES", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<int>("EmptorTicketWaitingReasonId")
                        .HasColumnType("int");

                    b.Property<bool>("Overtime")
                        .HasColumnType("bit");

                    b.Property<int>("WaitingTimeDay")
                        .HasColumnType("int");

                    b.Property<DateTime>("add_date")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("update_date")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("RBN_WAITING_TIMES");
                });
#pragma warning restore 612, 618
        }
    }
}
