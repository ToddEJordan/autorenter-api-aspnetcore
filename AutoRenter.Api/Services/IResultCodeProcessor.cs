using Microsoft.AspNetCore.Mvc;
using AutoRenter.Domain.Models;

namespace AutoRenter.Api.Services
{
    public interface IResultCodeProcessor
    {
        IActionResult Process(ResultCode resultCode);
    }
}
