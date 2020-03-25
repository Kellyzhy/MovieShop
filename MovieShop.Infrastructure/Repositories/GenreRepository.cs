using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MovieShop.Core.Entities;
using MovieShop.Core.RepositoryInterfaces;
using MovieShop.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace MovieShop.Infrastructure.Repositories
{
    public class GenreRepository : EfRepository<Genre>, IGenreRepository
    {
        public GenreRepository(MovieShopDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<IEnumerable<Genre>> GetGenreByMovieId(int movieId)
        {
            var genres = await _dbContext.MovieGenres.Where(mg => mg.MovieId == movieId).Include(mg => mg.Genre).Select(mg => mg.Genre).ToListAsync();
            return genres;
        }
    }
}
