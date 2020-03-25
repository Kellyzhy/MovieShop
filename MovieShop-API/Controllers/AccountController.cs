using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MovieShop.Core.ApiModels.Request;
using MovieShop.Core.Entities;
using MovieShop.Core.ServiceInterfaces;

namespace MovieShop_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase //create & delete account
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _config;
        public AccountController(IUserService userService, IConfiguration config)
        {
            _userService = userService;
            _config = config;
        }

        [HttpPost]
        public async Task<ActionResult> RegisterUserAsync([FromBody] UserRegisterRequestModel user)
        {
            var createdUser = await _userService.CreateUser(user);
            return Ok(createdUser);
        }

        [HttpPost("login")]
        public async Task<ActionResult> LoginAsync([FromBody] LoginRequestModel loginRequest)
        {
            var user = await _userService.ValidateUser(loginRequest.Email, loginRequest.Password);
            if (user == null)
            {
                return Unauthorized();
            }
            return Ok(new { token = GenerateJWT(user) });
        }

        private string GenerateJWT(User user)
        {
            var claims = new List<Claim>
                         {
                             new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                            // new Claim(JwtRegisteredClaimNames.Birthdate, user.DateOfBirth?.ToShortDateString()),
                             new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName), //key, value(property)
                             new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
                             new Claim(JwtRegisteredClaimNames.Email, user.Email),
                         }; // send these information into payload ==> a list of Claim

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["TokenSettings:PrivateKey"]));//take form appseeting & validate signature
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);//alg: libarary & header
            var expires = DateTime.UtcNow.AddHours(_config.GetValue<double>("TokenSettings:ExpirationHours")); //expiration time, Utc time = Coordinated Universal Time
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identityClaims,
                Expires = expires,
                SigningCredentials = credentials,
                Issuer = _config["TokenSettings:Issuer"],
                Audience = _config["TokenSettings:Audience"]
            };
            var encodedJwt = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(encodedJwt);
        }
    }
}