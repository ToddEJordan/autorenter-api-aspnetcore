using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoMapper;
using AutoRenter.Api.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoRenter.Api.Features.Sku
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
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            var query = new GetAll.Query();
            var model = await _mediator.SendAsync(query);
            var formattedResult = new Dictionary<string, object>();
            formattedResult.Add("skus", model.Skus);

            Response.Headers.Add("x-total-count", model.Skus.ToString());
            return Ok(formattedResult);
        }
    }
}