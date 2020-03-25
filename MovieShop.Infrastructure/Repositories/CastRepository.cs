using Microsoft.EntityFrameworkCore;
using MovieShop.Core.Entities;
using MovieShop.Core.RepositoryInterfaces;
using MovieShop.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieShop.Infrastructure.Repositories
{
    public class CastRepository: EfRepository<Cast>, ICastRepository
    {
        public CastRepository(MovieShopDbContext dbContext) : base(dbContext)
        {

        }
        public async Task<IEnumerable<Cast>> GetCastByMovieId(int movieId)
        {
            var cast = await _dbContext.MovieCasts.Where(m => m.MovieId == movieId).Include(mc => mc.Cast).Select(c => c.Cast).ToListAsync();
            return cast;

            //var movies = await _dbContext.MovieGenres.Where(g => g.GenreId == genreId).Include(mg => mg.Movie)
            //                            .Select(m => m.Movie)
            //                            .ToListAsync();
           // return movies;
        }

        public async Task<MovieCast> GetMovieCastById(int castId, int movieid)
        {
            var moviecast = await _dbContext.MovieCasts.Where(mc=>mc.CastId == castId && mc.MovieId == movieid).FirstOrDefaultAsync();
                                       
            return moviecast;
        }
    }
}

