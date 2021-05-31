﻿// Copyright (c) 2019 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using AuthPermissions.PermissionsCode;
using Microsoft.AspNetCore.Authorization;

namespace AuthPermissions.AspNetCore
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false)]
    public class HasPermissionAttribute : AuthorizeAttribute
    {
        public HasPermissionAttribute(object permission) : base(permission?.ToString()!)
        {
            if (permission == null) throw new ArgumentNullException(nameof(permission));
            if (!permission.GetType().IsEnum)
                throw new ArgumentException("Must be an enum");
            if (Enum.GetUnderlyingType(permission.GetType()) != typeof(short))
                throw new InvalidOperationException(
                    $"The enum permissions {permission.GetType().Name} should by 16 bits in size to work.\n" +
                    $"Please add ': short' to your permissions declaration, i.e. public enum {permission.GetType().Name} : short " + "{...};");
        }
    }
   

}