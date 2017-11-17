using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Practice.Core.DomainInterfaces
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        IList<T> Get(Expression<Func<T, bool>> filter);

    }
}
