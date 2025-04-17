using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Services.Contract
{
    public interface ICacheService
    {

        // te5azn fe el Db
        Task SetCacheKeyAsync(string key, object response, TimeSpan expireTime); 

        //Tgeb mn el Db
        Task<string> GetCacheKeyAsync(string key);
    }
}
