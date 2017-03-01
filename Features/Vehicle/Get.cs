using System;
using System.Threading.Tasks;
using AutoMapper;
using AutoRenter.API.Data;
using FluentValidation;
using MediatR;

namespace AutoRenter.API.Features.Vehicle
{
    public class Get
    {
        public class Query : IAsyncRequest<QueryModel>
        {
            public Guid? Id { get; set; }
        }

        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(m => m.Id).NotNull();
            }
        }

        public class QueryModel
        {
            public Guid? Id { get; set; }
            public string Vin { get; set; }
            public string MakeId { get; set; }
            public string ModelId { get; set; }
            public int Year { get; set; }
            public int Miles { get; set; }
            public string Color { get; set; }
            public bool IsRentToOwn { get; set; }
            public string Image { get; set; }
            public Guid LocationId { get; set; }
        }

        public class Hanlder : IAsyncRequestHandler<Query, QueryModel>
        {
            private readonly IVehicleRepository _vehicleRepository;

            public Hanlder(IVehicleRepository vehicleRepository)
            {
                _vehicleRepository = vehicleRepository;
            }

            public async Task<QueryModel> Handle(Query message)
            {
                var vehicle =
                    await Task.Run(() => _vehicleRepository.GetSingle(s => s.Id.Equals(message.Id), s => s.Location));
                var viewModel = Mapper.Map<QueryModel>(vehicle);

                return viewModel;
            }
        }
    }
}