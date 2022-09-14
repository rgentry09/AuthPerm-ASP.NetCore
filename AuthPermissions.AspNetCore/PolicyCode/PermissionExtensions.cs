﻿using Microsoft.AspNetCore.Builder;

namespace AuthPermissions.AspNetCore.PolicyCode
{
    /// <summary>
    /// Class which contains extensions for the ASP.Net core permission policy
    /// </summary>
    public static class PermissionExtensions
    {
        /// <summary>
        /// The fluent minimal api implementation of <see cref="HasPermissionAttribute"/>
        /// </summary>
        /// <typeparam name="TBuilder"></typeparam>
        /// <param name="builder"></param>
        /// <param name="permission"></param>
        /// <returns></returns>
        public static TBuilder HasPermission<TBuilder>(this TBuilder builder, object permission) where TBuilder : IEndpointConventionBuilder
        {
            var authorizeData = new HasPermissionAttribute(permission);
            builder.RequireAuthorization(authorizeData);

            return builder;
        }
    }
}
