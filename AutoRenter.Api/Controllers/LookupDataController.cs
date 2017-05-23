using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly IMakeService makeService;
        private readonly IModelService modelService;
        private readonly IStateService stateService;

        public LookupDataController(IMakeService makeService, IModelService modelService, IStateService stateService)
        {
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

        private async Task<Dictionary<string, object>> GetData(IQueryCollection lookupTypes)
        {
            Dictionary<string, object> lookupData = new Dictionary<string, object>();
            if (lookupTypes.ContainsKey("makes"))
            {
                var makesResult = await makeService.GetAll();
                if (makesResult.ResultCode == ResultCode.Success)
                {
                    lookupData.Add("makes", makesResult.Data
                        .ToList()
                        .OrderBy(x => x.ExternalId)
                    );
                }
            }

            if (lookupTypes.ContainsKey("models"))
            {
                var modelsResult = await modelService.GetAll();
                if (modelsResult.ResultCode == ResultCode.Success)
                {
                    lookupData.Add("models", modelsResult.Data
                        .ToList()
                        .OrderBy(x => x.ExternalId)
                    );
                }
                
            }

            if (lookupTypes.ContainsKey("states"))
            {
                var statesResult = await stateService.GetAll();
                if (statesResult.ResultCode == ResultCode.Success)
                {
                    lookupData.Add("states", statesResult.Data
                        .ToList()
                        .OrderBy(x => x.StateCode)
                    );
                }
            }

            return lookupData;
        }
    }
}
