using AbysterApi.Data;
using AbysterApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AbysterApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration Configuration;
        private readonly AbysterDbContext _context;

        public AuthController(IConfiguration configuration, AbysterDbContext context)
        {
            Configuration = configuration;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<Personne>> Register(PersonneDto request)
        {
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var personne = new Personne
            {
                Nom = request.Nom,
                Prenom = request.Prenom,
                Email = request.Email,
                DateCreation = DateTime.Now,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                IsActive = true,
                Role = "Utilisateur"
            };

            _context.Personnes.Add(personne);
            await _context.SaveChangesAsync();

            return Ok(personne);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(PersonneDto request)
        {
            var personne = await _context.Personnes.FirstOrDefaultAsync(p => p.Email == request.Email);

            if (personne == null || !VerifyPasswordHash(request.Password, personne.PasswordHash, personne.PasswordSalt))
            {
                return Unauthorized("Nom d'utilisateur ou mot de passe incorrect");
            }

            string token = CreateToken(personne);
            return Ok(token);
        }

        private string CreateToken(Personne user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
              
                new Claim(ClaimTypes.Email, user.Email)
            };
            Console.WriteLine(user.Id);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["AppSettings:Token"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(Convert.ToDouble(Configuration["Jwt:ExpirationInDays"])),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}
