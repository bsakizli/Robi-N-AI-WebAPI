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
    [Migration("20230831131113_IVRTable")]
    partial class IVRTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
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

            modelBuilder.Entity("Robi_N_WebAPI.Utility.Tables.IVR_HOLIDAY_DAYS", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("active")
                        .HasColumnType("bit");

                    b.Property<DateTime>("addDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("displayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("endDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("holidayDate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("holidayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("startDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("updateDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("IVR_HOLIDAY_DAYS");
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
#pragma warning restore 612, 618
        }
    }
}
