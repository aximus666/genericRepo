using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace Practice.Data
{
    public class Transformer<T, E>
        where T : class
        where E : class
    {
        IMapper mapperTtoE;
        IMapper mapperEtoT;
        public Transformer()
        {
            mapperTtoE = new MapperConfiguration(c => c.CreateMap<T, E>()).CreateMapper();
            mapperEtoT = new MapperConfiguration(c => c.CreateMap<E, T>()).CreateMapper();
        }

        public E MapToE(T t)
        {
            return mapperTtoE.Map<E>(t);
        }

        public T MapEoT(E e)
        {
            return mapperEtoT.Map<T>(e);
        }

        public IList<E> TransformTIntoE(IList<T> list)
        {
            List<E> transform = new List<E>();
            foreach (var t in list)
            {
                transform.Add(MapToE(t));
            }
            return transform;
        }

        public IList<T> TransformEIntoT(IList<E> list)
        {
            List<T> transform = new List<T>();
            foreach (var t in list)
            {
                transform.Add(MapEoT(t));
            }
            return transform;
        }
    }
}
