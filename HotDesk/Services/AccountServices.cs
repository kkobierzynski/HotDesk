using AutoMapper;
using HotDesk.Entities;
using HotDesk.Exceptions;
using HotDesk.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HotDesk.Services
{
    public interface IAccountServices
    {
        string GenerateJwt(LoginUserDto dto);
        void CreateUser(CreateUserDto dto);
    }

    public class AccountServices : IAccountServices
    {
        private readonly HotDeskDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AuthenticationSettings _authenticationSettings;

        public AccountServices(HotDeskDbContext dbContext, IMapper mapper, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
        }

        public void CreateUser(CreateUserDto dto)
        {
            var user = _mapper.Map<User>(dto);

            var hashedPassord = _passwordHasher.HashPassword(user, dto.Password);
            user.Password = hashedPassord;

            _dbContext.Add(user);
            _dbContext.SaveChanges();
        }

        public string GenerateJwt(LoginUserDto dto)
        {
            var user = _dbContext.Users
                .Include(user => user.Role)
                .FirstOrDefault(user => user.Email == dto.Email);
            if (user == null)
            {
                throw new BadRequestException("Invalid email or password");
            }
            var verifyPassword = _passwordHasher.VerifyHashedPassword(user, user.Password, dto.Password);
            if (verifyPassword == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Invalid email or password");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.Name} {user.Surname}"),
                new Claim(ClaimTypes.Role, user.Role.RoleName.ToString()),
                new Claim(ClaimTypes.Email, user.Email.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: cred);

            var securityTokenHandler = new JwtSecurityTokenHandler();
            return securityTokenHandler.WriteToken(token);

        }
    }
}
