using Songs.Common.Entities;
using Songs.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Songs.DataPersistence.Repositories
{
    public class SongsRepositoryAsync : ISongsRepositoryAsync
    {
        private IList<Song> _songs;

        public SongsRepositoryAsync()
        {
            _songs = new List<Song>();
        }

        /// <summary>
        /// Add a new song in the list.
        /// </summary>
        /// <param name="song"></param>
        /// <returns></returns>
        public async Task<Song> AddAsync(Song song)
        {
            song.Id = Guid.NewGuid();
            _songs.Add(song);

            return song;
        }

        /// <summary>
        /// Get the list of the songs. Results are paged.
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Song>> GetAsync(int pageNumber, int pageSize)
        {
            return _songs.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);
        }

        /// <summary>
        /// Get the song with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Song> GetByIdAsync(Guid id)
        {
            return _songs.FirstOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// Delete an existing song.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(Guid id)
        {
            var existingEntity = _songs.FirstOrDefault(x => x.Id == id);
            if (existingEntity == null)
                throw new KeyNotFoundException("Song with given id does not exist");

            return _songs.Remove(existingEntity);
        }

        /// <summary>
        /// Update an existing song.
        /// </summary>
        /// <param name="updatedSong"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(Song updatedSong)
        {
            var existingEntity = _songs.FirstOrDefault(x => x.Id == updatedSong.Id);
            if (existingEntity == null)
                throw new KeyNotFoundException("Song with given id does not exist");

            existingEntity.Title = updatedSong.Title;
            existingEntity.Artist = updatedSong.Artist;
            existingEntity.Time = updatedSong.Time;
            existingEntity.Genre = updatedSong.Genre;
            return true;
        }
    }
}
