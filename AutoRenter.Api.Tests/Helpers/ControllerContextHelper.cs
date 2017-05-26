using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AutoRenter.Api.Tests.Helpers
{
    internal class ControllerContextHelper
    {
        internal ControllerContext GetContext()
        {
            var headerDictionary = new HeaderDictionary();
            var response = new Mock<HttpResponse>();
            response.SetupGet(r => r.Headers).Returns(headerDictionary);

            var httpContext = new Mock<HttpContext>();
            httpContext.SetupGet(a => a.Response).Returns(response.Object);

            return new ControllerContext()
            {
                HttpContext = httpContext.Object
            };
        }
    }
}
