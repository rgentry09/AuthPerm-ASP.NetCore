﻿// <auto-generated />
using System;
using Example3.InvoiceCode.EfCoreCode;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Example3.InvoiceCode.EfCoreCode.Migrations
{
    [DbContext(typeof(InvoicesDbContext))]
    partial class InvoicesDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("invoice")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.9")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Example3.InvoiceCode.EfCoreClasses.CompanyTenant", b =>
                {
                    b.Property<int>("CompanyTenantId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AuthPTenantId")
                        .HasColumnType("int");

                    b.Property<string>("CompanyName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DataKey")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("CompanyTenantId");

                    b.HasIndex("DataKey");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("Example3.InvoiceCode.EfCoreClasses.Invoice", b =>
                {
                    b.Property<int>("InvoiceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DataKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<string>("InvoiceName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("InvoiceId");

                    b.HasIndex("DataKey");

                    b.ToTable("Invoices");
                });

            modelBuilder.Entity("Example3.InvoiceCode.EfCoreClasses.LineItem", b =>
                {
                    b.Property<int>("LineItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DataKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("InvoiceId")
                        .HasColumnType("int");

                    b.Property<string>("ItemName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NumberItems")
                        .HasColumnType("int");

                    b.Property<decimal>("TotalPrice")
                        .HasPrecision(9, 2)
                        .HasColumnType("decimal(9,2)");

                    b.HasKey("LineItemId");

                    b.HasIndex("DataKey");

                    b.HasIndex("InvoiceId");

                    b.ToTable("LineItems");
                });

            modelBuilder.Entity("Example3.InvoiceCode.EfCoreClasses.LineItem", b =>
                {
                    b.HasOne("Example3.InvoiceCode.EfCoreClasses.Invoice", null)
                        .WithMany("LineItems")
                        .HasForeignKey("InvoiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Example3.InvoiceCode.EfCoreClasses.Invoice", b =>
                {
                    b.Navigation("LineItems");
                });
#pragma warning restore 612, 618
        }
    }
}
