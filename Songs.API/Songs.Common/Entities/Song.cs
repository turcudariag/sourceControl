using Songs.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Songs.Common.Entities
{
    public class Song
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Artist { get; set; }

        public double Time { get; set; }

        public Genre Genre { get; set; }
    }
}
