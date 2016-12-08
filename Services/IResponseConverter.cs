using AutoRenter.API.Entities;

namespace AutoRenter.API.Services
{
    public interface IResponseConverter
    {
        ResponseEnvelope Convert(dynamic data);
    }
}