using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoRenter.Api.Data;
using AutoRenter.Api.Domain;
using AutoRenter.Api.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace AutoRenter.Api.Features.LookupData
{
    [Route("api/lookup-data")]
    public class LookupDataController : Controller
    {
        private readonly AutoRenterContext _context;
        private readonly IMediator _mediator;
        private readonly IResponseConverter _responseConverter;

        public LookupDataController(AutoRenterContext context, IResponseConverter responseConverter, IMediator mediator)
        {
            _context = context;
            _responseConverter = responseConverter;
            _mediator = mediator;
        }

        [HttpGet]
        [AllowAnonymous]
        public dynamic Get()
        {
            var query = Request.Query;
            var lookupData = GetData(query);
            var formattedResult = new Dictionary<string, object>();
            formattedResult.Add("lookupData", lookupData);

            Response.Headers.Add("x-total-count", lookupData.ToString());
            return Ok(formattedResult);
        }

        private Dictionary<string,object> GetData(IQueryCollection lookupTypes)
        {
            Dictionary<string,object> lookupData = new Dictionary<string, object>();
            if (lookupTypes.ContainsKey("makes")) {
                ICollection<Make> makesData = _context.Makes.AsEnumerable()
                    .OrderBy(s => s.Id)
                    .ToList();
                lookupData.Add("makes", makesData);
            }

            if (lookupTypes.ContainsKey("models")) {
                ICollection<Model> modelsData = _context.Models.AsEnumerable()
                    .OrderBy(s => s.Id)
                    .ToList();
                lookupData.Add("models", modelsData);
            }

            if (lookupTypes.ContainsKey("states")) {
                ICollection<State> statesData = _context.States.AsEnumerable()
                    .OrderBy(s => s.StateCode)
                    .ToList();
                lookupData.Add("states", statesData);
            }

            return lookupData;
        }
    }
}