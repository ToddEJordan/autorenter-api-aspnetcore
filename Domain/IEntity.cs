using System;

namespace AutoRenter.API.Domain
{
    public interface IEntity
    {
        Guid Id { get; set; }
    }
}