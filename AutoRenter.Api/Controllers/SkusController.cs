using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IResponseFormatter responseFormatter;

        public SkusController(ISkuService skuService, IResultCodeProcessor resultCodeProcessor, IResponseFormatter responseFormatter)
        {
            this.skuService = skuService;
            this.resultCodeProcessor = resultCodeProcessor;
            this.responseFormatter = responseFormatter;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var result = await skuService.GetAll();
            if (result.ResultCode == ResultCode.Success)
            {
                Response.Headers.Add("x-total-count", result.Data.Count().ToString());
                var formattedResult = responseFormatter.Format("skus", result.Data);
                return Ok(formattedResult);
            }

            return resultCodeProcessor.Process(result.ResultCode);
        }
    }
}