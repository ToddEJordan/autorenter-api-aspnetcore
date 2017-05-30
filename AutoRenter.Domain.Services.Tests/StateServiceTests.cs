using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Xunit;
using AutoRenter.Api.Data;
using AutoRenter.Domain.Interfaces;
using AutoRenter.Domain.Models;
using AutoRenter.Domain.Services.Commands;

namespace AutoRenter.Domain.Services.Tests
{
    public class StateServiceTests : IDisposable
    {
        AutoRenterContext context;

        public StateServiceTests()
        {
            // xUnit.net creates a new instance of the test class for every test that is run.
            // The constructor and the Dispose method will always be called once for every test.
            context = TestableContextFactory.GenerateContext();
        }

        public void Dispose()
        {
            context.Dispose();
        }

        [Fact]
        public async void GetAll_ReturnsData()
        {
            // arrange
            ICommandFactory<State> commandFactory = new CommandFactory<State>();
            var sut = new StateService(context, commandFactory);

            // act
            var result = await sut.GetAll();

            // assert
            Assert.NotEmpty(result.Data);
        }

        [Fact]
        public async void GetAll_WhenNotFoundReturnsNotFound()
        {
            // arrange
            var allState = await context.Set<State>().ToListAsync();

            ICommandFactory<State> commandFactory = new CommandFactory<State>();

            var sut = new StateService(context, commandFactory);

            context.States.RemoveRange(allState);
            await context.SaveChangesAsync();

            // act
            var result = await sut.GetAll();

            // assert
            Assert.Equal(ResultCode.NotFound, result.ResultCode);
        }

        [Fact]
        public async void Get_ReturnsData()
        {
            // arrange
            var targetId = context.States.FirstOrDefault().Id;
            ICommandFactory<State> commandFactory = new CommandFactory<State>();

            var sut = new StateService(context, commandFactory);

            // act
            var result = await sut.Get(targetId);

            // assert
            Assert.NotNull(result.Data);
        }

        [Fact]
        public async void Get_WhenNotFoundReturnsNotFound()
        {
            // arrange
            var targetId = context.States.FirstOrDefault().Id;
            var targetEntity = await context.FindAsync<State>(targetId);

            ICommandFactory<State> commandFactory = new CommandFactory<State>();

            var sut = new StateService(context, commandFactory);

            var removeResult = context.Remove(targetEntity);
            await context.SaveChangesAsync();

            // act
            var result = await sut.Get(targetId);

            // assert
            Assert.Equal(ResultCode.NotFound, result.ResultCode);
        }
    }
}
