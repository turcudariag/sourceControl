﻿using System;
using System.Text.Json.Serialization;


namespace Songs.Common.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string UserRole { get; set; }

        [JsonIgnore]
        public string PasswordHash { get; set; }
    }
}
