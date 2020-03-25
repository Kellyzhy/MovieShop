using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieShop.Core.ApiModels.Request;
using MovieShop.Core.ServiceInterfaces;

namespace MovieShop_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IMovieService _movieService;
        public AdminController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpPost("movie")]
        public async Task<IActionResult> AddMovies(MovieCreateRequest movieCreateRequest)
        {
            var createmovie = await _movieService.CreateMovie(movieCreateRequest);
            return CreatedAtRoute("GetMovie", new { id = createmovie.Id }, createmovie);
            //return Ok(createmovies);
        }

        [HttpPut("movie")]
        public async Task<IActionResult> UpdateMovies(MovieCreateRequest movieCreateRequest)
        {
            var updatemovies = await _movieService.UpdateMovie(movieCreateRequest);
            return Ok(updatemovies);
        }
    }
    //[HttpPost("movie")]
    //public async Task<IActionResult> CreateMovie([FromBody] MovieCreateRequest movieCreateRequest)
    //{
    //    var createdMovie = await _movieService.CreateMovie(movieCreateRequest);
    //    return CreatedAtRoute("GetMovie", new { id = createdMovie.Id }, createdMovie);
    //}
   
    
}