using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoRenter.API.Data;
using MediatR;

namespace AutoRenter.API.Features.LookupData
{
    public class GetMakes
    {
        public class Query : IAsyncRequest<Model>
        {
        }

        public class Model
        {
            public List<Make> Makes { get; set; }

            public class Make 
            {
                public string Id { get; set; }
                public string Name { get; set; }
            }
        }

        public class Handler : IAsyncRequestHandler<Query, Model>
        {
            private readonly AutoRenterContext _context;

            public Handler(AutoRenterContext context)
            {
                _context = context;
            }

            public async Task<Model> Handle(Query message)
            {
                var makes = await Task.Run(() => _context.Makes.AsEnumerable()
                    .OrderBy(s => s.Id)
                    .ToList());

                var viewModel = new Model
                {
                    Makes = Mapper.Map<List<Model.Make>>(makes)
                };

                return viewModel;
            }
        }
    }
}