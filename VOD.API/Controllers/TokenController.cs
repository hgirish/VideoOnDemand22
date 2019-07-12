using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VOD.API.Services;
using VOD.Common.DTOModels;

namespace VOD.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        public TokenController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }
        [HttpPost]
        public async Task<ActionResult<TokenDTO>> GenerateTokenAsync(LoginUserDTO loginUserDTO)
        {
            try
            {
                var jwt = await _tokenService.GenerateTokenAsync(loginUserDTO);
                if (jwt.Token == null)
                {
                    return Unauthorized();
                }
                return jwt;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Unauthorized();
            }
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<TokenDTO>> GetTokenAsync(
            string userId,
            LoginUserDTO loginUserDTO)
        {
            try
            {
                var jwt = await _tokenService.GetTokenAsync(loginUserDTO,userId);
                if (jwt.Token == null)
                {
                    return Unauthorized();
                }
                return jwt;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Unauthorized();
            }
        }
    }
}
