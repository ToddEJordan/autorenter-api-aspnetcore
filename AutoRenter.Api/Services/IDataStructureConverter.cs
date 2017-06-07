using System.Collections.Generic;

namespace AutoRenter.Api.Services
{
    public interface IDataStructureConverter
    {
        Dictionary<string, object> Convert(string contentLabel, object data);

        TDest Map<TDest, TSource>(TSource source);
        IEnumerable<TDest> Map<TDest, TSource>(IEnumerable<TSource> sources);
        Dictionary<string, object> ConvertAndMap<TDest, TSource>(string contentLabel, TSource source);
        Dictionary<string, object> ConvertAndMap<TDest, TSource>(string contentLabel, IEnumerable<TSource> sources);
    }
}
