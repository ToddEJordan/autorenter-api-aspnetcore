namespace AutoRenter.Api.Domain
{
    public class Result<T>
    {
        public Result() { }
        public Result(ResultCode resultCode)
        {
            ResultCode = resultCode;
        }

        public Result(ResultCode resultCode, T data)
        {
            ResultCode = resultCode;
            Data = data;
        }

        public ResultCode ResultCode { get; set; }
        public T Data { get; set; }
    }
}
