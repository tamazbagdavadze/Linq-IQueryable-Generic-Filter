using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Linq_IQueryable_Generic_Filter;

namespace Example1
{
    class Test
    {
        private TestDBContainer Context { get; set; } = new TestDBContainer();

        public IEnumerable<Person> GetPersons(Filter filter)
        {
            var persons = Context.People.AsQueryable();

            var filteredPersons = GenericFilter.Filter(filter, persons);

            return filteredPersons;
        } 
    }
}
