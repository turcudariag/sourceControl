using Songs.API.Filters.Auth;
using Songs.API.Middleware.Auth;
using Songs.API.Models;
using Songs.API.Utils;
using Songs.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Songs.API.Controllers
{
    
    [Route("api/v2/songs")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v2")]
    public class SongsController : ControllerBase
    {
        private ISongsServiceAsync _songsServiceAsync;

        public SongsController(ISongsServiceAsync songsServiceAsync)
        {
            _songsServiceAsync = songsServiceAsync;
        }

             
        /// <summary>
        /// Get the song with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns the song with the given id</returns>
        [HttpGet]
        [Route("{id}")]
        [Authorize(Policy = Policies.All)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(SongModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Id cannot be empty");

            var result = await _songsServiceAsync.GetByIdAsync(id);
            if (result == null)
                return NotFound();

            return Ok(result.ToModel());
        }

        /// <summary>
        /// Get all songs.
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns>Returns the list of all songs </returns>
        [HttpGet]
        [Authorize(Policy = Policies.All)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<SongModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] int? pageNumber = null, [FromQuery] int? pageSize = null)
        {
            if (!pageNumber.HasValue) pageNumber = 1;
            if (!pageSize.HasValue) pageSize = 10;

            var results = await _songsServiceAsync.GetAsync(pageNumber.Value,pageSize.Value);
            if (!results.Any())
                return NoContent();

            return Ok(results.Select(x => x.ToModel()));
        }

        /// <summary>
        /// Add a new song.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = Policies.Admin)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(SongModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Add([FromBody] SongModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _songsServiceAsync.AddAsync(model.ToDto(null));

            return Ok(result.ToModel());
        }

        /// <summary>
        /// Update an existing song.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{id}")]
        [Authorize(Policy = Policies.Admin)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] SongModel model)
        {
            if (id == Guid.Empty)
                return BadRequest("Id cannot be empty");

            if (id != model.Id)
                return BadRequest("Ids do not match");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _songsServiceAsync.UpdateAsync(model.ToDto(id));
            if (await result)
                return Ok();

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        /// <summary>
        /// Delete an existing song.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        [Authorize(Policy = Policies.Admin)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Id cannot be empty");

            var result = _songsServiceAsync.RemoveAsync(id);
            if (await result)
                return Ok();

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
