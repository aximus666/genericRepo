using Practice.Core.DomainInterfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice.Data
{
    public class Repository<C, T, E> : IRepository<T>
        where C : DbContext, new()
        where T : class, new()
        where E : class, new()
    {
        public Transformer<T, E> Mapper
        { get { return new Transformer<T, E>(); } }

        internal C context = new C();
        DbSet<E> table;

        public Repository()
        {
            table = context.Set<E>();
        }
        public void Add(T entity)
        {
            E e = Mapper.MapToE(entity);
            table.Add(e);
            context.SaveChanges();
        }

        public IList<T> Get(System.Linq.Expressions.Expression<Func<T, bool>> filter)
        {
            var newExpression = ExpressionHelper.TypeConvert<T, E, bool, bool>(filter);

            IList<E> list = table.Where(newExpression).ToList();

            return Mapper.TransformEIntoT(list);

        }
    }
}
