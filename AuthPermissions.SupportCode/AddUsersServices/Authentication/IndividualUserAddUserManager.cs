﻿// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using AuthPermissions.AdminCode;
using AuthPermissions.BaseCode.CommonCode;
using Microsoft.AspNetCore.Identity;
using StatusGeneric;

namespace AuthPermissions.SupportCode.AddUsersServices.Authentication;

/// <summary>
/// This the implementation of the <see cref="IAuthenticationAddUserManager"/> for the Individual User Accounts authentication handler
/// This will create (or find) an individual user account and then create an AuthUser linked to that individual user.
/// It uses the the authP data in the <see cref="AddNewUserDto"/> class when creating the AuthUser
/// </summary>
/// <typeparam name="TIdentity"></typeparam>
public class IndividualUserAddUserManager<TIdentity> : IAuthenticationAddUserManager
    where TIdentity : IdentityUser, new()
{
    private readonly IAuthUsersAdminService _authUsersAdmin;
    private readonly IAuthTenantAdminService _tenantAdminService;
    private readonly UserManager<TIdentity> _userManager;
    private readonly SignInManager<TIdentity> _signInManager;

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="authUsersAdmin"></param>
    /// <param name="tenantAdminService"></param>
    /// <param name="userManager"></param>
    /// <param name="signInManager"></param>
    public IndividualUserAddUserManager(IAuthUsersAdminService authUsersAdmin, IAuthTenantAdminService tenantAdminService, UserManager<TIdentity> userManager, SignInManager<TIdentity> signInManager)
    {
        _authUsersAdmin = authUsersAdmin;
        _tenantAdminService = tenantAdminService;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    /// <summary>
    /// This Add User Manager works with the Individual User Accounts authentication handler
    /// </summary>
    public string AuthenticationGroup { get; } = "IndividualUserAccounts";

    /// <summary>
    /// This holds the data provided for the login.
    /// Used to check that the email of the person who will login is the same as the email provided by the user
    /// NOTE: Email and UserName can be null if providing a default value
    /// </summary>
    public AddNewUserDto NewUserLogin { get; private set; }

    /// <summary>
    /// This makes a quick check that the user isn't already has an AuthUser 
    /// </summary>
    /// <param name="newUser"></param>
    /// <returns>status, with error if there an user already</returns>
    public async Task<IStatusGeneric> CheckNoExistingAuthUserAsync(AddNewUserDto newUser)
    {
        var status = new StatusGenericHandler();
        if ((await _authUsersAdmin.FindAuthUserByEmailAsync(newUser.Email))?.Result != null)
            return status.AddError("There is already an AuthUser with your email, so you can't add another.");
        return status;
    }

    /// <summary>
    /// This either register the user and creates the AuthUser to match, or for
    /// external authentication handlers where you can't get a user's data before the login 
    /// it adds the new user AuthP information into the database to be read within the login event
    /// </summary>
    /// <param name="newUser">The information for creating an AuthUser </param>
    /// <param name="password">This is used to create a user.
    /// It also checks if there is a user already, which could happen if the user's login failed</param>
    /// <returns>status</returns>
    public async Task<IStatusGeneric> SetUserInfoAsync(AddNewUserDto newUser, string password = null)
    {
        NewUserLogin = newUser ?? throw new ArgumentNullException(nameof(newUser));

        var status = new StatusGenericHandler { Message = "New user with claims added" };

        var user = await _userManager.FindByEmailAsync(newUser.Email);
        if (user == null)
        {
            user = new TIdentity { UserName = newUser.UserName, Email = newUser.Email };
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                result.Errors.Select(x => x.Description).ToList().ForEach(error => status.AddError(error));
            }
        }
        else if (!await _userManager.CheckPasswordAsync(user, password))
            status.AddError("The user was already known, but the password was wrong.");

        //We have created the individual user account, so we have the user's UserId.
        //Now we create the AuthUser using the data we have been given

        var tenantName = newUser.TenantId == null
            ? null
            : (await _tenantAdminService.GetTenantViaIdAsync((int)newUser.TenantId)).Result?.TenantFullName;

        status.CombineStatuses(await _authUsersAdmin.AddNewUserAsync(user.Id, 
            newUser.Email, newUser.UserName, newUser.Roles, tenantName));

        return status;
    }

    /// <summary>
    /// This logs in the user, checking that the email / username are the same as was provided
    /// </summary>
    /// <param name="givenEmail">email to login by</param>
    /// <param name="givenUserName">username to login by</param>
    /// <param name="isPersistent">true if cookie should be persistent</param>
    /// <returns>status</returns>
    public async Task<IStatusGeneric> LoginVerificationAsync(string givenEmail, string givenUserName, bool isPersistent)
    {
        if (NewUserLogin == null)
            throw new AuthPermissionsException($"Must call {nameof(SetUserInfoAsync)} before calling this method.");

        var normalizedEmail = givenEmail.Trim().ToLower();

        var status = new StatusGenericHandler();
        if (NewUserLogin.Email != null && NewUserLogin.Email != normalizedEmail)
            return status.AddError("The email you used isn't the one that was expected.");
        if (NewUserLogin.UserName != null && NewUserLogin.UserName != givenUserName)
            return status.AddError("The username you used isn't the one that was expected.");

        var user = await _userManager.FindByEmailAsync(normalizedEmail);
        await _signInManager.SignInAsync(user, isPersistent: isPersistent);

        return status;
    }
}