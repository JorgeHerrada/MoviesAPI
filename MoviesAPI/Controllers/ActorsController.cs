using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;

namespace MoviesAPI.Controllers
{
    [ApiController]
    [Route("api/actors")]
    public class ActorsController : ControllerBase
    {
        private readonly ApplicationDbContext contextDB;
        private readonly IMapper mapper;

        public ActorsController(
            ApplicationDbContext contextDB,
            IMapper mapper)
        {
            this.contextDB = contextDB;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<ActorDTO>>> Get()
        {
            var entities = await contextDB.Actors.ToListAsync();
            var dtos = mapper.Map<List<ActorDTO>>(entities);
            return dtos;
        }

        [HttpGet("{id:int}", Name = "getActorById")]
        public async Task<ActionResult<ActorDTO>> GetById(int id)
        {
            var entity = await contextDB.Actors.FirstOrDefaultAsync(actor => actor.Id == id);

            if (entity == null)
            {
                return NotFound();
            }

            var dto = mapper.Map<ActorDTO>(entity);
            return dto;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ActorCreationDTO actorCreationDTO)
        {
            var entity = mapper.Map<Actor>(actorCreationDTO);
            contextDB.Add(entity);
            await contextDB.SaveChangesAsync();
            var autorDTO = mapper.Map<ActorDTO>(entity);

            return new CreatedAtRouteResult("getActorById", new { id = entity.Id }, autorDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] ActorCreationDTO actorCreationDTO)
        {
            var entity = mapper.Map<Actor>(actorCreationDTO);
            entity.Id = id;
            contextDB.Entry(entity).State = EntityState.Modified;
            await contextDB.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await contextDB.Actors.AnyAsync(actor => actor.Id == id);

            if (!exist)
            {
                return NotFound();
            }

            contextDB.Remove(new Actor { Id = id });
            await contextDB.SaveChangesAsync();

            return NoContent();
        }
    }
}
