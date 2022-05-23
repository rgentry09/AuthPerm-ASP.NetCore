﻿// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.ComponentModel.DataAnnotations;
using AuthPermissions.BaseCode.DataLayer.Classes.SupportTypes;

namespace AuthPermissions.SupportCode.AddUsersServices;

/// <summary>
/// This is used holds the data to securely add a new user to a AuthP application
/// </summary>
public class AddUserData
{
    private string _email;

    /// <summary>
    /// Contains a unique Email (normalized by applying .ToLower), which is used for lookup
    /// (can be null if using Windows authentication provider)
    /// </summary>
    [MaxLength(AuthDbConstants.EmailSize)]
    public string Email
    {
        get => _email;
        set => _email = value.Trim().ToLower();
    }

    /// <summary>
    /// Contains a unique user name
    /// This is used to a) provide more info on the user, or b) when using Windows authentication provider
    /// </summary>
    [MaxLength(AuthDbConstants.UserNameSize)]
    public string UserName { get; set; }

    /// <summary>
    /// A list of Role names to add to the AuthP user when the AuthP user is created
    /// </summary>
    public List<string> Roles { get; set; }

    /// <summary>
    /// Optional. This holds the tenantId of the tenant that the joining user should be linked to
    /// If null, then the application must not be a multi-tenant application 
    /// </summary>
    public int? TenantId { get; set; }

    /// <summary>
    /// This converts the list of roles into 
    /// </summary>
    /// <returns></returns>
    public string GetRolesAsCommaDelimited()
    {
        return string.Join(",", Roles?.Select(x => x.Trim()) ?? Array.Empty<string>());
    }
    
}