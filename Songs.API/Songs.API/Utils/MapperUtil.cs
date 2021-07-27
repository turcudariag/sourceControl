
using Songs.Common.DTOs;
using Songs.Common.Enums;
using System;
using System.Collections.Generic;

namespace Songs.API.Utils
{
    public static class MapperUtil
    {
        public static SongModel ToModel(this SongDto dto)
        {
            return new SongModel
            {
                Id = dto.Id ?? Guid.Empty,
                Title = dto.Title,
                Time = dto.Time,
                Genre = dto.Genre.ToModel(),
                Artist = dto.Artist                
            };
        }

        public static Genre ToModel(this Common.Enums.Genre dto)
        {
            switch (dto)
            {
                case Common.Enums.Genre.Classic: return Genre.Classic;
                case Common.Enums.Genre.Pop: return Genre.Pop;
                case Common.Enums.Genre.Rock: return Genre.Rock;
                case Common.Enums.Genre.Disco: return Genre.Disco;
                
                default: return Genre.Pop;
            }
        }

       
        public static SongDto ToDto(this SongModel model, Guid? id)
        {
            return new SongDto
            {
                Id = id,
                Title = model.Title,
                Artist = model.Artist,
                Genre = model.Genre.ToDto(),
                Time = model.Time
            };
        }

        public static Common.Enums.Genre ToDto(this Genre model)
        {
            switch (model)
            {
                case Genre.Classic: return Common.Enums.Genre.Classic;
                case Genre.Pop: return Common.Enums.Genre.Pop;
                case Genre.Rock: return Common.Enums.Genre.Rock;
                case Genre.Disco: return Common.Enums.Genre.Disco;
                

                default: return Common.Enums.Genre.Pop;

                
            }
        }
    }
}
