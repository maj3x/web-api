using AutoMapper;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Uyg.API.DTOs;
using Uyg.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Uyg.API.Helpers;

namespace Uyg.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly JwtHelper _jwtHelper;
        ResultDto result = new ResultDto();
        public UserController(UserManager<AppUser> userManager, IMapper mapper, RoleManager<IdentityRole> roleManager, IConfiguration configuration, SignInManager<AppUser> signInManager, IWebHostEnvironment webHostEnvironment, JwtHelper jwtHelper)
        {
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
            _configuration = configuration;
            _signInManager = signInManager;
            _webHostEnvironment = webHostEnvironment;
            _jwtHelper = jwtHelper;
        }

        [HttpGet]
        [Authorize]
        public List<UserDto> List()
        {
            var users = _userManager.Users.ToList();
            var userDtos = _mapper.Map<List<UserDto>>(users);
            return userDtos;
        }
        [HttpGet("{id}")]
        [Authorize]
        public UserDto GetById(string id)
        {
            var user = _userManager.Users.Where(s => s.Id == id).SingleOrDefault();
            var userDto = _mapper.Map<UserDto>(user);
            return userDto;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new AppUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber,
                PhotoUrl = "profil.jpg"  // Default profile image
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // Create roles if they don't exist
            if (!await _roleManager.RoleExistsAsync("Admin"))
                await _roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
            if (!await _roleManager.RoleExistsAsync("Editor"))
                await _roleManager.CreateAsync(new IdentityRole { Name = "Editor" });
            if (!await _roleManager.RoleExistsAsync("User"))
                await _roleManager.CreateAsync(new IdentityRole { Name = "User" });

            // Assign role based on email domain (you can modify this logic)
            if (model.Email.EndsWith("@admin.com"))
                await _userManager.AddToRoleAsync(user, "Admin");
            else if (model.Email.EndsWith("@editor.com"))
                await _userManager.AddToRoleAsync(user, "Editor");
            else
                await _userManager.AddToRoleAsync(user, "User");

            return Ok(new { message = "User registered successfully" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
                return Unauthorized(new { message = "Invalid username or password" });

            var result = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!result)
                return Unauthorized(new { message = "Invalid username or password" });

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtHelper.GenerateToken(user, roles);

            return Ok(new
            {
                token = token,
                expiration = DateTime.Now.AddMinutes(Convert.ToDouble(60)),
                user = new
                {
                    id = user.Id,
                    userName = user.UserName,
                    email = user.Email,
                    fullName = user.FullName,
                    roles = roles
                }
            });
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
                return NotFound();

            var roles = await _userManager.GetRolesAsync(user);

            return Ok(new
            {
                id = user.Id,
                userName = user.UserName,
                email = user.Email,
                fullName = user.FullName,
                phoneNumber = user.PhoneNumber,
                roles = roles
            });
        }

        [Authorize]
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto model)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
                return NotFound();

            user.FullName = model.FullName;
            user.PhoneNumber = model.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new { message = "Profile updated successfully" });
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
                return NotFound();

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new { message = "Password changed successfully" });
        }

        [HttpPost]
        [Authorize]
        public async Task<ResultDto> Upload(UploadDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);
            if (user == null)
            {
                result.Status = false;
                result.Message = "Kayıt Bulunmadı!";
                return result;
            }

            var path = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot/Files/UserPhotos");
            string userPic = user.PhotoUrl;

            if (userPic != "profil.jpg")
            {

                var userPicUrl = Path.Combine(path, userPic);

                if (System.IO.File.Exists(userPicUrl))
                {
                    System.IO.File.Delete(userPicUrl);
                }
            }
            string data = dto.PicData;
            string base64 = data.Substring(data.IndexOf(',') + 1);
            base64 = base64.Trim('\0');
            byte[] imageBytes = Convert.FromBase64String(base64);
            string filePath = Guid.NewGuid().ToString() + dto.PicExt;


            var picPath = Path.Combine(path, filePath);

            System.IO.File.WriteAllBytes(picPath, imageBytes);

            user.PhotoUrl = filePath;
            await _userManager.UpdateAsync(user);

            result.Status = true;
            result.Message = "Profil Fotoğrafı Güncellendi";

            return result;
        }

    }
}

