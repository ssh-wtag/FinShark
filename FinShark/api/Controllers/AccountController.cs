﻿using Domain.DTOs.Account;
using Application.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName.ToLower() == loginDTO.Username.ToLower());

            if(user == null)
                return Unauthorized("User Does not Exist");

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);

            if (!result.Succeeded)
                return Unauthorized("Username and/or Password is Invalid");

            return Ok(
                new NewUserDTO
                {
                    Username = user.UserName,
                    Email = user.Email,
                    Token = _tokenService.CreateToken(user)
                }
            );
        }



        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            try
            {
                if(!ModelState.IsValid)
                    return BadRequest(ModelState);

                var appUser = new AppUser
                {
                    UserName = registerDTO.UserName,
                    Email = registerDTO.Email
                };

                var createdUser = await _userManager.CreateAsync(appUser, registerDTO.Password);

                if(createdUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                    if (roleResult.Succeeded)
                        return Ok(
                                new NewUserDTO
                                {
                                    Username = appUser.UserName,
                                    Email = appUser.Email,
                                    Token = _tokenService.CreateToken(appUser)
                                }
                            );
                    else
                        return StatusCode(500, roleResult.Errors);
                }
                else
                {
                    return StatusCode(500, createdUser.Errors);
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
