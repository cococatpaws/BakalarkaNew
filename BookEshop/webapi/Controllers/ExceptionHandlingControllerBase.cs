using Microsoft.AspNetCore.Mvc;
using System.Net;
using webapi.Models.ResponseModels;
using webapi.Models.UtilityModels;

namespace webapi.Controllers
{
    [ApiController]
    public class ExceptionHandlingControllerBase : ControllerBase
    {
        
        protected async Task<IActionResult> HandleExceptionAsync(Func<Task<IActionResult>> action)
        {
            try
            {
                return await action();
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.StatusCode, new ErrorResponse(ex.Message, ex.Details));
            }
            catch (NotImplementedException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError,
                    new ErrorResponse("Method not implemented.", null));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError,
                    new ErrorResponse("An unexpected error occurred.", null));
            }
        }

    }
}
