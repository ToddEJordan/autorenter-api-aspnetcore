namespace AutoRenter.Api.Services
{
    public class ResponseConverter : IResponseConverter
    {
        public ResponseEnvelope Convert(dynamic data)
        {
            return new ResponseEnvelope {Data = data};
        }
    }
}