﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Songs.API.Middleware.Auth
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PrivilegeRequirement>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PrivilegeRequirement requirement)
        {
            var claimsPrincipal = context.User;
            if (!AreClaimsValid(claimsPrincipal))
            {
                context.Fail();
                await Task.CompletedTask;
                return;
            }

            var userClaimsModel = ExtractUserClaims(claimsPrincipal);
            if (userClaimsModel.Claim_Roles == null)
                context.Fail();
            else
                ValidateUserPrivileges(context, requirement, userClaimsModel.Claim_Roles);

            await Task.CompletedTask;
        }

        #region Private Methods

        private void ValidateUserPrivileges(AuthorizationHandlerContext context, PrivilegeRequirement requirement, IEnumerable<string> userClaimRoles)
        {
            if(requirement.Role == Policies.All)
                if (userClaimRoles.Contains(Policies.User) || userClaimRoles.Contains(Policies.Admin))
                {
                    context.Succeed(requirement);
                    return;
                }

            if (userClaimRoles.Contains(requirement.Role))
                context.Succeed(requirement);
            else context.Fail();
        }

        private bool AreClaimsValid(ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal != null &&
                   claimsPrincipal.Identity.IsAuthenticated &&
                   claimsPrincipal.HasClaim(c => c.Type.Equals(AuthorizationConstants.ClaimRole)) &&
                   claimsPrincipal.HasClaim(c => c.Type.Equals(AuthorizationConstants.ClaimSubject));
        }

        private UserClaimModel ExtractUserClaims(ClaimsPrincipal claimsPrincipal)
        {
            var claimRoleValues = claimsPrincipal
                .FindAll(c => c.Type.Equals(AuthorizationConstants.ClaimRole))
                .Select(c => c.Value);
            Guid.TryParse(claimsPrincipal
                .FindFirst(c => c.Type.Equals(AuthorizationConstants.ClaimSubject)).Value, out Guid claimSubjectValue);

            return new UserClaimModel
            {
                Claim_UserId = claimSubjectValue,
                Claim_Roles = claimRoleValues
            };
        }

        #endregion
    }
}
