using Songs.Common.DTOs;
using Songs.Common.Entities;
using Songs.Common.Util;
using Songs.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Songs.Services
{
    public class SongsService : ISongsService
    {
        private readonly ISongsRepository _songsRepository;
        public SongsService(ISongsRepository songsRepository)
        {
            _songsRepository = songsRepository;
        }

        /// <summary>
        /// Add a new song.
        /// </summary>
        /// <param name="song"></param>
        /// <returns></returns>
        public SongDto Add(SongDto song)
        {
            var result = _songsRepository.Add(song.ToEntity());
            return result.ToDto();
        }

        /// <summary>
        /// Get the list of all songs.
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IEnumerable<SongDto> Get(int pageNumber = 1, int pageSize = 100)
        {
            var results = _songsRepository.Get(pageNumber, pageSize).ToList();
            return results.Any() ? results.Select(x => x.ToDto()) : new List<SongDto>();
        }

        /// <summary>
        /// Get the song with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SongDto GetById(Guid id)
        {
            return _songsRepository.GetById(id)?.ToDto();
        }

        /// <summary>
        /// Delete the song with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Remove(Guid id)
        {
            return _songsRepository.Remove(id);
        }

        /// <summary>
        /// Update the existing song.
        /// </summary>
        /// <param name="updatedSong"></param>
        /// <returns></returns>
        public bool Update(SongDto updatedSong)
        {
            return _songsRepository.Update(updatedSong.ToEntity());
        }
    }
}
