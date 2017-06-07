using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoRenter.Api.Models;
using AutoRenter.Api.Services;

namespace AutoRenter.Api.Controllers
{
    [Route("api/")]
    public class InfoController : Controller
    {
        private readonly IDataStructureConverter dataStructureConverter;

        public InfoController(IDataStructureConverter dataStructureConverter)
        {
            this.dataStructureConverter = dataStructureConverter;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            var apiInfo = GetApiInfo();
            var formattedResult = dataStructureConverter.Convert("data", apiInfo);
            return await Task.FromResult(Ok(formattedResult));
        }

        private ApiInfoModel GetApiInfo()
        {
            return new ApiInfoModel
            {
                Title = "AutoRenter API",
                Environment = GetEnvironment(),
                Version = GetVersion(),
                Build = GetBuild()
            };
        }

        private string GetEnvironment()
        {
            var version = Microsoft.Extensions.PlatformAbstractions
                .PlatformServices.Default.Application.RuntimeFramework
                .Version;

            var name = Microsoft.Extensions.PlatformAbstractions
                .PlatformServices.Default.Application.RuntimeFramework
                .FullName;

            return $"{name} {version}";
        }

        private string GetVersion()
        {
            return Microsoft.Extensions.PlatformAbstractions
                .PlatformServices.Default.Application
                .ApplicationVersion;
        }

        private string GetBuild()
        {
            return "not-implemented";
        }
    }
}
