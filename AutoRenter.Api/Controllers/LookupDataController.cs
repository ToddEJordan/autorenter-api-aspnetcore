using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoRenter.Api.Models;
using AutoRenter.Domain.Interfaces;
using AutoRenter.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoRenter.Api.Controllers
{
    [Route("api/lookup-data")]
    public class LookupDataController : Controller
    {
        private readonly IMapper mapper;
        private readonly IMakeService makeService;
        private readonly IModelService modelService;
        private readonly IStateService stateService;

        public LookupDataController(IMapper mapper, IMakeService makeService, IModelService modelService, IStateService stateService)
        {
            this.mapper = mapper;
            this.makeService = makeService;
            this.modelService = modelService;
            this.stateService = stateService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<dynamic> Get()
        {
            var query = Request.Query;
            var lookupData = await GetData(query);
            var formattedResult = new Dictionary<string, object>
            {
                {"lookupData", lookupData}
            };

            Response.Headers.Add("x-total-count", lookupData.Count.ToString());
            return Ok(formattedResult);
        }

        private async Task<ConcurrentDictionary<string, object>> GetData(IQueryCollection lookupTypes)
        {
            ConcurrentDictionary<string, object> lookupData = new ConcurrentDictionary<string, object>();

            if (lookupTypes.ContainsKey("makes"))
            {
                await GetMakes(lookupData);
            }

            if (lookupTypes.ContainsKey("models"))
            {
                await GetModels(lookupData);
            }

            if (lookupTypes.ContainsKey("states"))
            {
                await GetStates(lookupData);
            }

            return lookupData;
        }

        private async Task GetStates(ConcurrentDictionary<string, object> lookupData)
        {
            var statesResult = await stateService.GetAll();
            if (statesResult.ResultCode == ResultCode.Success)
            {
                var data = statesResult.Data.OrderBy(x => x.StateCode).ToList();
                lookupData.AddOrUpdate("states", data,
                    (key, oldValue) => data);
            }
        }

        private async Task GetModels(ConcurrentDictionary<string, object> lookupData)
        {
            var modelsResult = await modelService.GetAll();
            if (modelsResult.ResultCode == ResultCode.Success)
            {
                var data = modelsResult.Data
                    .Select(model => mapper.Map<ModelModel>(model))
                    .OrderBy(x => x.Id)
                    .ToList();
                lookupData.AddOrUpdate("models", data,
                    (key, oldValue) => data);
            }
        }

        private async Task GetMakes(ConcurrentDictionary<string, object> lookupData)
        {
            var makesResult = await makeService.GetAll();
            if (makesResult.ResultCode == ResultCode.Success)
            {
                var data = makesResult.Data
                    .Select(make => mapper.Map<MakeModel>(make))
                    .OrderBy(x => x.Id)
                    .ToList();
                lookupData.AddOrUpdate("makes", data,
                        (key, oldValue) => data);
            }
        }
    }
}
