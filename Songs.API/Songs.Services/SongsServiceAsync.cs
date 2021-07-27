using Songs.Common.DTOs;
using Songs.Common.Entities;
using Songs.Common.Util;
using Songs.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Songs.Services
{
    public class SongsServiceAsync : ISongsServiceAsync
    {
        private readonly ISongsRepository _songsRepository;

        public SongsServiceAsync(ISongsRepository songsRepository)
        {
            _songsRepository = songsRepository;
        }

        /// <summary>
        /// Add a new song.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<SongDto> AddAsync(SongDto entity)
        {
            var result = _songsRepository.Add(entity.ToEntity());
            return result.ToDto();
        }

        /// <summary>
        /// Get the list of all songs.
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SongDto>> GetAsync(int pageNumber = 1, int pageSize = 100)
        {
            var results = _songsRepository.Get(pageNumber, pageSize).ToList();
            return results.Any() ? results.Select(x => x.ToDto()) : new List<SongDto>();
        }

        /// <summary>
        /// Get the song with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<SongDto> GetByIdAsync(Guid id)
        {
            return _songsRepository.GetById(id)?.ToDto();

        }

        /// <summary>
        /// Delete the song with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(Guid id)
        {
            return _songsRepository.Remove(id);
        }

        /// <summary>
        /// Update an existing song.
        /// </summary>
        /// <param name="updatedEntity"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(SongDto updatedEntity)
        {
            return _songsRepository.Update(updatedEntity.ToEntity());
        }
    }
}
