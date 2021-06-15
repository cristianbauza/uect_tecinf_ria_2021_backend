using DataAccessLayer;
using DataAccessLayer.Models;
using LibretaDigitalBackEnd.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LibretaDigitalBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {

        //private readonly SignInManager<IdentityUser> _signInManager;
        //private readonly UserManager<IdentityUser> _userManager;
        //private readonly IConfiguration _configuration;
        //private readonly RoleManager<IdentityRole> _roleManager;
        //private readonly ApplicationDbContext _context;


        //public AuthenticateController(
        //    UserManager<IdentityUser> userManager,
        //    SignInManager<IdentityUser> signInManager,
        //    IConfiguration configuration,
        //    RoleManager<IdentityRole> rolManager,
        //    ApplicationDbContext context)
        //{
        //    _userManager = userManager;
        //    _signInManager = signInManager;
        //    _configuration = configuration;
        //    _roleManager = rolManager;
        //    _context = context;
        //}

        //[HttpPost]
        //[Route("login")]
        //[ProducesResponseType(typeof(LoginResponse), 200)]
        //[ProducesResponseType(500)]
        //public async Task<object> Login([FromBody] LoginModel model)
        //{
        //    var user = await _userManager.FindByNameAsync(model.Username);

        //    if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
        //    {
        //        var appUser = _userManager.Users.SingleOrDefault(r => r.UserName == model.Username);
        //        var res = GenerateJwtTokenAsync(model.Username, appUser);
        //        var aux = res.Result;

        //        return new LoginResponse(aux.ToString(), model.Username);
        //    }

        //    return StatusCode(500, "Usuario o contraseña incorrecta.");
        //}

        //[HttpPost]
        //[Route("register")]
        //[ProducesResponseType(typeof(LoginResponse), 200)]
        //[ProducesResponseType(500)]
        //public async Task<object> Register([FromBody] RegisterModel model)
        //{
        //    try
        //    {
        //        var user = new IdentityUser
        //        {
        //            UserName = model.Email,
        //            Email = model.Email
        //        };

        //        //if (model.Nombres == null || model.Nombres.Length < 3)
        //        //    model.Nombres = "Nombres";

        //        //if (model.Apellidos == null || model.Apellidos.Length < 3)
        //        //    model.Apellidos = "Apellidos";

        //        //if (model.Documento == null || model.Documento.Length < 3)
        //        //    model.Documento = "Documento";

        //        //if (model.Apellidos.Length < 3 || model.Nombres.Length < 3 || model.Documento.Length < 3)
        //        //    return StatusCode(500, "El nombre, apellido y documento del usuario tienen que tener mas de 3 caracteres.");

        //        var result = await _userManager.CreateAsync(user, model.Password);

        //        if (result.Succeeded)
        //        {
        //            List<string> l = new List<string>();
        //            l.Add("USER");
        //            await _userManager.AddToRolesAsync(user, l);

        //            await _signInManager.SignInAsync(user, false);
        //            var aux = GenerateJwtTokenAsync(model.Email, user);

        //            return new LoginResponse(aux.Result.ToString(), model.Email);
        //        }
        //        else
        //        {
        //            string errors = "";
        //            result.Errors.ToList().ForEach(x => {
        //                errors += (x.Description + "|");
        //            });
        //            return StatusCode(500, "Error no contralado al crear el usuario: " + errors);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }
        //}

        //private async Task<object> GenerateJwtTokenAsync(string username, IdentityUser user)
        //{
        //    var claims = new List<Claim>
        //    {
        //        new Claim(JwtRegisteredClaimNames.Sub, username),
        //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //        new Claim(ClaimTypes.NameIdentifier, user.Id),
        //    };

        //    var roles = await _userManager.GetRolesAsync(user);

        //    foreach (string r in roles)
        //    {
        //        claims.Add(new Claim(ClaimTypes.Role, r));
        //    }

        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        //    var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtExpireDays"]));

        //    var token = new JwtSecurityToken(
        //        _configuration["JwtIssuer"],
        //        _configuration["JwtIssuer"],
        //        claims,
        //        expires: expires,
        //        signingCredentials: creds
        //    );

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthenticateController(UserManager<ApplicationUser> userManager, IConfiguration configuration, RoleManager<IdentityRole> rolManager)
        {
            _userManager = userManager;
            _configuration = configuration;
            _roleManager = rolManager;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JwtIssuer"],
                    audience: _configuration["JwtIssuer"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                string error = "";
                if (result.Errors.ToList().Count() > 0)
                    error = result.Errors.ToList()[0].Description;
                if (result.Errors.ToList().Count() > 1)
                    error += " | " + result.Errors.ToList()[1].Description;
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again. " + error });
            }                

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        [HttpGet]
        [Route("roles")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetRoles()
        {
            List<string> roles = _roleManager.Roles.Select(x => x.Name).ToList();
            return Ok(roles);
        }

        [HttpPost]
        [Route("roles")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> AddRole([FromBody] Role x)
        {
            if (!User.Identity.IsAuthenticated || User.Identity.Name != "admin")
                throw new UnauthorizedAccessException("Only admin user can access.");

            var result = await _roleManager.CreateAsync(new IdentityRole(x.Nombre));

            if (result.Succeeded)
                return Ok();
            else
                throw new ApplicationException(result.Errors.ToString());
        }

        [HttpGet]
        [Route("users")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetUsers()
        {
            if (!User.Identity.IsAuthenticated || User.Identity.Name != "admin")
                throw new UnauthorizedAccessException("Only admin user can access.");

            List<ApplicationUser> users = _userManager.Users.ToList();
            return Ok(users);
        }

        [HttpPost]
        [Route("users-role")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> AddRoleToUser([FromBody] AddUserModel x)
        {
            if (!User.Identity.IsAuthenticated || User.Identity.Name != "admin")
                throw new UnauthorizedAccessException("Only admin user can access.");

            var user = await _userManager.FindByNameAsync(x.UserName);
            var result = await _userManager.AddToRoleAsync(user, x.Role);

            if (result.Succeeded)
                return Ok();
            else
                throw new ApplicationException(result.Errors.ToString());
        }

        [HttpGet]
        [Route("users-role")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetUserRoles(string username)
        {
            if (!User.Identity.IsAuthenticated || User.Identity.Name != "admin")
                throw new UnauthorizedAccessException("Only admin user can access.");
            var user = await _userManager.FindByNameAsync(username);
            return Ok(await _userManager.GetRolesAsync(user));
        }

        [HttpGet]
        [Route("user-info")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetUserInfo()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            UserDTO dto = new UserDTO();
            dto.Id = user.Id;
            dto.Nombre = user.Nombre;
            dto.Username = user.UserName;
            dto.Fotos = user.Foto;
            dto.Email = user.Email;
            dto.Roles = ((List<string>)await _userManager.GetRolesAsync(user));
            return Ok(dto);
        }
    }
}
