﻿// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AuthPermissions.BulkLoadServices.Concrete;
using AuthPermissions.CommonCode;
using AuthPermissions.DataLayer.EfCode;
using AuthPermissions.SetupCode;
using AuthPermissions.SetupCode.Factories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AuthPermissions.AspNetCore.HostedServices
{
    public class AddRolesTenantsUsersIfEmptyOnStartup : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public AddRolesTenantsUsersIfEmptyOnStartup(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<AuthPermissionsDbContext>();
                var authOptions = services.GetRequiredService<IAuthPermissionsOptions>();
                var findUserIdServiceFactory = services.GetRequiredService<IFindUserInfoServiceFactory>();

                var status = await context.SeedRolesTenantsUsersIfEmpty(authOptions, findUserIdServiceFactory);
                status.IfErrorsTurnToException();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}