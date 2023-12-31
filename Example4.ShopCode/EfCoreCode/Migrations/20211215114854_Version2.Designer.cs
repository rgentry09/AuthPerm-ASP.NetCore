﻿// <auto-generated />
using Example4.ShopCode.EfCoreCode;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Example4.ShopCode.EfCoreCode.Migrations
{
    [DbContext(typeof(RetailDbContext))]
    [Migration("20211215114854_Version2")]
    partial class Version2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("retail")
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Example4.ShopCode.EfCoreClasses.RetailOutlet", b =>
                {
                    b.Property<int>("RetailOutletId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RetailOutletId"), 1L, 1);

                    b.Property<int>("AuthPTenantId")
                        .HasColumnType("int");

                    b.Property<string>("DataKey")
                        .HasMaxLength(250)
                        .IsUnicode(false)
                        .HasColumnType("varchar(250)");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ShortName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RetailOutletId");

                    b.HasIndex("DataKey");

                    b.HasIndex("FullName")
                        .IsUnique()
                        .HasFilter("[FullName] IS NOT NULL");

                    b.ToTable("RetailOutlets", "retail");
                });

            modelBuilder.Entity("Example4.ShopCode.EfCoreClasses.ShopSale", b =>
                {
                    b.Property<int>("ShopSaleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ShopSaleId"), 1L, 1);

                    b.Property<string>("DataKey")
                        .HasMaxLength(250)
                        .IsUnicode(false)
                        .HasColumnType("varchar(250)");

                    b.Property<int>("NumSoldReturned")
                        .HasColumnType("int");

                    b.Property<string>("ReturnReason")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ShopStockId")
                        .HasColumnType("int");

                    b.HasKey("ShopSaleId");

                    b.HasIndex("DataKey");

                    b.HasIndex("ShopStockId");

                    b.ToTable("ShopSales", "retail");
                });

            modelBuilder.Entity("Example4.ShopCode.EfCoreClasses.ShopStock", b =>
                {
                    b.Property<int>("ShopStockId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ShopStockId"), 1L, 1);

                    b.Property<string>("DataKey")
                        .HasMaxLength(250)
                        .IsUnicode(false)
                        .HasColumnType("varchar(250)");

                    b.Property<int>("NumInStock")
                        .HasColumnType("int");

                    b.Property<decimal>("RetailPrice")
                        .HasPrecision(9, 2)
                        .HasColumnType("decimal(9,2)");

                    b.Property<string>("StockName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TenantItemId")
                        .HasColumnType("int");

                    b.HasKey("ShopStockId");

                    b.HasIndex("DataKey");

                    b.HasIndex("TenantItemId");

                    b.ToTable("ShopStocks", "retail");
                });

            modelBuilder.Entity("Example4.ShopCode.EfCoreClasses.ShopSale", b =>
                {
                    b.HasOne("Example4.ShopCode.EfCoreClasses.ShopStock", "StockItem")
                        .WithMany()
                        .HasForeignKey("ShopStockId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("StockItem");
                });

            modelBuilder.Entity("Example4.ShopCode.EfCoreClasses.ShopStock", b =>
                {
                    b.HasOne("Example4.ShopCode.EfCoreClasses.RetailOutlet", "Shop")
                        .WithMany()
                        .HasForeignKey("TenantItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Shop");
                });
#pragma warning restore 612, 618
        }
    }
}
