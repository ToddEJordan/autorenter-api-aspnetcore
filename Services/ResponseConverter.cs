using AutoRenter.API.Models;

namespace AutoRenter.API.Services
{
    public class ResponseConverter : IResponseConverter
    {
        public ResponseEnvelope Convert(dynamic data)
        {
            return new ResponseEnvelope {Data = data};
        }
    }
}