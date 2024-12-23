﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WeatherApp.API.Data;

#nullable disable

namespace WeatherApp.API.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20241117213424_AddIsActiveToUsers")]
    partial class AddIsActiveToUsers
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WeatherApp.Shared.Models.Alert", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CityId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.ToTable("Alerts");
                });

            modelBuilder.Entity("WeatherApp.Shared.Models.City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("WeatherApp.Shared.Models.CloudCoverage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Percentage")
                        .HasColumnType("int");

                    b.Property<int>("WeatherInfoId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("WeatherInfoId")
                        .IsUnique();

                    b.ToTable("CloudCoverages");
                });

            modelBuilder.Entity("WeatherApp.Shared.Models.Coordinates", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CityId")
                        .HasColumnType("int");

                    b.Property<double>("Latitude")
                        .HasColumnType("float");

                    b.Property<double>("Longitude")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("CityId")
                        .IsUnique();

                    b.ToTable("Coordinates");
                });

            modelBuilder.Entity("WeatherApp.Shared.Models.Forecast", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CityId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<double>("Humidity")
                        .HasColumnType("float");

                    b.Property<double>("Temperature")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.ToTable("Forecasts");
                });

            modelBuilder.Entity("WeatherApp.Shared.Models.SunInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Sunrise")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Sunset")
                        .HasColumnType("datetime2");

                    b.Property<int>("WeatherInfoId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("WeatherInfoId")
                        .IsUnique();

                    b.ToTable("SunInfos");
                });

            modelBuilder.Entity("WeatherApp.Shared.Models.TemperatureDetails", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("Max")
                        .HasColumnType("float");

                    b.Property<double>("Min")
                        .HasColumnType("float");

                    b.Property<int>("WeatherInfoId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("WeatherInfoId")
                        .IsUnique();

                    b.ToTable("TemperatureDetails");
                });

            modelBuilder.Entity("WeatherApp.Shared.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CityId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("WeatherApp.Shared.Models.WeatherInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CityId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<double>("Humidity")
                        .HasColumnType("float");

                    b.Property<double>("Pressure")
                        .HasColumnType("float");

                    b.Property<double>("Temperature")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.ToTable("WeatherInfos");
                });

            modelBuilder.Entity("WeatherApp.Shared.Models.WindInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("Direction")
                        .HasColumnType("float");

                    b.Property<double>("Speed")
                        .HasColumnType("float");

                    b.Property<int>("WeatherInfoId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("WeatherInfoId")
                        .IsUnique();

                    b.ToTable("WindInfos");
                });

            modelBuilder.Entity("WeatherApp.Shared.Models.Alert", b =>
                {
                    b.HasOne("WeatherApp.Shared.Models.City", "City")
                        .WithMany("Alerts")
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("City");
                });

            modelBuilder.Entity("WeatherApp.Shared.Models.CloudCoverage", b =>
                {
                    b.HasOne("WeatherApp.Shared.Models.WeatherInfo", "WeatherInfo")
                        .WithOne("CloudCoverage")
                        .HasForeignKey("WeatherApp.Shared.Models.CloudCoverage", "WeatherInfoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WeatherInfo");
                });

            modelBuilder.Entity("WeatherApp.Shared.Models.Coordinates", b =>
                {
                    b.HasOne("WeatherApp.Shared.Models.City", "City")
                        .WithOne("Coordinates")
                        .HasForeignKey("WeatherApp.Shared.Models.Coordinates", "CityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("City");
                });

            modelBuilder.Entity("WeatherApp.Shared.Models.Forecast", b =>
                {
                    b.HasOne("WeatherApp.Shared.Models.City", "City")
                        .WithMany("Forecasts")
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("City");
                });

            modelBuilder.Entity("WeatherApp.Shared.Models.SunInfo", b =>
                {
                    b.HasOne("WeatherApp.Shared.Models.WeatherInfo", "WeatherInfo")
                        .WithOne("SunInfo")
                        .HasForeignKey("WeatherApp.Shared.Models.SunInfo", "WeatherInfoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WeatherInfo");
                });

            modelBuilder.Entity("WeatherApp.Shared.Models.TemperatureDetails", b =>
                {
                    b.HasOne("WeatherApp.Shared.Models.WeatherInfo", "WeatherInfo")
                        .WithOne("TemperatureDetails")
                        .HasForeignKey("WeatherApp.Shared.Models.TemperatureDetails", "WeatherInfoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WeatherInfo");
                });

            modelBuilder.Entity("WeatherApp.Shared.Models.User", b =>
                {
                    b.HasOne("WeatherApp.Shared.Models.City", "City")
                        .WithMany("Users")
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("City");
                });

            modelBuilder.Entity("WeatherApp.Shared.Models.WeatherInfo", b =>
                {
                    b.HasOne("WeatherApp.Shared.Models.City", "City")
                        .WithMany("WeatherHistory")
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("City");
                });

            modelBuilder.Entity("WeatherApp.Shared.Models.WindInfo", b =>
                {
                    b.HasOne("WeatherApp.Shared.Models.WeatherInfo", "WeatherInfo")
                        .WithOne("Wind")
                        .HasForeignKey("WeatherApp.Shared.Models.WindInfo", "WeatherInfoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WeatherInfo");
                });

            modelBuilder.Entity("WeatherApp.Shared.Models.City", b =>
                {
                    b.Navigation("Alerts");

                    b.Navigation("Coordinates")
                        .IsRequired();

                    b.Navigation("Forecasts");

                    b.Navigation("Users");

                    b.Navigation("WeatherHistory");
                });

            modelBuilder.Entity("WeatherApp.Shared.Models.WeatherInfo", b =>
                {
                    b.Navigation("CloudCoverage")
                        .IsRequired();

                    b.Navigation("SunInfo")
                        .IsRequired();

                    b.Navigation("TemperatureDetails")
                        .IsRequired();

                    b.Navigation("Wind")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
