using Microsoft.AspNetCore.Mvc;
using webapi.Models.ResponseModels;
using webapi.Models.UtilityModels;
using webapi.Service;

namespace webapi.Controllers
{
    [ApiController]
    //[Route("api")]
    public class AuthController : ExceptionHandlingControllerBase
    {

        private readonly ISqlService SqlService;

        public AuthController(ISqlService varSqlService, IJwtService varJwtService)
        {
            this.SqlService = varSqlService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login userInfo)
        {
            return await HandleExceptionAsync(async () =>
            {
                var result = await SqlService.Login(userInfo);


                return Ok(new
                {
                    Token = result,
                    Message = "Úspešné prihlásenie!"
                });
            });
                
            
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
