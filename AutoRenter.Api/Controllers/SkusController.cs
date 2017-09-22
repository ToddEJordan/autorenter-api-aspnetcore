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
        private readonly IErrorCodeConverter errorCodeConverter;
        private readonly IDataStructureConverter dataStructureConverter;

        public SkusController(ISkuService skuService, IErrorCodeConverter errorCodeConverter, IDataStructureConverter dataStructureConverter)
        {
            this.skuService = skuService;
            this.errorCodeConverter = errorCodeConverter;
            this.dataStructureConverter = dataStructureConverter;
        }

        [HttpGet]
        [Authorize(Policy = "RequireToken")]
        public async Task<IActionResult> GetAll()
        {
            var result = await skuService.GetAll();

            if (result.ResultCode == ResultCode.Success)
            {
                Response.Headers.Add("x-total-count", result.Data.Count().ToString());
                var formattedResult = dataStructureConverter
                    .ConvertAndMap<IEnumerable<SkuModel>, IEnumerable<Sku>>("skus", result.Data);
                return Ok(formattedResult);
            }

            return errorCodeConverter.Convert(result.ResultCode);
        }
    }
}