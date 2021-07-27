using Songs.Common;
using Songs.Common.Entities;
using Songs.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace Songs.Interfaces
{
    public interface ISongsRepositoryAsync
    {
        Task<Song> GetByIdAsync(Guid id);

        Task<IEnumerable<Song>> GetAsync(int pageNumber, int pageSize);

        Task<Song> AddAsync(Song song);

        
        Task<bool> UpdateAsync(Song updatedSong);

        
        Task<bool> RemoveAsync(Guid id);
    }
}
