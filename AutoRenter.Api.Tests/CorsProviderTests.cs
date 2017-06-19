using System;
using System.Linq;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Xunit;

namespace AutoRenter.Api.Tests
{
    public class CorsProviderTests
    {
        [Fact]
        public void DevMode_DisallowsAnyOrigin()
        {
            // arrange
            var sut = new CorsProvider(true, new string[0]);

            // act
            var result = sut.GetCorsPolicy();

            // assert
            Assert.False(result.AllowAnyOrigin);
        }

        [Theory]
        [InlineData("GET")]
        [InlineData("POST")]
        [InlineData("PUT")]
        [InlineData("DELETE")]
        [InlineData("OPTIONS")]
        public void DevMode_HasExpectedMethods(string expectedMethod)
        {
            // arrange
            var sut = new CorsProvider(true, new string[0]);

            // act
            var result = sut.GetCorsPolicy();

            // assert
            Assert.True(result.Methods.Contains(expectedMethod));
        }

        [Fact]
        public void DevMode_AllowsAnyHeader()
        {
            // arrange
            var sut = new CorsProvider(true, new string[0]);

            // act
            var result = sut.GetCorsPolicy();

            // assert
            Assert.True(result.AllowAnyHeader);
        }

        [Fact]
        public void DevMode_DisallowsAnyMethod()
        {
            // arrange
            var origins = new[] { "http://www.fusionalliance.com" };
            var sut = new CorsProvider(true, origins);

            // act
            var result = sut.GetCorsPolicy();

            // assert
            Assert.False(result.AllowAnyMethod);
        }

        [Fact]
        public void DevMode_DoesNotSupportsCredentials()
        {
            // arrange
            var sut = new CorsProvider(true, new string[0]);

            // act
            var result = sut.GetCorsPolicy();

            // assert
            Assert.False(result.SupportsCredentials);
        }

        [Theory]
        [InlineData("x-total-count")]
        public void DevMode_HasExpectedExposedHeaders(string expectedMethod)
        {
            // arrange
            var sut = new CorsProvider(true, new string[0]);

            // act
            var result = sut.GetCorsPolicy();

            // assert
            Assert.True(result.ExposedHeaders.Contains(expectedMethod));
        }

        [Fact]
        public void DevMode_GetsPolicy()
        {
            // arrange
            var origins = new[] { "http://www.fusionalliance.com" };
            var sut = new CorsProvider(true, origins);

            // act
            var result = sut.GetCorsPolicy();

            // assert
            Assert.IsType(typeof(CorsPolicy), result);
        }

        [Theory]
        [InlineData("http://localhost:8080")]
        [InlineData("http://127.0.0.1:8080")]
        public void DevMode_AllowsLocalHostWhenNotProvided(string expectedOrigin)
        {
            // arrange
            var sut = new CorsProvider(true, new string[0]);

            // act
            var result = sut.GetCorsPolicy();

            // assert
            Assert.True(result.Origins.Contains(expectedOrigin));
        }

        [Fact]
        public void DevMode_SetsPreflightMaxAge()
        {
            // arrange
            var expected = TimeSpan.FromSeconds(5);
            var origins = new[] { "http://www.fusionalliance.com" };
            var sut = new CorsProvider(true, origins);

            // act
            var result = sut.GetCorsPolicy();

            // assert
            Assert.Equal(expected, result.PreflightMaxAge);
        }

        [Fact]
        public void ProdMode_ExposesTotalCountHeader()
        {
            // arrange
            var origins = new[] { "http://www.fusionalliance.com" };
            var sut = new CorsProvider(false, origins);

            // act
            var result = sut.GetCorsPolicy();

            // assert
            Assert.True(result.ExposedHeaders.Contains("x-total-count"));
        }

        [Theory]
        [InlineData("GET")]
        [InlineData("POST")]
        [InlineData("PUT")]
        [InlineData("DELETE")]
        [InlineData("OPTIONS")]
        public void ProdMode_HasExpectedMethods(string expectedMethod)
        {
            // arrange
            var sut = new CorsProvider(false, new string[0]);

            // act
            var result = sut.GetCorsPolicy();

            // assert
            Assert.True(result.Methods.Contains(expectedMethod));
        }

        [Theory]
        [InlineData("x-total-count")]
        public void ProdMode_HasExpectedExposedHeaders(string expectedHeader)
        {
            // arrange
            var sut = new CorsProvider(false, new string[0]);

            // act
            var result = sut.GetCorsPolicy();

            // assert
            Assert.True(result.ExposedHeaders.Contains(expectedHeader));
        }

        [Fact]
        public void ProdMode_DisallowsAnyOrigin()
        {
            // arrange
            var origins = new[] { "http://www.fusionalliance.com" };
            var sut = new CorsProvider(false, origins);

            // act
            var result = sut.GetCorsPolicy();

            // assert
            Assert.False(result.AllowAnyOrigin);
        }

        [Fact]
        public void ProdMode_DisallowsAnyMethod()
        {
            // arrange
            var origins = new[] { "http://www.fusionalliance.com" };
            var sut = new CorsProvider(false, origins);

            // act
            var result = sut.GetCorsPolicy();

            // assert
            Assert.False(result.AllowAnyMethod);
        }

        [Fact]
        public void ProdMode_NotSupportCredentials()
        {
            // arrange
            var origins = new[] { "http://www.fusionalliance.com" };
            var sut = new CorsProvider(false, origins);

            // act
            var result = sut.GetCorsPolicy();

            // assert
            Assert.False(result.SupportsCredentials);
        }

        [Theory]
        [InlineData("http://www.fusionalliance.com")]
        [InlineData("http://www.fusionalliance.com, http://fusionalliance.com")]
        public void ProdMode_SetsOrigins(string data)
        {
            // arrange
            var origins = data.Split(',');
            var sut = new CorsProvider(false, origins);

            // act
            var result = sut.GetCorsPolicy();

            // assert
            foreach (var o in origins)
            {
                Assert.True(result.Origins.Contains(o));
            }
        }

        [Fact]
        public void ProdMode_SetsPreflightMaxAge()
        {
            // arrange
            var expected = TimeSpan.FromMinutes(10);
            var origins = new[] { "http://www.fusionalliance.com" };
            var sut = new CorsProvider(false, origins);

            // act
            var result = sut.GetCorsPolicy();

            // assert
            Assert.Equal(expected, result.PreflightMaxAge);
        }

        [Fact]
        public void ProdMode_GetsPolicy()
        {
            // arrange
            var origins = new[] { "http://www.fusionalliance.com" };
            var sut = new CorsProvider(false, origins);

            // act
            var result = sut.GetCorsPolicy();

            // assert
            Assert.IsType(typeof(CorsPolicy), result);
        }

        [Fact]
        public void ProdMode_AllowsNoOriginsWhenNotProvided()
        {
            // arrange
            var sut = new CorsProvider(false, new string[0]);

            // act
            var result = sut.GetCorsPolicy();

            // assert
            Assert.False(result.Origins.Any());
        }

        [Fact]
        public void ProdMode_AllowsAnyHeader()
        {
            // arrange
            var sut = new CorsProvider(false, new string[0]);

            // act
            var result = sut.GetCorsPolicy();

            // assert
            Assert.True(result.AllowAnyHeader);
        }
    }
}
