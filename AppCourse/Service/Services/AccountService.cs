using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Service.DTOs.Account;
using Service.Helpers.Account;
using Service.Helpers.Enums;
using Service.Helpers.Exceptions;
using Service.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Service.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly JWTSettings _jwtSettings;

        public AccountService(UserManager<AppUser> userManager,
                              RoleManager<IdentityRole> roleManager,
                              IMapper mapper,
                              IConfiguration configuration,
                              IOptions<JWTSettings> jwtSettings)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _mapper = mapper;
            _configuration = configuration;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<UserDto> GetByUsernameAsync(string username)
        {
            var existUser = await _userManager.FindByNameAsync(username);

            if (existUser is null)
            {
                throw new NotFoundException($"{username} - user not found");
            }

            return _mapper.Map<UserDto>(existUser);
        }

        public async Task<IEnumerable<UserDto>> GetUsersAsync()
        {
            return _mapper.Map<IEnumerable<UserDto>>(await _userManager.Users.ToListAsync());
        }

        public async Task CreateRoleAsync()
        {
            foreach (var role in Enum.GetValues(typeof(Roles)))
            {
                if (!await _roleManager.RoleExistsAsync(role.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole
                    {
                        Name = role.ToString(),
                    });
                }
            }
        }

        public async Task<RegisterResponse> SignUpAsync(RegisterDto model)
        {
            var user = _mapper.Map<AppUser>(model);

            IdentityResult result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return new RegisterResponse { Success = false, Errors = result.Errors.Select(m => m.Description) };
            }

            //await _userManager.AddToRoleAsync(user, Roles.Member.ToString());

            return new RegisterResponse { Success = true, Errors = null };
        }

        public async Task<LoginResponse> SignInAsync(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.EmailOrUsername);

            if (user is null)
            {
                user = await _userManager.FindByNameAsync(model.EmailOrUsername);
            }

            if (user is null)
            {
                return new LoginResponse { Success = false, Error = "Login failed" , Token = null };
            }

            bool result = await _userManager.CheckPasswordAsync(user, model.Password);

            if (!result)
            {
                return new LoginResponse { Success = false, Error = "Login failed", Token = null };
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            string token = GenerateJwtToken(user.UserName, (List<string>)userRoles);

            return new LoginResponse { Success = true, Error = null, Token = token };
        }

        private string GenerateJwtToken(string username, List<string> roles)
        {
            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, username)
        };

            roles.ForEach(role =>
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            });

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_jwtSettings.ExpireDays));

            var token = new JwtSecurityToken(
                _jwtSettings.Issuer,
                _jwtSettings.Issuer,
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
