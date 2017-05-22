using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using AutoRenter.Api.Services;
using AutoRenter.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;


namespace AutoRenter.Api.Tests
{
    public class ResultCodeProcessorTests
    {
        [Fact]
        public void NotFound()
        {
            // arrange
            var payload = ResultCode.NotFound;
            var sut = new ResultCodeProcessor();

            // act
            var result = sut.Process(payload);
            var notFoundResult = result as NotFoundResult;

            // assert
            Assert.NotNull(notFoundResult);
        }

        [Fact]
        public void Conflict()
        {
            // arrange
            var payload = ResultCode.Conflict;
            var expected = StatusCodes.Status409Conflict;
            var sut = new ResultCodeProcessor();

            // act
            var result = sut.Process(payload);
            var conflictResult = result as StatusCodeResult;
            
            // assert
            Assert.Equal(expected, conflictResult.StatusCode);
        }

        [Fact]
        public void BadRequest()
        {
            // arrange
            var payload = ResultCode.BadRequest;
            var expected = StatusCodes.Status400BadRequest;
            var sut = new ResultCodeProcessor();

            // act
            var result = sut.Process(payload);
            var conflictResult = result as StatusCodeResult;

            // assert
            Assert.Equal(expected, conflictResult.StatusCode);
        }

        [Theory]
        [InlineData(ResultCode.Unknown)]
        [InlineData(ResultCode.Failed)]
        public void InternalServerError(ResultCode value)
        {
            // arrange
            var expected = StatusCodes.Status500InternalServerError;
            var sut = new ResultCodeProcessor();

            // act
            var result = sut.Process(value);
            var conflictResult = result as StatusCodeResult;

            // assert
            Assert.Equal(expected, conflictResult.StatusCode);
        }
    }
}
