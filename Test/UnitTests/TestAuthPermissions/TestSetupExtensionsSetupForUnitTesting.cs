﻿// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Linq;
using System.Threading.Tasks;
using AuthPermissions;
using Microsoft.Extensions.DependencyInjection;
using Test.TestHelpers;
using Xunit;
using Xunit.Abstractions;
using Xunit.Extensions.AssertExtensions;

namespace Test.UnitTests.TestAuthPermissions
{
    public class TestSetupExtensionsSetupForUnitTesting
    {
        private readonly ITestOutputHelper _output;

        public TestSetupExtensionsSetupForUnitTesting(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task TestAddRolesToDatabaseIfEmpty()
        {
            //SETUP
            var services = new ServiceCollection();

            //ATTEMPT
            var context = await services.RegisterAuthPermissions<TestEnum>()
                .UsingInMemoryDatabase()
                .SetupForUnitTestingAsync();

            //VERIFY
            context.RoleToPermissions.Count().ShouldEqual(0);
            context.UserToRoles.Count().ShouldEqual(0);
        }

        [Fact]
        public async Task AddRolesToDatabaseIfEmptyOk()
        {
            //SETUP
            var services = new ServiceCollection();
            var lines = @"Role1 : One, Three
Role2 |my description|: One, Two, Two, Three
Role3: One";

            //ATTEMPT
            var context = await services.RegisterAuthPermissions<TestEnum>()
                .UsingInMemoryDatabase()
                .AddRolesPermissionsIfEmpty(lines)
                .SetupForUnitTestingAsync();

            //VERIFY
            context.RoleToPermissions.Count().ShouldEqual(3);
            context.UserToRoles.Count().ShouldEqual(0);
        }

        [Fact]
        public async Task AddRolesToDatabaseIfEmptyAddUsersIfEmptyOk()
        {
            //SETUP
            var services = new ServiceCollection();
            var lines = @"Role1 : One, Three
Role2 |my description|: One, Two, Two, Three
Role3: One";

            //ATTEMPT
            var context = await services.RegisterAuthPermissions<TestEnum>()
                .UsingInMemoryDatabase()
                .AddRolesPermissionsIfEmpty(lines)
                .AddUsersRolesIfEmpty(SetupHelpers.TestUserDefineWithUserId())
                .SetupForUnitTestingAsync();

            //VERIFY
            context.RoleToPermissions.Count().ShouldEqual(3);
            context.UserToRoles.Count().ShouldEqual(5);
        }
    }
}
