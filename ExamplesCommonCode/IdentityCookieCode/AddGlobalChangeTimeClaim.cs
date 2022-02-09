﻿// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthPermissions;

namespace ExamplesCommonCode.IdentityCookieCode;

public class AddGlobalChangeTimeClaim : IClaimsAdder
{
    /// <summary>
    /// This adds the current time (utc) as the last time the user's claims were updated
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public Task<Claim> AddClaimToUserAsync(string userId)
    {
        var claim = new TimeSpan(0, 0, 0, 0).CreateTimeToRefreshUserClaim();
        return Task.FromResult(claim);
    }
}