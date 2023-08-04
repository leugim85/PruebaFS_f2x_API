﻿// <auto-generated />
using System;
using DBContextF2xF2xFullStackAssesment.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace F2x.FullStackAssesment.Domain.Migrations
{
    [DbContext(typeof(DBContextF2xFullStackAssesment))]
    [Migration("20230804161839_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("AndresUrrego")
                .HasAnnotation("ProductVersion", "7.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("F2x.FullStackAssesment.Domain.Entities.VehicleCounterQueryHistory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2")
                        .HasColumnName("dtDate");

                    b.Property<int>("Quantity")
                        .HasColumnType("int")
                        .HasColumnName("intRegisters");

                    b.HasKey("Id");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("Id"));

                    b.ToTable("TblVehicleCounterQueryHistory", "AndresUrrego");
                });

            modelBuilder.Entity("F2x.FullStackAssesment.Domain.Entities.VehicleCounterWithAmount", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("Amount")
                        .HasColumnType("float")
                        .HasColumnName("dblAmount");

                    b.Property<string>("Category")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("strCategory");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2")
                        .HasColumnName("dtDate");

                    b.Property<string>("Direction")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("strDirection");

                    b.Property<string>("Hour")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("tHour");

                    b.Property<string>("Station")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("strStation");

                    b.HasKey("Id");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("Id"));

                    b.ToTable("TblVehicleCounterWithAmount", "AndresUrrego");
                });

            modelBuilder.Entity("F2xF2xFullStackAssesment.Domain.Entities.VehicleCounterInformation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Category")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("strCategory");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2")
                        .HasColumnName("dtDate");

                    b.Property<string>("Direction")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("strDirection");

                    b.Property<string>("Hour")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("tHour");

                    b.Property<int>("Quantity")
                        .HasColumnType("int")
                        .HasColumnName("intQuantity");

                    b.Property<string>("Station")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("strStation");

                    b.HasKey("Id");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("Id"));

                    b.ToTable("TblVehicleCount", "AndresUrrego");
                });
#pragma warning restore 612, 618
        }
    }
}
