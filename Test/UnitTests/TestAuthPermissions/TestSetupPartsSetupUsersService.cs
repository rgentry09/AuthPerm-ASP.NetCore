﻿// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading.Tasks;
using AuthPermissions;
using AuthPermissions.BulkLoadServices.Concrete;
using AuthPermissions.DataLayer.EfCode;
using AuthPermissions.SetupCode;
using Microsoft.EntityFrameworkCore;
using Test.TestHelpers;
using TestSupport.EfHelpers;
using Xunit;
using Xunit.Abstractions;
using Xunit.Extensions.AssertExtensions;

namespace Test.UnitTests.TestAuthPermissions
{
    public class TestSetupPartsSetupUsersRolesService
    {
        private readonly ITestOutputHelper _output;

        public TestSetupPartsSetupUsersRolesService(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task TestAddUserRolesToDatabaseIfEmpty()
        {
            //SETUP
            var options = SqliteInMemory.CreateOptions<AuthPermissionsDbContext>();
            using var context = new AuthPermissionsDbContext(options);
            context.Database.EnsureCreated();

            await context.SetupRolesInDbAsync();

            context.ChangeTracker.Clear();

            var service = new BulkLoadUsersService(context, new StubIFindUserInfoFactory(true), new AuthPermissionsOptions());

            //ATTEMPT
            var status = await service.AddUsersRolesToDatabaseAsync(
                SetupHelpers.TestUserDefineWithUserId());
            status.IsValid.ShouldBeTrue(status.GetAllErrors());
            context.SaveChanges();

            //VERIFY
            context.ChangeTracker.Clear();
            context.AuthUsers.Count().ShouldEqual(3);
            context.RoleToPermissions.Count().ShouldEqual(3);
            context.UserToRoles.Count().ShouldEqual(5);
            context.Tenants.Count().ShouldEqual(0);
        }

        [Fact]
        public async Task TestAddUserRolesToDatabaseIfEmptyWithIFindUserId()
        {
            //SETUP
            var options = SqliteInMemory.CreateOptions<AuthPermissionsDbContext>();
            using var context = new AuthPermissionsDbContext(options);
            context.Database.EnsureCreated();

            await context.SetupRolesInDbAsync();

            context.ChangeTracker.Clear();

            var service = new BulkLoadUsersService(context, new StubIFindUserInfoFactory(false), new AuthPermissionsOptions());

            //ATTEMPT
            var status = await service.AddUsersRolesToDatabaseAsync(
                SetupHelpers.TestUserDefineNoUserId());
            status.IsValid.ShouldBeTrue(status.GetAllErrors());
            context.SaveChanges();

            //VERIFY
            context.ChangeTracker.Clear();
            context.AuthUsers.Count().ShouldEqual(3);
            context.RoleToPermissions.Count().ShouldEqual(3);
            context.UserToRoles.Count().ShouldEqual(5);
            context.Tenants.Count().ShouldEqual(0);
        }

        [Fact]
        public async Task TestAddUserRolesToDatabaseIfEmptyNoUserIdFail()
        {
            //SETUP
            var options = SqliteInMemory.CreateOptions<AuthPermissionsDbContext>();
            using var context = new AuthPermissionsDbContext(options);
            context.Database.EnsureCreated();

            await context.SetupRolesInDbAsync();

            context.ChangeTracker.Clear();

            var service = new BulkLoadUsersService(context, new StubIFindUserInfoFactory(true), new AuthPermissionsOptions());

            //ATTEMPT
            var status = await service.AddUsersRolesToDatabaseAsync(SetupHelpers.TestUserDefineNoUserId(null));

            //VERIFY
            status.IsValid.ShouldBeFalse();
            status.Errors.Single().ToString().ShouldStartWith("Line/index 1: The user User2 didn't have a userId and the IFindUserInfoService wasn't available.");
        }

        [Fact]
        public async Task TestAddUserRolesToDatabaseIfEmptyNoRoleError()
        {
            //SETUP
            var options = SqliteInMemory.CreateOptions<AuthPermissionsDbContext>();
            using var context = new AuthPermissionsDbContext(options);
            context.Database.EnsureCreated();

            await context.SetupRolesInDbAsync();

            context.ChangeTracker.Clear();

            var service = new BulkLoadUsersService(context, new StubIFindUserInfoFactory(true), new AuthPermissionsOptions());

            //ATTEMPT
            var status = await service.AddUsersRolesToDatabaseAsync(
                SetupHelpers.TestUserDefineWithUserId(""));

            //VERIFY
            status.IsValid.ShouldBeFalse();
            status.GetAllErrors().ShouldStartWith("Line/index 1: The user User2 didn't have any roles.");
        }

        [Fact]
        public async Task TestAddUserRolesToDatabaseIfEmptyBadRole()
        {
            //SETUP
            var options = SqliteInMemory.CreateOptions<AuthPermissionsDbContext>();
            using var context = new AuthPermissionsDbContext(options);
            context.Database.EnsureCreated();

            await context.SetupRolesInDbAsync();

            context.ChangeTracker.Clear();

            var service = new BulkLoadUsersService(context, new StubIFindUserInfoFactory(true), new AuthPermissionsOptions());

            //ATTEMPT
            var status = await service.AddUsersRolesToDatabaseAsync(
                SetupHelpers.TestUserDefineWithUserId("Role99"));

            //VERIFY
            status.IsValid.ShouldBeFalse();
            status.GetAllErrors().ShouldStartWith("Line/index 2, char: 1: The role Role99 wasn't found in the auth database.");
        }

        [Fact]
        public async Task TestAddUserRolesToDatabaseIfEmptySetupWithTenantsGood()
        {
            //SETUP
            var options = SqliteInMemory.CreateOptions<AuthPermissionsDbContext>();
            using var context = new AuthPermissionsDbContext(options);
            context.Database.EnsureCreated();

            await context.SetupRolesInDbAsync();
            context.SetupSingleTenantsInDb();

            context.ChangeTracker.Clear();

            var service = new BulkLoadUsersService(context, new StubIFindUserInfoFactory(true), new AuthPermissionsOptions{TenantType = TenantTypes.SingleTenant});

            //ATTEMPT
            var status = await service.AddUsersRolesToDatabaseAsync(
                SetupHelpers.TestUserDefineWithTenants());

            //VERIFY
            status.IsValid.ShouldBeTrue(status.GetAllErrors());
            var users = context.AuthUsers.Include(x => x.UserTenant).ToList();
            users.Count(x => x.UserTenant != null ).ShouldEqual(3);
        }

        [Fact]
        public async Task TestAddUserRolesToDatabaseIfEmptySetupWithTenantsMissingTenant()
        {
            //SETUP
            var options = SqliteInMemory.CreateOptions<AuthPermissionsDbContext>();
            using var context = new AuthPermissionsDbContext(options);
            context.Database.EnsureCreated();

            await context.SetupRolesInDbAsync();
            context.SetupSingleTenantsInDb();

            context.ChangeTracker.Clear();

            var service = new BulkLoadUsersService(context, new StubIFindUserInfoFactory(true), new AuthPermissionsOptions { TenantType = TenantTypes.SingleTenant });

            //ATTEMPT
            var status = await service.AddUsersRolesToDatabaseAsync(
                SetupHelpers.TestUserDefineWithTenants("Tenant99"));

            //VERIFY
            status.IsValid.ShouldBeFalse();
            status.GetAllErrors().ShouldStartWith("Line/index 1: The user User2 has a tenant name of Tenant99 which wasn't found in the auth database.");
        }

        [Fact]
        public async Task TestAddUserRolesToDatabaseIfEmptySetupWithTenantsNoTenant()
        {
            //SETUP
            var options = SqliteInMemory.CreateOptions<AuthPermissionsDbContext>();
            using var context = new AuthPermissionsDbContext(options);
            context.Database.EnsureCreated();

            await context.SetupRolesInDbAsync();
            context.SetupSingleTenantsInDb();

            context.ChangeTracker.Clear();

            var service = new BulkLoadUsersService(context, new StubIFindUserInfoFactory(true), new AuthPermissionsOptions { TenantType = TenantTypes.SingleTenant });

            //ATTEMPT
            var status = await service.AddUsersRolesToDatabaseAsync(
                SetupHelpers.TestUserDefineWithTenants(null));

            //VERIFY
            status.IsValid.ShouldBeTrue(status.GetAllErrors());
            var users = context.AuthUsers.Include(x => x.UserTenant).ToList();
            users.Count.ShouldEqual(3);
            users.Count(x => x.UserTenant != null).ShouldEqual(2);
        }

    }
}
