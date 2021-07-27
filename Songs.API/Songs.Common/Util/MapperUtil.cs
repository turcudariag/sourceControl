using Songs.Common.DTOs;
using Songs.Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Songs.Common.Util
{
    public static class MapperUtil
    {
        public static Song ToEntity(this SongDto songDto)
        {
            return new Song
            {
                Id = songDto.Id ?? Guid.Empty,
                Title = songDto.Title,
                Artist = songDto.Artist,
                Time = songDto.Time,
                Genre = songDto.Genre
            };
        }

        public static SongDto ToDto(this Song song)
        {
            return new SongDto
            {
                Id = song.Id ,
                Title = song.Title,
                Artist = song.Artist,
                Time = song.Time,
                Genre = song.Genre
            };
        }
    }
}
