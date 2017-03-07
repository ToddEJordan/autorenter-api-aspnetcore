using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoRenter.Api.Data;
using MediatR;

namespace AutoRenter.Api.Features.Sku
{
    public class GetAll
    {
        public class Query : IAsyncRequest<Model>
        {
        }

        public class Model
        {
            public List<Sku> Skus { get; set; }

            public class Sku 
            {
                public Guid Id { get; set; }
                public string MakeId { get; set; }
                public string ModelId { get; set; }
                public int Year { get; set; }
                public string Color { get; set; }
            }
        }

        public class Hanlder : IAsyncRequestHandler<Query, Model>
        {
            private readonly ISkuRepository _skuRepository;

            public Hanlder(ISkuRepository skuRepository)
            {
                _skuRepository = skuRepository;
            }

            public async Task<Model> Handle(Query message)
            {
                var skus = await Task.Run(() => _skuRepository.GetAll()
                    .OrderBy(s => s.MakeId)
                    .ToList());

                var viewModel = new Model
                {
                    Skus = Mapper.Map<List<Model.Sku>>(skus)
                };

                return viewModel;
            }
        }
    }
}