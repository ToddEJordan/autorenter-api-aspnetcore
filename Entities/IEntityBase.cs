using System;

namespace AutoRenter.API.Entities
{
    public interface IEntityBase
    {
        Guid Id { get; set; }
    }
}