namespace AutoRenter.Domain.Models
{
    public enum ResultCode
    {
        Unknown = 0,
        Success,
        NotFound,
        Failed,
        BadRequest,
        Conflict
    }
}
