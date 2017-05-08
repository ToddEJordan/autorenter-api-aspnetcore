using System;

namespace AutoRenter.Api.Domain
{
    public class Result<T>
    {
        public ResultCode ResultCode { get; set; }
        public T Data { get; set; }
    }
}
