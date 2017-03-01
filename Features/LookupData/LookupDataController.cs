using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoMapper;
using AutoRenter.API.Domain;
using AutoRenter.API.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AutoRenter.API.Features.LookupData
{
    [Route("api/lookup-data")]
    public class LookupDataController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IResponseConverter _responseConverter;

        public LookupDataController(IResponseConverter responseConverter, IMediator mediator)
        {
            _responseConverter = responseConverter;
            _mediator = mediator;
        }

        // [HttpGet]
        // public async Task<IActionResult> Get()
        // {
        //     var query = new GetAll.Query();
        //     var model = await _mediator.SendAsync(query);
        //     var formattedResult = _responseConverter.Convert(model);

        //     Response.Headers.Add("x-total-count", model.LookupData.ToString());
        //     return Ok(formattedResult);
        // }

        [HttpGet]
        public dynamic Get([FromQueryAttribute] Boolean makes, [FromQueryAttribute] Boolean models, [FromQueryAttribute] Boolean states)
        {
            var data = GetData(makes, models, states);
            var formattedResult = _responseConverter.Convert(data);

            Response.Headers.Add("x-total-count", data.ToString());
            return Ok(formattedResult);
        }

        private Domain.LookupData GetData(Boolean makes, Boolean models, Boolean states)
        {
            var lookupData = new Domain.LookupData();
            if (makes) {
                ICollection<Make> makesData = new List<Make> {
                    new Make{Id = "tsl", Name = "Tesla"},
                    new Make{Id = "che", Name = "Chevrolet"},
                    new Make{Id = "frd", Name = "Ford"}
                };
                lookupData.Makes = makesData;
            }

            if (models) {
                ICollection<Model> modelsData = new List<Model> {
                    new Model{Id = "tms", Name = "Model S"},
                    new Model{Id = "tmx", Name = "Model X"},
                    new Model{Id = "cvt", Name = "Corvette"},
                    new Model{Id = "fxp", Name = "Explorer"},
                    new Model{Id = "fta", Name = "Taurus"}
                };
                lookupData.Models = modelsData;
            }

            if (states) {
                ICollection<State> statesData = new List<State> {
                    new State{StateCode = "AZ", Name = "Arizona"},
                    new State{StateCode = "CA", Name = "California"},
                    new State{StateCode = "HI", Name = "Hawaii"},
                    new State{StateCode = "IN", Name = "Indiana"},
                    new State{StateCode = "WA", Name = "Washington"}
                };
                lookupData.States = statesData;
            }

            return lookupData;
        }
    }
}