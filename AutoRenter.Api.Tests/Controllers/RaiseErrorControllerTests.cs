using System;
using Xunit;
using AutoRenter.Api.Controllers;

namespace AutoRenter.Api.Tests.Controllers
{
    public class RaiseErrorControllerTests
    {
        public void Get_RaisesError()
        {
            // arrange
            var sut = new RaiseErrorController();

            // act & assert
            Assert.ThrowsAny<Exception>(() => sut.Get());
        }
    }
}
