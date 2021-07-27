using System;
using System.Collections.Generic;

namespace Songs.API.Middleware.Auth
{
    public class UserClaimModel
    {
        public Guid Claim_UserId { get; set; }
        public IEnumerable<string> Claim_Roles { get; set; }
    }
}
