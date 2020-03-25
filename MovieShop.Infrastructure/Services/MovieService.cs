using MovieShop.Core.ApiModels.Request;
using MovieShop.Core.ApiModels.Response;
using MovieShop.Core.Entities;
using MovieShop.Core.Helpers;
using MovieShop.Core.RepositoryInterfaces;
using MovieShop.Core.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static MovieShop.Core.ApiModels.Response.MovieDetailsResponseModel;

namespace MovieShop.Infrastructure.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        private readonly ICastRepository _castRepository;
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IGenreRepository _genreRepository;


        public MovieService(IMovieRepository movieRepository, ICastRepository castRepository, IFavoriteRepository favoriteRepository, IGenreRepository genreRepository)
        {
            _movieRepository = movieRepository;
            _castRepository = castRepository;
            _favoriteRepository = favoriteRepository;
            _genreRepository = genreRepository;
        }
        public async Task<MovieDetailsResponseModel> CreateMovie(MovieCreateRequest movieCreateRequest)
        {
            var movie = new Movie();
            {
                movieCreateRequest.Id = movie.Id;
                movieCreateRequest.Title = movie.Title;
                movieCreateRequest.PosterUrl = movie.PosterUrl;
                movieCreateRequest.BackdropUrl = movie.BackdropUrl;
                movieCreateRequest.Overview = movie.Overview;
                movieCreateRequest.Tagline = movie.Tagline;
                movieCreateRequest.Budget = movie.Budget;
                movieCreateRequest.Revenue = movie.Revenue;
                movieCreateRequest.ImdbUrl = movie.ImdbUrl;
                movieCreateRequest.TmdbUrl = movie.TmdbUrl;
                movieCreateRequest.ReleaseDate = movie.ReleaseDate;
                movieCreateRequest.RunTime = movie.RunTime;
                movieCreateRequest.Price = movie.Price;
            }
            var movies = await _movieRepository.AddAsync(movie);
            var responseMovies = new MovieDetailsResponseModel
            {
                Id = movieCreateRequest.Id,
                Title = movieCreateRequest.Title,
                PosterUrl = movieCreateRequest.PosterUrl,
                BackdropUrl = movieCreateRequest.BackdropUrl,
                Rating = movie.Rating,
                Overview = movieCreateRequest.Overview,
                Tagline = movieCreateRequest.Tagline,
                Budget = movieCreateRequest.Budget,
                Revenue = movieCreateRequest.Revenue,
                ImdbUrl = movieCreateRequest.ImdbUrl,
                TmdbUrl = movieCreateRequest.TmdbUrl,
                ReleaseDate = movieCreateRequest.ReleaseDate,
                RunTime = movieCreateRequest.RunTime,
                Price = movieCreateRequest.Price,
                //FavoritesCount = movieCreateRequest,
            };
            return responseMovies;

        }
        public async Task<MovieDetailsResponseModel> UpdateMovie(MovieCreateRequest movieCreateRequest)
        {
            var movie = new Movie();
            {
                movieCreateRequest.Id = movie.Id;
                movieCreateRequest.Title = movie.Title;
                movieCreateRequest.PosterUrl = movie.PosterUrl;
                movieCreateRequest.BackdropUrl = movie.BackdropUrl;
                //movieCreateRequest.Rating = movie.Rating;
                movieCreateRequest.Overview = movie.Overview;
                movieCreateRequest.Tagline = movie.Tagline;
                movieCreateRequest.Budget = movie.Budget;
                movieCreateRequest.Revenue = movie.Revenue;
                movieCreateRequest.ImdbUrl = movie.ImdbUrl;
                movieCreateRequest.TmdbUrl = movie.TmdbUrl;
                movieCreateRequest.ReleaseDate = movie.ReleaseDate;
                movieCreateRequest.RunTime = movie.RunTime;
                movieCreateRequest.Price = movie.Price;
                //movieCreateRequest.FavoritesCount = movie.Favorites.Count;
            }
            var movies = await _movieRepository.UpdateAsync(movie);
            var responseMovies = new MovieDetailsResponseModel
            {
                Id = movieCreateRequest.Id,
                Title = movieCreateRequest.Title,
                PosterUrl = movieCreateRequest.PosterUrl,
                Overview = movieCreateRequest.Overview,
                Tagline = movieCreateRequest.Tagline,
                Budget = movieCreateRequest.Budget,
                Revenue = movieCreateRequest.Revenue,
                ImdbUrl = movieCreateRequest.ImdbUrl,
                TmdbUrl = movieCreateRequest.TmdbUrl,
                ReleaseDate = movieCreateRequest.ReleaseDate,
                RunTime = movieCreateRequest.RunTime,
                Price = movieCreateRequest.Price,
                //FavoritesCount = movieCreateRequest,
            };
            return responseMovies;
        }

        public Task<PagedResultSet<MovieResponseModel>> GetAllMoviePurchasesByPagination(int pageSize = 20, int page = 0)
        {
            throw new NotImplementedException();
        }

        public Task<PaginatedList<MovieResponseModel>> GetAllPurchasesByMovieId(int movieId)
        {
            throw new NotImplementedException();
        }

       
        public async Task<MovieDetailsResponseModel> GetMovieAsync(int id)
        {
            var movie = await _movieRepository.GetByIdAsync(id);
            var casts = await _castRepository.GetCastByMovieId(id);
            var favoritecount = await _favoriteRepository.GetFavoriteCountAsync(id);
            var moviegenre = await _genreRepository.GetGenreByMovieId(id);


            var responseMovies = new MovieDetailsResponseModel
            {
                Id = movie.Id,
                Title = movie.Title,
                PosterUrl = movie.PosterUrl,
                BackdropUrl = movie.BackdropUrl,
                Rating = movie.Rating,
                Overview = movie.Overview,
                Tagline = movie.Tagline,
                Budget = movie.Budget,
                Revenue = movie.Revenue,
                ImdbUrl = movie.ImdbUrl,
                TmdbUrl = movie.TmdbUrl,
                ReleaseDate = movie.ReleaseDate,
                RunTime = movie.RunTime,
                Price = movie.Price,
                FavoritesCount = favoritecount,
                Casts = new List<CastResponseModel>(),
                Genres = moviegenre.ToList(),           
            };

            foreach (var cast in casts)
            {
                var moviecast = await _castRepository.GetMovieCastById(cast.Id, id);
                responseMovies.Casts.Add(new MovieDetailsResponseModel.CastResponseModel
                {
                    Id = cast.Id,
                    Name = cast.Name,
                    Gender = cast.Gender,
                    TmdbUrl = cast.TmdbUrl,
                    ProfilePath = cast.ProfilePath,
                    Character = moviecast.Character
                });
            }
            return responseMovies;

           

        }

        public async Task<IEnumerable<MovieResponseModel>> GetMoviesByGenre(int genreId)
        {
            var movies = await _movieRepository.GetMoviesByGenre(genreId);
            var responseMovies = new List<MovieResponseModel>();
            foreach (var movie in movies)
            {
                responseMovies.Add(new MovieResponseModel
                {
                    Id = movie.Id,
                    Title = movie.Title,
                    PosterUrl = movie.PosterUrl,
                    ReleaseDate = movie.ReleaseDate.Value,
                });
            }
            return responseMovies;
        }

        public async Task<PagedResultSet<MovieResponseModel>> GetMoviesByPagination(int page = 20, int pageSize = 25, string title = "")
        {
            var movies = await _movieRepository.GetPagedData(page, pageSize);
            var responseMovies = new List<MovieResponseModel>();
            foreach (var item in movies)
            {

                responseMovies.Add(new MovieResponseModel
                {
                    Id = item.Id,
                    PosterUrl = item.PosterUrl,
                    ReleaseDate = item.ReleaseDate.Value,
                    Title = item.Title

                });

            }
            var set = new PagedResultSet<MovieResponseModel>(responseMovies, page, pageSize, movies.Count);
            return set;
        }

        public async Task<int> GetMoviesCount(string title = "")
        {
            var count = await _movieRepository.GetCountAsync();
            return count;
        }

        public async Task<IEnumerable<ReviewMovieResponseModel>> GetReviewsForMovie(int id)
        {
            var reviews = await _movieRepository.GetMovieReviews(id);
            var responseReviews = new List<ReviewMovieResponseModel>();
            foreach (var review in reviews)
            {
                responseReviews.Add(new ReviewMovieResponseModel
                { 
                    UserId = review.UserId,
                    MovieId = review.MovieId,
                    ReviewText = review.ReviewText,
                    Rating = review.Rating,
                    Name = review.User.FirstName + " " + review.User.LastName,
                });
  
           }
            return responseReviews;
        }
        public async Task<IEnumerable<MovieResponseModel>> GetHighestGrossingMovies()
        {
            //manually mapping: create MovieResponseModel object and fill the required property value from Movie Entity 
            var movies = await _movieRepository.GetHighestGrossingMovies();
            var responseMovies = new List<MovieResponseModel>();
            foreach (var movie in movies)
            {
                responseMovies.Add(new MovieResponseModel
                {
                    Id = movie.Id,
                    Title = movie.Title,
                    PosterUrl = movie.PosterUrl,
                    ReleaseDate = movie.ReleaseDate.Value,
                });
            }
            return responseMovies;
        }
        //testing three layers: service --> repository --> DB(typically not touch it, testing performanace)
        //breake calling
        //create a Moqtest to replace the real repository, avoid to touch the DB
        //unit test should be fast, find bugs as much as we can to minimize demage in the future
        //every method will have more than one unit test for different testing purpose
        //help find bugs before going to actual production

        public async Task<IEnumerable<MovieResponseModel>> GetTopRatedMovies()
        {
            var movies = await _movieRepository.GetTopRatedMovies();
            var responseMovies = new List<MovieResponseModel>();
            foreach (var movie in movies)
            {
                responseMovies.Add(new MovieResponseModel
                {
                    Id = movie.Id,
                    Title = movie.Title,
                    PosterUrl = movie.PosterUrl,
                    ReleaseDate = movie.ReleaseDate.Value,
                });
            }
            return responseMovies;
        }     
    }
}

