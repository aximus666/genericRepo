using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice.Core.DomainInterfaces
{
    public interface IRepoFactory: IDisposable
    {
        IRepository<T> GetRepo<T>() where T : class, new();

        object CreateRepo<T>() where T : class, new();

    }
}
