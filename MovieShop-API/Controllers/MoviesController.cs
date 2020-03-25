using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieShop.Core.ServiceInterfaces;

namespace MovieShop_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;
        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetMovies(int page = 1, int pageSize = 25, String title = "")
        {//From Query String
            return Ok(await _movieService.GetMoviesByPagination(page, pageSize, title));
        }

        [HttpGet]
        [Route("toprated")]
        public async Task<IActionResult> GetTopRatedMovies()
        {
            var movies = await _movieService.GetTopRatedMovies();
            return Ok(movies);
        }

        [HttpGet]
        [Route("toprevenue")]
        public async Task<IActionResult> GetHighestGrossingMovies()
        {
            var movies = await _movieService.GetHighestGrossingMovies();
            return Ok(movies);
        }

        [HttpGet]
        [Route("genre/{genreId}")]
        public async Task<IActionResult> GetMoviesByGenre(int genreId)
        {
            var movies = await _movieService.GetMoviesByGenre(genreId);
            return Ok(movies);
        }

        [HttpGet]
        [Route("{id}/reviews")]
        public async Task<IActionResult> GetReviewsForMovie(int id)
        {
            var movies = await _movieService.GetReviewsForMovie(id);
            return Ok(movies);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetMovies(int id)
        //From Query String
        {
            var movie = await _movieService.GetMovieAsync(id);
            return Ok(movie);
        }
    }
}