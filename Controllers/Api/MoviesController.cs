using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vidly.Data;
using Vidly.Dtos;
using AutoMapper;
using Vidly.Models;

namespace Vidly.Controllers.Api;

[ApiController]
[Route("api/[controller]")]
public class MoviesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public MoviesController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper= mapper;
    }
    [HttpGet]
    public ActionResult<IEnumerable<MovieDto>> GetMovies()
    {
        var movies = _context.Movies.Include(m => m.Genre).ToList();
        var movieDto = _mapper.Map<IEnumerable<MovieDto>>(movies);
        return Ok(movieDto);
    }

    [HttpPost]
    public ActionResult<MovieDto> Create([FromBody] MovieDto movieDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var movie = _mapper.Map<Movie>(movieDto);
        movie.DateAdded = DateTime.Now;  // server-controlled

        _context.Movies.Add(movie);
        _context.SaveChanges();

        var createdDto = _mapper.Map<MovieDto>(movie);

        return CreatedAtAction(
            nameof(GetMovie),
            new { id = movie.Id },
            createdDto
        );
    }

    // GET: /api/movie/1
    [HttpGet("{id}")]
    public ActionResult<MovieDto> GetMovie(int id)
    {
        var movie = _context.Movies.Include(m => m.Genre).SingleOrDefault(m => m.Id == id);
        if (movie == null)
            return NotFound();

        return Ok(_mapper.Map<Movie, MovieDto>(movie));
    }

    [HttpPut("{id}")]
    public ActionResult UpdateMovie(int id, MovieDto movieDto)
    {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);

        var movieInDb =  _context.Movies.SingleOrDefault(m => m.Id == id);
        if(movieInDb == null)
            return NotFound();
        
        _mapper.Map(movieDto,movieInDb);
        movieInDb.DateAdded = movieInDb.DateAdded;

        _context.SaveChanges();

        // Return updated DTO instead of NoContent
        var updatedDto = _mapper.Map<MovieDto>(movieInDb);

        return Ok(updatedDto);
    }
}