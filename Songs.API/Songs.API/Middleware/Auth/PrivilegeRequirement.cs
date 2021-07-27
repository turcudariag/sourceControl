using Microsoft.AspNetCore.Authorization;

namespace Songs.API.Middleware.Auth
{
    public class PrivilegeRequirement : IAuthorizationRequirement
    {
        public string Role { get; private set; }

        public PrivilegeRequirement(string role)
        {
            Role = role;
        }
    }
}
