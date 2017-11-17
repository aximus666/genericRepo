using Practice.Core.DomainInterfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice.Data
{
    public class RepoFactory : IRepoFactory
    {
        private object lockObject = new object();

        Dictionary<string, object> repositoryContainer = new Dictionary<string, object>();
        public virtual object CreateRepo<T>() where T : class, new()
        {
            var table = typeof(T).GetCustomAttributes(typeof(TableAttribute), true).FirstOrDefault() as TableAttribute;

            return Activator.CreateInstance(typeof(Repository<,,>).MakeGenericType(new Type[] { typeof(PracticeEntities), typeof(T), Type.GetType(GetType().Namespace + "." + table.Name) }));
        }

        public IRepository<T> GetRepo<T>() where T : class, new()
        {
            lock (lockObject)
            {
                var isExist = repositoryContainer.ContainsKey(typeof(T).Name);
                if (!isExist)
                    repositoryContainer.Add(typeof(T).Name, CreateRepo<T>());
            }

            return repositoryContainer[typeof(T).Name] as IRepository<T>;

        }

        public void Dispose()
        {
            lock (lockObject)
            {
                if (repositoryContainer == null)
                {
                    return;
                }
                foreach (var item in repositoryContainer)
                {
                    IDisposable disposable = (IDisposable)item.Value;
                    disposable.Dispose();
                }
                repositoryContainer.Clear();
                repositoryContainer = null;
            }
        }
    }
}
