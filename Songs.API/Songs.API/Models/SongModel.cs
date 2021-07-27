using Songs.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Songs.API
{
    public class SongModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MinLength(2, ErrorMessage = "Length must be at least 2 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Artist is required")]
        [MinLength(2, ErrorMessage = "Length must be at least 2 characters")]
        public string Artist{ get; set; }

        [Required]
        public double Time { get; set; }

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Genre Genre { get; set; }
    }
}