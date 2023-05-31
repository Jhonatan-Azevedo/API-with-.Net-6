using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MoviesApi.Data;
using MoviesApi.Data.Dtos;
using MoviesApi.Models;

namespace MoviesApi.Controllers;

[ApiController]
[Route("[controller]")]
public class MovieController : ControllerBase
{
    private MovieContext _context;
    private IMapper _mapper;

    public MovieController(MovieContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Adiciona um filme ao banco de dados
    /// </sumary>
    /// <param name="movieDto">Objeto com os campos necessários para criação de um filme</param>
    /// <returns>IActionResult</returns>
    /// <response code="201">Caso inserção seja feita com sucesso</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult AddMovie([FromBody] CreateMovieDto movieDto)
    {
        Movie movie = _mapper.Map<Movie>(movieDto);
        _context.Movies.Add(movie);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetMovieId), new { id = movie.Id }, movie);
    }

    [HttpGet]
    public IEnumerable<ReadMovieDto> GetAllMovies([FromQuery] int skip = 0, [FromQuery] int take = 15)
    {
        return _mapper.Map<List<ReadMovieDto>>(_context.Movies.Skip(skip).Take(take));
    }

    [HttpGet("{id}")]
    public IActionResult GetMovieId(int id)
    {
        var movie = _context.Movies.FirstOrDefault(movie => movie.Id == id);
        if (movie == null) return NotFound();

        var movieDto = _mapper.Map<ReadMovieDto>(movie);
        return Ok(movieDto);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateMovie(int id, [FromBody] UpdateMovieDto movieDto)
    {
        var movie = _context.Movies.FirstOrDefault(movie => movie.Id == id);
        if (movie == null) return NotFound();

        _mapper.Map(movieDto, movie);
        _context.SaveChanges();
        return NoContent();
    }

    [HttpPatch("{id}")]
    public IActionResult UpdateMoviePatial(int id, JsonPatchDocument<UpdateMovieDto> patch)
    {
        var movie = _context.Movies.FirstOrDefault(movie => movie.Id == id);
        if (movie == null) return NotFound();

        var updateMovie = _mapper.Map<UpdateMovieDto>(movie);

        patch.ApplyTo(updateMovie, ModelState);
        if (!TryValidateModel(updateMovie)) return ValidationProblem(ModelState);

        _mapper.Map(updateMovie, movie);
        _context.SaveChanges();
        return NoContent();



    }

    [HttpDelete("{id}")]
    public IActionResult DeleteMovie(int id) 
    {
        var movie = _context.Movies.FirstOrDefault(movie => movie.Id == id);
        if (movie == null) return NotFound();

        _context.Remove(movie);
        _context.SaveChanges();
        return NoContent();
    }
}
