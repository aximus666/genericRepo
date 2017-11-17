using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice.Core.DomainObjects
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public string Name { get; set; }
    }
}
