﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyQueue.Data;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace MyQueue.Migrations
{
    [DbContext(typeof(MQDBContext))]
    [Migration("20221022132815_OrderAndCategory2")]
    partial class OrderAndCategory2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("MyQueue.Data.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("MyQueue.Data.Models.Foods", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("CategoryId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Foods");
                });

            modelBuilder.Entity("MyQueue.Data.Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("FoodsId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("FoodsId");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("MyQueue.Data.Models.Foods", b =>
                {
                    b.HasOne("MyQueue.Data.Models.Category", "Category")
                        .WithMany("Foods")
                        .HasForeignKey("CategoryId");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("MyQueue.Data.Models.Order", b =>
                {
                    b.HasOne("MyQueue.Data.Models.Foods", "Foods")
                        .WithMany("Orders")
                        .HasForeignKey("FoodsId");

                    b.Navigation("Foods");
                });

            modelBuilder.Entity("MyQueue.Data.Models.Category", b =>
                {
                    b.Navigation("Foods");
                });

            modelBuilder.Entity("MyQueue.Data.Models.Foods", b =>
                {
                    b.Navigation("Orders");
                });
#pragma warning restore 612, 618
        }
    }
}
