using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Quiz.App.Dtos;
using Quiz.App.Entities;
using Quiz.App.JWT;
using Quiz.App.ResponseModels;

namespace Quiz.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenHelper _tokenHelper;


        //DI
        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, ITokenHelper tokenHelper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenHelper = tokenHelper;
        }



        [HttpPost]
        [Route("[Action]")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var user = new User()
            {
                Email = registerDto.Email,
                UserName = registerDto.Email,
                Name = registerDto.Name,
                LastName = registerDto.LastName,
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (result.Succeeded)
            {
                return Ok(new ResponseModel(true,"Üye Olundu."));
            }

            return BadRequest(new ResponseModel(false, result.Errors.First().Description));
            
        }




        [HttpPost]
        [Route("[Action]")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == loginDto.Email);

            if (user != null)
            {
                
                var loginResult = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

                if (loginResult.Succeeded)
                {
                    return Ok(new DataResponseModel<AccessToken>(_tokenHelper.CreateToken(user), true, "Giriş Yapıldı."));
                }

                return BadRequest(new DataResponseModel<AccessToken>(false, "Şifre Yanlış."));
            }

            return BadRequest(new DataResponseModel<AccessToken>(false, "Kullanıcı Bulunamadı."));
        }

    }
}
