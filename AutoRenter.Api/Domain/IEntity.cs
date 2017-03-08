using System;

namespace AutoRenter.Api.Domain
{
    public interface IEntity
    {
        Guid Id { get; set; }
    }
}