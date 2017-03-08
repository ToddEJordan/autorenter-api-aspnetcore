namespace AutoRenter.Api.Services
{
    public interface IResponseConverter
    {
        ResponseEnvelope Convert(dynamic data);
    }
}