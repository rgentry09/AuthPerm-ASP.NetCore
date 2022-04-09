﻿// <auto-generated />
using System;
using AuthPermissions.DataLayer.EfCode;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AuthPermissions.DataLayer.Migrations
{
    [DbContext(typeof(AuthPermissionsDbContext))]
    [Migration("20220323172428_Version3")]
    partial class Version3
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("authp")
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("AuthPermissions.DataLayer.Classes.AuthUser", b =>
                {
                    b.Property<string>("UserId")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<byte[]>("ConcurrencyToken")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("ROWVERSION");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("IsDisabled")
                        .HasColumnType("bit");

                    b.Property<int?>("TenantId")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.HasKey("UserId");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasFilter("[Email] IS NOT NULL");

                    b.HasIndex("TenantId");

                    b.HasIndex("UserName")
                        .IsUnique()
                        .HasFilter("[UserName] IS NOT NULL");

                    b.ToTable("AuthUsers", "authp");
                });

            modelBuilder.Entity("AuthPermissions.DataLayer.Classes.RefreshToken", b =>
                {
                    b.Property<string>("TokenValue")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("AddedDateUtc")
                        .HasColumnType("datetime2");

                    b.Property<byte[]>("ConcurrencyToken")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("ROWVERSION");

                    b.Property<bool>("IsInvalid")
                        .HasColumnType("bit");

                    b.Property<string>("JwtId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("TokenValue");

                    b.HasIndex("AddedDateUtc")
                        .IsUnique();

                    b.ToTable("RefreshTokens", "authp");
                });

            modelBuilder.Entity("AuthPermissions.DataLayer.Classes.RoleToPermissions", b =>
                {
                    b.Property<string>("RoleName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<byte[]>("ConcurrencyToken")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("ROWVERSION");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PackedPermissionsInRole")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte>("RoleType")
                        .HasColumnType("tinyint");

                    b.HasKey("RoleName");

                    b.HasIndex("RoleType");

                    b.ToTable("RoleToPermissions", "authp");
                });

            modelBuilder.Entity("AuthPermissions.DataLayer.Classes.Tenant", b =>
                {
                    b.Property<int>("TenantId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TenantId"), 1L, 1);

                    b.Property<byte[]>("ConcurrencyToken")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("ROWVERSION");

                    b.Property<string>("ConnectionName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("HasOwnDb")
                        .HasColumnType("bit");

                    b.Property<bool>("IsHierarchical")
                        .HasColumnType("bit");

                    b.Property<string>("ParentDataKey")
                        .HasMaxLength(250)
                        .IsUnicode(false)
                        .HasColumnType("varchar(250)");

                    b.Property<int?>("ParentTenantId")
                        .HasColumnType("int");

                    b.Property<string>("TenantFullName")
                        .IsRequired()
                        .HasMaxLength(400)
                        .HasColumnType("nvarchar(400)");

                    b.HasKey("TenantId");

                    b.HasIndex("ParentDataKey");

                    b.HasIndex("ParentTenantId");

                    b.HasIndex("TenantFullName")
                        .IsUnique();

                    b.ToTable("Tenants", "authp");
                });

            modelBuilder.Entity("AuthPermissions.DataLayer.Classes.UserToRole", b =>
                {
                    b.Property<string>("UserId")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("RoleName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<byte[]>("ConcurrencyToken")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("ROWVERSION");

                    b.HasKey("UserId", "RoleName");

                    b.HasIndex("RoleName");

                    b.ToTable("UserToRoles", "authp");
                });

            modelBuilder.Entity("RoleToPermissionsTenant", b =>
                {
                    b.Property<string>("TenantRolesRoleName")
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("TenantsTenantId")
                        .HasColumnType("int");

                    b.Property<byte[]>("ConcurrencyToken")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("ROWVERSION");

                    b.HasKey("TenantRolesRoleName", "TenantsTenantId");

                    b.HasIndex("TenantsTenantId");

                    b.ToTable("RoleToPermissionsTenant", "authp");
                });

            modelBuilder.Entity("AuthPermissions.DataLayer.Classes.AuthUser", b =>
                {
                    b.HasOne("AuthPermissions.DataLayer.Classes.Tenant", "UserTenant")
                        .WithMany()
                        .HasForeignKey("TenantId");

                    b.Navigation("UserTenant");
                });

            modelBuilder.Entity("AuthPermissions.DataLayer.Classes.Tenant", b =>
                {
                    b.HasOne("AuthPermissions.DataLayer.Classes.Tenant", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentTenantId");

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("AuthPermissions.DataLayer.Classes.UserToRole", b =>
                {
                    b.HasOne("AuthPermissions.DataLayer.Classes.RoleToPermissions", "Role")
                        .WithMany()
                        .HasForeignKey("RoleName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AuthPermissions.DataLayer.Classes.AuthUser", null)
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("RoleToPermissionsTenant", b =>
                {
                    b.HasOne("AuthPermissions.DataLayer.Classes.RoleToPermissions", null)
                        .WithMany()
                        .HasForeignKey("TenantRolesRoleName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AuthPermissions.DataLayer.Classes.Tenant", null)
                        .WithMany()
                        .HasForeignKey("TenantsTenantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AuthPermissions.DataLayer.Classes.AuthUser", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("AuthPermissions.DataLayer.Classes.Tenant", b =>
                {
                    b.Navigation("Children");
                });
#pragma warning restore 612, 618
        }
    }
}
