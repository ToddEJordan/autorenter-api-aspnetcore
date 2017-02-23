using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoMapper;
using AutoRenter.API.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AutoRenter.API.Features.Sku
{
    [Route("api/skus")]
    public class SkusController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IResponseConverter _responseConverter;

        public SkusController(IResponseConverter responseConverter, IMediator mediator)
        {
            _responseConverter = responseConverter;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var query = new GetAll.Query();
            var model = await _mediator.SendAsync(query);
            var formattedResult = _responseConverter.Convert(model);

            Response.Headers.Add("x-total-count", model.Skus.ToString());
            return Ok(formattedResult);
        }
    }
}