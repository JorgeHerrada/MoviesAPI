using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;

namespace MoviesAPI.Controllers
{
    [ApiController]
    [Route("api/genres")]
    public class GenresController: ControllerBase
    {
        private readonly ApplicationDbContext contextDB;
        private readonly IMapper mapper;

        public GenresController(
            ApplicationDbContext contextDB,
            IMapper mapper)
        {
            this.contextDB = contextDB;
            this.mapper = mapper;
        }

        /// <summary>
        /// API endpoint to list all the Genres.
        /// </summary>
        /// <returns><see cref="ActionResult"/></returns>
        [HttpGet]
        public async Task<ActionResult<List<GenreDTO>>> Get()
        {
            var entities = await contextDB.Genres.ToListAsync();
            var dtos = mapper.Map<List<GenreDTO>>(entities);
            return dtos;
        }

        /// <summary>
        /// API endpoint to find an especific Genre by ID.
        /// </summary>
        /// <param name="id">The ID of the Genre to be found.</param>
        /// <returns><see cref="ActionResult"/></returns>
        [HttpGet("{id:int}", Name = "getGenreById")]
        public async Task<ActionResult<GenreDTO>> GetById(int id)
        {
            var entity = await contextDB.Genres.FirstOrDefaultAsync(genre => genre.Id == id);
            
            if (entity == null)
            {
                return NotFound();
            }
            
            var dto = mapper.Map<GenreDTO>(entity);

            return dto;
        }

        /// <summary>
        /// API endpoint to create a Genre.
        /// </summary>
        /// <param name="genreCreationDTO">The genreCreationDTO.</param>
        /// <returns><see cref="ActionResult"/></returns>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GenreCreationDTO genreCreationDTO)
        {
            var entity = mapper.Map<Genre>(genreCreationDTO);
            contextDB.Add(entity);
            await contextDB.SaveChangesAsync();
            var genreDTO = mapper.Map<GenreDTO>(entity);

            return new CreatedAtRouteResult("getGenreById", new { id = genreDTO.Id }, genreDTO);
        }

        /// <summary>
        /// API endpoint to Put an existing Genre.
        /// </summary>
        /// <param name="id">The ID of the Genre to be put.</param>
        /// <param name="genreCreationDTO">The genreCreationDTO.</param>
        /// <returns><see cref="ActionResult"/></returns>
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] GenreCreationDTO genreCreationDTO)
        {
            var entity = mapper.Map<Genre>(genreCreationDTO);
            entity.Id = id;
            contextDB.Entry(entity).State = EntityState.Modified; // modified entity
            await contextDB.SaveChangesAsync();
            
            return NoContent();
        }

        /// <summary>
        /// API endpoint to Delete a Genre.
        /// </summary>
        /// <param name="id">The ID of the Genre to be deleted.</param>
        /// <returns><see cref="ActionResult"/></returns>
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await contextDB.Genres.AnyAsync(genre => genre.Id == id);

            if (!exist)
            {
                return NotFound();
            }

            contextDB.Remove(new Genre() { Id = id });
            await contextDB.SaveChangesAsync();

            return NoContent();
        }
    }
}
