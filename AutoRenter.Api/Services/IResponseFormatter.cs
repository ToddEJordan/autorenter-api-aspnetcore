using System.Collections.Generic;
using AutoRenter.Domain.Models;

namespace AutoRenter.Api.Services
{
    public interface IResponseFormatter
    {
        Dictionary<string, object> Format(string contentLabel, object data);

        TDest Map<TDest, TSource>(TSource source);
        IEnumerable<TDest> Map<TDest, TSource>(IEnumerable<TSource> sources);
        Dictionary<string, object> FormatAndMap<TDest, TSource>(string contentLabel, TSource source);
        Dictionary<string, object> FormatAndMap<TDest, TSource>(string contentLabel, IEnumerable<TSource> sources);
    }
}
