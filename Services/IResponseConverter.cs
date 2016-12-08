namespace AutoRenter.API.Services
{
    public interface IResponseConverter
    {
        string Convert(dynamic data);
    }
}