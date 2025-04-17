using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Helper
{
    public class PaginationResponse<TEntity>
    {
        public PaginationResponse(int limit, int page, int count, IEnumerable<TEntity> data)
        {
            Page = page;
            Limit = limit;
            Count = count;
            Data = data;
        }

        public int Limit { get; set; }

        public int Page { get; set; }

        public int Count { get; set; } 

        public IEnumerable<TEntity> Data { get; set; }

    }
}
