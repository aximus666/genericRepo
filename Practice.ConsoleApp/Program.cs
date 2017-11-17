using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Practice.Core.DomainInterfaces;
using Practice.Data;
using Practice.Core.DomainObjects;

namespace Practice.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {

            IRepoFactory factory = new RepoFactory();
            User u = new User();
            u.UserName = "ozgur";
            u.Password = "fdsf";
            //factory.GetRepo<User>().Get(w => true);
            factory.GetRepo<User>().Add(u);

            var list = factory.GetRepo<User>().Get(w => true);


        }

        static void RegisterDI()
        {

        }
    }


}
