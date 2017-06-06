using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoRenter.Api.Models;
using AutoRenter.Api.Services;
using AutoRenter.Domain.Interfaces;
using AutoRenter.Domain.Models;

namespace AutoRenter.Api.Controllers
{
    [Route("api/skus")]
    public class SkusController : Controller
    {
        private readonly ISkuService skuService;
        private readonly IResultCodeProcessor resultCodeProcessor;
        private readonly IDataStructureConverter dataStructureConverter;

        public SkusController(ISkuService skuService, IResultCodeProcessor resultCodeProcessor, IDataStructureConverter dataStructureConverter)
        {
            this.skuService = skuService;
            this.resultCodeProcessor = resultCodeProcessor;
            this.dataStructureConverter = dataStructureConverter;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var result = await skuService.GetAll();

            if (result.ResultCode == ResultCode.Success)
            {
                Response.Headers.Add("x-total-count", result.Data.Count().ToString());
                var formattedResult = dataStructureConverter
                    .FormatAndMap<IEnumerable<SkuModel>, IEnumerable<Sku>>("skus", result.Data);
                return Ok(formattedResult);
            }

            return resultCodeProcessor.Process(result.ResultCode);
        }
    }
}