using System;
using System.Collections.Generic;
﻿using System.Threading.Tasks;
using AutoRenter.Domain.Models;

namespace AutoRenter.Domain.Interfaces
{
    public interface IModelService
    {
        Task<Result<IEnumerable<Model>>> GetAll();
        Task<Result<Model>> Get(Guid id);
        Task<Result<Model>> Get(string id);
    }
}
