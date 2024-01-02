using Microsoft.AspNetCore.Mvc;
using webapi.Models;
using webapi.Models.ResponseModels;
using webapi.Service;

namespace webapi.Controllers
{
    [ApiController]
    public class OrderController : ExceptionHandlingControllerBase
    {
        private readonly ISqlService SqlService;

        public OrderController(ISqlService varSqlService, IJwtService varJwtService)
        {
            this.SqlService = varSqlService;
        }

        [HttpPost("/order")]
        public async Task<IActionResult> Order([FromBody] OrderResponse order)
        {
            return await HandleExceptionAsync(async () =>
            {
                var result = await this.SqlService.Order(order);
                return Ok(result);
            });
        }
        
    }
}
