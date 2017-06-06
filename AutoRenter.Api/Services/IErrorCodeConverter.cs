using Microsoft.AspNetCore.Mvc;
using AutoRenter.Domain.Models;

namespace AutoRenter.Api.Services
{
    public interface IErrorCodeConverter
    {
        IActionResult Convert(ResultCode resultCode);
    }
}
