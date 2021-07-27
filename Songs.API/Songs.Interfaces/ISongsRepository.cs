using Songs.Common.Entities;
using Songs.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Songs.Interfaces
{
    public interface ISongsRepository
    {
        Song Add(Song song);

        bool Update(Song updatedSong);

        bool Remove(Guid id);

        Song GetById(Guid id);

        IEnumerable<Song> Get(int pageNumber, int pageSize);


        Song GetByTitle(string title);


    }
}
