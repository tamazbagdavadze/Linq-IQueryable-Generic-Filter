using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Linq_IQueryable_Generic_Filter;
using Args = Linq_IQueryable_Generic_Filter.Trio<string, System.TypeCode, Linq_IQueryable_Generic_Filter.FilterConstraint>;

namespace Example1
{
    class Program
    {
        static void Main(string[] args)
        {
            var test = new Test();
            
            var filter = new Filter
            {
                ConstraintList = new List<Args>
                {
                    new Args
                    {
                        TypeCode.String,
                        "Name",
                        new FilterConstraint
                        {
                            ContainsString = "az2",
                        }
                    },
                    new Args
                    {
                        TypeCode.String,
                        "Name",
                        new FilterConstraint
                        {
                            Equals = "tazo"
                        }
                    }
                },
                Or = true
            };

            var personSet = test.GetPersons(filter);
            
            foreach (var p in personSet)
            {
                Console.WriteLine($"{p.Age} {p.BirthDate} {p.Id} {p.IsMan} {p.Name} {p.Salary}");
            }

            Console.ReadLine();
        }
    }
}
