using System;

namespace AutoRenter.Domain.Models
{
    public interface IEntity
    {
        Guid Id { get; set; }
    }
}