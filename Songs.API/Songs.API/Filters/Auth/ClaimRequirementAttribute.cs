using Microsoft.AspNetCore.Mvc;
using songs.API.Filters.Auth;
using System.Security.Claims;

namespace Songs.API.Filters.Auth
{
    public class ClaimRequirementAttribute : TypeFilterAttribute
    {
        public ClaimRequirementAttribute(string claimType, string claimValue) : base(typeof(ClaimRequirementFilter))
        {
            Arguments = new object[] { new Claim(claimType, claimValue) };
        }
    }
}
