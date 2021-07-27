using Songs.Common.DTOs;
using Songs.Common.Entities;
using Songs.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Songs.Interfaces
{
    public interface ISongsService
    {
        SongDto Add(SongDto song);

        bool Update(SongDto updatedSong);

        bool Remove(Guid id);

        SongDto GetById(Guid id);

        IEnumerable<SongDto> Get(int pageNumber, int pageSize);

        //IEnumerable<Song> GetByTime(double time);
        //IEnumerable<Song> GetByArtist(string artist);
        //IEnumerable<Song> GetByGenre(Genre? genre);

    }
}
