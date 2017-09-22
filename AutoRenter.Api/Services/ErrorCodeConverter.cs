using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoRenter.Domain.Models;

namespace AutoRenter.Api.Services
{
    public class ErrorCodeConverter : IErrorCodeConverter
    {
        public IActionResult Convert(ResultCode resultCode)
        {
            switch (resultCode)
            {
                case ResultCode.NotFound:
                    return new NotFoundResult();
                case ResultCode.Conflict:
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                case ResultCode.BadRequest:
                    return new BadRequestResult();
                case ResultCode.Unauthorized:
                    return new UnauthorizedResult();
                case ResultCode.Unknown:
                case ResultCode.Failed:
                default:
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
