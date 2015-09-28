using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Linq_IQueryable_Generic_Filter;

namespace Example1
{
    class Dal
    {
        private TestDBContainer Context { get; } = new TestDBContainer();

        public Dal()
        {
            Context.Database.Log = Console.WriteLine;
        }
        

        public IQueryable<Person> GetPersons()
        {
            return Context.People.AsQueryable();
        } 
    }
}
