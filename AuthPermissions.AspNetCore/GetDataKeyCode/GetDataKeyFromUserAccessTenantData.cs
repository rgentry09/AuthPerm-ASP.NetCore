﻿// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using AuthPermissions.AspNetCore.AccessTenantData;
using AuthPermissions.AspNetCore.AccessTenantData.Services;
using AuthPermissions.CommonCode;
using AuthPermissions.SetupCode;
using Microsoft.AspNetCore.Http;

namespace AuthPermissions.AspNetCore.GetDataKeyCode
{

    /// <summary>
    /// This service is registered if a multi-tenant setup is defined <see cref="AuthPermissionsOptions.TenantType"/>
    /// and the <see cref="AuthPermissionsOptions.LinkToTenantType"/> is not set to <see cref="LinkToTenantTypes.NotTurnedOn"/>
    /// </summary>
    public class GetDataKeyFromUserAccessTenantData : IGetDataKeyFromUser
    {
        /// <summary>
        /// This will return the AuthP' DataKey claim, unless the <see cref="AccessTenantDataCookie"/> overrides it
        /// If no <see cref="HttpContext"/>, or no user, or no claim and no override from the <see cref="AccessTenantDataCookie"/> then returns null
        /// </summary>
        /// <param name="accessor">IHttpContextAccessor</param>
        /// <param name="options"><see cref="AuthPermissionsOptions"/> to access the <see cref="AuthPermissionsOptions.LinkToTenantType"/> parameter</param>
        /// <param name="linkService">service to get </param>
        public GetDataKeyFromUserAccessTenantData(IHttpContextAccessor accessor,
            AuthPermissionsOptions options,
            ILinkToTenantDataService linkService)
        {
            if (accessor.HttpContext == null)
                //If no IHttpContextAccessor, then in startup or background service so don't try linkService
                return;

            DataKey = accessor.HttpContext.User.GetAuthDataKeyFromUser();
            if (options.LinkToTenantType == LinkToTenantTypes.AppAndHierarchicalUsers // always look for DataKey override with tenant users
                || DataKey == null) //if LinkToTenantType is OnlyAppUsers, then only override DataKey on app users.
            {
                DataKey = linkService.GetDataKeyOfLinkedTenant();
            }
            
        }

        /// <summary>
        /// The AuthP' DataKey, can be null.
        /// </summary>
        public string DataKey { get; }
    }
}