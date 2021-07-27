using Songs.Common.DTOs;
using Songs.Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Songs.Interfaces
{
    public interface ISongsServiceAsync
    {
        Task<SongDto> GetByIdAsync(Guid id);

        Task<IEnumerable<SongDto>> GetAsync(int pageNumber, int pageSize);

        Task<SongDto> AddAsync(SongDto entity);


        Task<bool> UpdateAsync(SongDto updatedEntity);


        Task<bool> RemoveAsync(Guid id);
    }
}
