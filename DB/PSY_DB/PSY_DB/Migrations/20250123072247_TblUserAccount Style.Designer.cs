﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PSY_DB;

#nullable disable

namespace PSY_DB.Migrations
{
    [DbContext(typeof(PsyDbContext))]
    [Migration("20250123072247_TblUserAccount Style")]
    partial class TblUserAccountStyle
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("PSY_DB.Tables.TblUserAccount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CharacterId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValueSql("20001")
                        .HasComment("캐릭터 아이디");

                    b.Property<DateTime?>("DeletedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Evolution")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValueSql("0")
                        .HasComment("업데이트 스택");

                    b.Property<string>("EyebrowStyle")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("longtext")
                        .HasDefaultValueSql("AnnoyedEyebrows");

                    b.Property<string>("EyesStyle")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("longtext")
                        .HasDefaultValueSql("Annoyed");

                    b.Property<int>("Gold")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValueSql("0");

                    b.Property<string>("HairStyle")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("longtext")
                        .HasDefaultValueSql("Afro")
                        .HasComment("디자인");

                    b.Property<string>("Nickname")
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("RegisterDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UserName")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("TblUserAccount", t =>
                        {
                            t.HasComment("User 계정 정보");
                        });
                });

            modelBuilder.Entity("PSY_DB.Tables.TblUserMission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("MissionId")
                        .HasColumnType("int");

                    b.Property<int>("MissionStatus")
                        .HasColumnType("int");

                    b.Property<int>("Param1")
                        .HasColumnType("int");

                    b.Property<int>("UserAccountId")
                        .HasColumnType("int")
                        .HasComment("TblUserAccount FK");

                    b.HasKey("Id");

                    b.HasIndex("UserAccountId");

                    b.ToTable("TblUserMission", t =>
                        {
                            t.HasComment("UserScore 정보");
                        });
                });

            modelBuilder.Entity("PSY_DB.Tables.TblUserScore", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("DeletedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Gold")
                        .HasColumnType("int");

                    b.Property<int>("History")
                        .HasColumnType("int");

                    b.Property<DateTime>("RegisterDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("UserAccountId")
                        .HasColumnType("int")
                        .HasComment("TblUserScore FK");

                    b.HasKey("Id");

                    b.HasIndex("UserAccountId");

                    b.ToTable("TblUserScore", t =>
                        {
                            t.HasComment("UserScore 정보");
                        });
                });

            modelBuilder.Entity("PSY_DB.Tables.TblUserMission", b =>
                {
                    b.HasOne("PSY_DB.Tables.TblUserAccount", "TblUserAccountKeyNavigation")
                        .WithMany("TblUserMissions")
                        .HasForeignKey("UserAccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TblUserAccountKeyNavigation");
                });

            modelBuilder.Entity("PSY_DB.Tables.TblUserScore", b =>
                {
                    b.HasOne("PSY_DB.Tables.TblUserAccount", "TblUserAccountKeyNavigation")
                        .WithMany("TblUserScores")
                        .HasForeignKey("UserAccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TblUserAccountKeyNavigation");
                });

            modelBuilder.Entity("PSY_DB.Tables.TblUserAccount", b =>
                {
                    b.Navigation("TblUserMissions");

                    b.Navigation("TblUserScores");
                });
#pragma warning restore 612, 618
        }
    }
}