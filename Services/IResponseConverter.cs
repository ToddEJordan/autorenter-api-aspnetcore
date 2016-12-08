using AutoRenter.API.Models;

namespace AutoRenter.API.Services
{
    public interface IResponseConverter
    {
        ResponseEnvelope Convert(dynamic data);
    }
}