using System.Collections.Generic;

namespace AutoRenter.API.Services
{
    public interface ICrudService<T>
    {
        IEnumerable<T> List();
        T Get(int id);
        void Create(T model);
        void Delete(int id);
        void Update(int id, T model);
    }
}