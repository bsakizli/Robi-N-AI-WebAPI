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
    [Migration("20240327102328_FirmCodeAdded")]
    partial class FirmCodeAdded
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

                    b.Property<string>("trackingUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("updateDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("validityPeriod")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("RBN_CARGO_COMPANY_LIST");
                });

            modelBuilder.Entity("Robi_N_WebAPI.Utility.Tables.RBN_EMPTOR_AUTOCLOSEDTICKET", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AutoClosedId")
                        .HasColumnType("int");

                    b.Property<long>("TicketId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("closedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("RBN_EMPTOR_AUTOCLOSEDTICKET");
                });

            modelBuilder.Entity("Robi_N_WebAPI.Utility.Tables.RBN_EMPTOR_AUTOTICKETCLOSEDScheduler", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool?>("active")
                        .HasColumnType("bit");

                    b.Property<string>("cron")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("lastStartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("process")
                        .HasColumnType("int");

                    b.Property<DateTime?>("registerDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ticketQuery")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("RBN_EMPTOR_AUTOTICKETCLOSEDScheduler");
                });

            modelBuilder.Entity("Robi_N_WebAPI.Utility.Tables.RBN_EMPTOR_ClosedTicketHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<long>("TicketId")
                        .HasColumnType("bigint");

                    b.Property<string>("TicketIdDesc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("active")
                        .HasColumnType("bit");

                    b.Property<DateTime>("addDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("autoTicketId")
                        .HasColumnType("int");

                    b.Property<DateTime>("closedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("RBN_EMPTOR_ClosedTicketHistory");
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

                    b.Property<bool>("userRecord")
                        .HasColumnType("bit");

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

            modelBuilder.Entity("Robi_N_WebAPI.Utility.Tables.RBN_SGK_HealthReports", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("ABASTAR")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ABITTAR")
                        .HasColumnType("datetime2");

                    b.Property<string>("AD")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ARSIV")
                        .HasColumnType("int");

                    b.Property<DateTime>("BASHEKIMONAYTARIHI")
                        .HasColumnType("datetime2");

                    b.Property<long?>("BildirimId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("DOGUMONCBASTAR")
                        .HasColumnType("datetime2");

                    b.Property<int>("FirmCode")
                        .HasColumnType("int");

                    b.Property<DateTime>("ISBASKONTTAR")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ISKAZASITARIHI")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ISVERENEBILDIRILDIGITARIH")
                        .HasColumnType("datetime2");

                    b.Property<string>("ISYERIADI")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ISYERIKODU")
                        .HasColumnType("int");

                    b.Property<long>("MEDULARAPORID")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("OnaylamaTarihi")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("POLIKLINIKTAR")
                        .HasColumnType("datetime2");

                    b.Property<bool?>("Personel")
                        .HasColumnType("bit");

                    b.Property<DateTime>("RAPORBITTAR")
                        .HasColumnType("datetime2");

                    b.Property<int>("RAPORDURUMU")
                        .HasColumnType("int");

                    b.Property<int>("RAPORSIRANO")
                        .HasColumnType("int");

                    b.Property<string>("RAPORTAKIPNO")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SOYAD")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("TCKIMLIKNO")
                        .HasColumnType("bigint");

                    b.Property<string>("TESISADI")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("TESISKODU")
                        .HasColumnType("bigint");

                    b.Property<int>("VAKA")
                        .HasColumnType("int");

                    b.Property<string>("VAKAADI")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("YATRAPBASTAR")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("YATRAPBITTAR")
                        .HasColumnType("datetime2");

                    b.Property<bool?>("active")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("addDate")
                        .HasColumnType("datetime2");

                    b.Property<bool?>("mailSend")
                        .HasColumnType("bit");

                    b.Property<int?>("process")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("RBN_SGK_HealthReports");
                });

            modelBuilder.Entity("Robi_N_WebAPI.Utility.Tables.RBN_SGK_VisitingIntroductionInformation", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<bool?>("active")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("addDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("region")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("updateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("RBN_SGK_VisitingIntroductionInformation");
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

                    b.Property<bool>("whatsappSend")
                        .HasColumnType("bit");

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

            modelBuilder.Entity("Robi_N_WebAPI.Utility.Tables.RBN_WhatsAppMessageTemplate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("MessageBody")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SmsId")
                        .HasColumnType("int");

                    b.Property<bool>("active")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("add_date")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("update_date")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("RBN_WhatsAppMessageTemplate");
                });

            modelBuilder.Entity("Robi_N_WebAPI.Utility.Tables.RNB_IVR_WORKING_HOURS", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<bool?>("active")
                        .HasColumnType("bit");

                    b.Property<string>("csqname")
                        .HasColumnType("nvarchar(max)");

                    b.Property<TimeSpan?>("endhours")
                        .HasColumnType("time");

                    b.Property<TimeSpan?>("starthours")
                        .HasColumnType("time");

                    b.Property<bool?>("weekend")
                        .HasColumnType("bit");

                    b.HasKey("id");

                    b.ToTable("RNB_IVR_WORKING_HOURS");
                });
#pragma warning restore 612, 618
        }
    }
}
