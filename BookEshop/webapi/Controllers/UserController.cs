﻿using Microsoft.AspNetCore.Mvc;
using webapi.Models;
using webapi.Models.ResponseModels;
using webapi.Service;

namespace webapi.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly ISqlService SqlService;

        public UserController(ISqlService varSqlService)
        {
            this.SqlService = varSqlService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login userInfo)
        {
            try
            {
                var result = await SqlService.Login(userInfo);
                return Ok(result);
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Register registerObj)
        {
            try
            {
                var result = await SqlService.Register(registerObj);
                return Ok(result);
            } catch (Exception ex) {
                throw new CustomException(StatusCodes.Status500InternalServerError, $"Nieco sa dogabalo: {ex}");
            }  
        }
    }
}
