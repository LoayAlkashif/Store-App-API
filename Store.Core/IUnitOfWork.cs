﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Store.Core.Entities;
using Store.Core.Repositories.Contract;

namespace Store.Core
{
    public interface IUnitOfWork
    {

        Task<int> CompleteAsync();

        // create Repository<T> And Return 

        IGenericRepository<TEntity, TKey> Repository<TEntity, TKey>() where TEntity: BaseEntity<TKey>;

    }
}
