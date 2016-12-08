using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoRenter.API.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AutoRenter.API.Services
{
    public class ResponseConverter : IResponseConverter
    {
        public string Convert(dynamic data)
        {
            var responseEvelope = new ResponseEnvelope { Data = data };
            var formattedResult = JsonConvert.SerializeObject(responseEvelope, Formatting.Indented, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            return formattedResult;
        }
    }
}
