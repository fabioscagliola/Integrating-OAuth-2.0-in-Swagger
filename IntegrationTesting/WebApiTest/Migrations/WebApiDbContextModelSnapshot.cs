﻿// <auto-generated />
using com.fabioscagliola.IntegrationTesting.WebApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

#nullable disable

namespace com.fabioscagliola.IntegrationTesting.WebApiTest.Migrations
{
    [DbContext(typeof(WebApiDbContext))]
    partial class WebApiDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.0");

            modelBuilder.Entity("com.fabioscagliola.IntegrationTesting.WebApi.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("FName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("LName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Person", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
