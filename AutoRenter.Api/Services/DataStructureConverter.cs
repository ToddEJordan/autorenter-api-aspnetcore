using System.Collections.Generic;
using System.Linq;
using AutoMapper;

namespace AutoRenter.Api.Services
{
    public class DataStructureConverter : IDataStructureConverter
    {
        private readonly IMapper mapper;
        public DataStructureConverter(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public Dictionary<string, object> Format(string contentLabel, object data)
        {
            return new Dictionary<string, object>
            {
                { contentLabel, data }
            };
        }

        public TDest Map<TDest, TSource>(TSource source)
        {
            return mapper.Map<TDest>(source);
        }

        public IEnumerable<TDest> Map<TDest, TSource>(IEnumerable<TSource> sources)
        {
            return sources
                .Select(source => Map<TDest, TSource>(source))
                .ToList();
        }

        public Dictionary<string, object> FormatAndMap<TDest, TSource>(string contentLabel, TSource source)
        {
            var data = Map<TDest, TSource>(source);
            return Format(contentLabel, data);
        }

        public Dictionary<string, object> FormatAndMap<TDest, TSource>(string contentLabel, IEnumerable<TSource> sources)
        {
            var datas = Map<TDest, TSource>(sources);
            return Format(contentLabel, datas);
        }
    }
}
