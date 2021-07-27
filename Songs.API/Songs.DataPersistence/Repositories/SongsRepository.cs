using Songs.Common.Entities;
using Songs.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Songs.DataPersistence.Repositories
{
    public class SongsRepository : ISongsRepository
    {
        private IList<Song> _songs;

        public SongsRepository()
        {
            _songs = new List<Song>();
            _songs.Add(new Song { Id = Guid.NewGuid(), Title = "test", Artist = "testt", Time = 2.5, Genre = Common.Enums.Genre.Rock });
        }

        /// <summary>
        /// Add a new song in the list.
        /// </summary>
        /// <param name="song"></param>
        /// <returns></returns>
        public Song Add(Song song)
        {
            song.Id = Guid.NewGuid();
            _songs.Add(song);
            return song;
        }

        /// <summary>
        /// Get all songs. Results are paged
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IEnumerable<Song> Get(int pageNumber, int pageSize)
        {
            return _songs.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);
        }
        /// <summary>
        /// Get the song with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Song GetById(Guid id)
        {
            return _songs.FirstOrDefault(x=>x.Id == id);
        }

        //new featureq
        public Song GetByTitle(string tilte)
        {
            return _songs.FirstOrDefault(x => x.Title == tilte);
        }

        /// <summary>
        /// Delete an existing song.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Remove(Guid id)
        {
            var song = _songs.FirstOrDefault(x => x.Id == id);
            if (song == null)
                throw new KeyNotFoundException("Song with the given id does not exist");
            return _songs.Remove(song);
        }

        /// <summary>
        /// Update an existing song.
        /// </summary>
        /// <param name="updatedSong"></param>
        /// <returns></returns>
        public bool Update(Song updatedSong)
        {
            var song = _songs.FirstOrDefault(x => x.Id == updatedSong.Id);
            if (song == null)
                throw new KeyNotFoundException($"Song with given id does not exist");

            song.Title = updatedSong.Title;
            song.Genre = updatedSong.Genre;
            song.Time = updatedSong.Time;
            song.Artist = updatedSong.Artist;
           
            return true;
        }
    }
}
