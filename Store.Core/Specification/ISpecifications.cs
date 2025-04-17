using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Store.Core.Entities;

namespace Store.Core.Specification
{

    public interface ISpecifications <TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        /// Criteria m3naha m3ayeer elle btnafez Where fe el LINQ
        public Expression<Func<TEntity, bool>> Criteria { get; set; }

        public List<Expression<Func<TEntity, object>>> Includes { get; set; }

        public Expression<Func<TEntity, object>> OrderBy { get; set; }

        public Expression<Func<TEntity, object>> OrderByDescending { get; set; }

        public int Skip { get; set; } 
        public int Take { get; set; } 
        public bool IsPaginationEnabled { get; set; } 

    }
}
