using MovieShop.Core.ApiModels.Request;
using MovieShop.Core.ApiModels.Response;
using MovieShop.Core.Entities;
using MovieShop.Core.Exceptions;
using MovieShop.Core.Helpers;
using MovieShop.Core.RepositoryInterfaces;
using MovieShop.Core.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieShop.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICryptoService _encryptionService;
        public UserService(IUserRepository userRepository, ICryptoService encryptionService)
        {
            _userRepository = userRepository;
            _encryptionService = encryptionService;
        }
        public Task AddFavorite(FavoriteRequestModel favoriteRequest)
        {
            throw new NotImplementedException();
        }

        public Task AddMovieReview(ReviewRequestModel reviewRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<UserRegisterResponseModel> CreateUser(UserRegisterRequestModel requestModel)
        {
            var dbUser = await _userRepository.GetUserByEmail(requestModel.Email);//make sure the user exist or not
            if (dbUser != null &&
                string.Equals(dbUser.Email, requestModel.Email, StringComparison.CurrentCultureIgnoreCase))
                throw new ConflictException("Email Already Exits");  //if user already exist, cannot create user ccount
            var salt = _encryptionService.CreateSalt();
            var hashedPassword = _encryptionService.HashPassword(requestModel.Password, salt);
            var user = new User
            {
                Email = requestModel.Email,
                Salt = salt,
                HashedPassword = hashedPassword,
                FirstName = requestModel.FirstName,
                LastName = requestModel.LastName
            }; //create new user information 
            var createdUser = await _userRepository.AddAsync(user); //add those information to DB
            var response = new UserRegisterResponseModel 
            {   Id = createdUser.Id, 
                Email=createdUser.Email, 
                FirstName=createdUser.FirstName, 
                LastName = createdUser.LastName
            };
            return response;
        }//manully mapping 
        public async Task<User> ValidateUser(string email, string password)
        {
            var user = await _userRepository.GetUserByEmail(email);
            if (user == null) return null;
            var hashedPassword = _encryptionService.HashPassword(password, user.Salt);
            return user.HashedPassword != hashedPassword ? null : user; //compare password entered by user and the existing password in DB
            //if(user.HashedPassword == hashedPassword)
            //return user;
            //else
            //{ 
            //return null
            //}
        }

        public Task DeleteMovieReview(int userId, int movieId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> FavoriteExists(int id, int movieId)
        {
            throw new NotImplementedException();
        }

        public Task<FavoriteResponseModel> GetAllFavoritesForUser(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PurchaseResponseModel> GetAllPurchasesForUser(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ReviewResponseModel> GetAllReviewsByUser(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResultSet<User>> GetAllUsersByPagination(int pageSize = 20, int page = 0, string lastName = "")
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUser(string email)
        {
            throw new NotImplementedException();
        }

        public Task<UserRegisterResponseModel> GetUserDetails(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsMoviePurchased(PurchaseRequestModel purchaseRequest)
        {
            throw new NotImplementedException();
        }

        public Task PurchaseMovie(PurchaseRequestModel purchaseRequest)
        {
            throw new NotImplementedException();
        }

        public Task RemoveFavorite(FavoriteRequestModel favoriteRequest)
        {
            throw new NotImplementedException();
        }

        public Task UpdateMovieReview(ReviewRequestModel reviewRequest)
        {
            throw new NotImplementedException();
        }

 
    }
}
