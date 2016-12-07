using System;
using System.Collections.Generic;

namespace AutoRenter.API.Services
{
    public interface ICrudService<T>
    {
        IEnumerable<T> List();
        T Get(Guid id);
        void Create(T model);
        void Delete(Guid id);
        void Update(Guid id, T model);
    }
}