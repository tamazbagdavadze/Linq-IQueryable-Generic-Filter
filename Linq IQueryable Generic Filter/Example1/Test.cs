using System;
using System.Collections.Generic;
using System.Linq;
using Linq_IQueryable_Generic_Filter;
using Args = Linq_IQueryable_Generic_Filter.Trio<string, System.TypeCode, Linq_IQueryable_Generic_Filter.FilterConstraint>;
namespace Example1
{
    class Test
    {

        private Dal Data { get; } = new Dal();

        public void TestStringAndInt()
        {
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
                            StartsWith = "tazo",
                            EndsWith = "2"
                        }
                    },
                     new Args
                     {
                         TypeCode.Int16,
                         "Age",
                         new FilterConstraint
                         {
                             Equals = 18
                         }
                     }
                },
                Or = true
            };

            var personSet = Data.GetPersons(filter).ToList();
            Console.WriteLine("--------------- Results : ---------------");
            foreach (var p in personSet)
            {
                Console.WriteLine($"{p.Age} {p.BirthDate} {p.Id} {p.IsMan} {p.Name} {p.Salary}");
            }
        }
    }
}
