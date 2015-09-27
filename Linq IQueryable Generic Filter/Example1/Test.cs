﻿using System;
using System.Collections.Generic;
using System.Linq;
using Linq_IQueryable_Generic_Filter;
using Pair = Linq_IQueryable_Generic_Filter.Pair<string, Linq_IQueryable_Generic_Filter.FilterConstraint>;

namespace Example1
{
    internal class Test
    {
        private Dal Data { get; } = new Dal();

        public void TestStringAndInt()
        {
            var filter = new Filter
            {
                ConstraintList = new List<Pair>
                {
                    new Pair
                    {
                        "Name",
                        new FilterConstraint
                        {
                            StartsWith = "tazo",
                            EndsWith = "2"
                        }
                    },
                    new Pair
                    {
                        "Age",
                        new FilterConstraint
                        {
                            Equals = (short)18
                        }
                    }
                },
                Or = true
            };

            var personSet = Data.GetPersons(filter).ToList();
            ShowData(personSet);
        }

        public void TestLess()
        {
            var filter = new Filter
            {
                ConstraintList = new List<Pair>
                {
                    new Pair
                    {
                        "BirthDate",
                        new FilterConstraint
                        {
                            LessThen = DateTime.Parse("10/10/2001 12:00:00 AM") //DateTime.Now
                        }
                    }
                },
                Or = true
            };

            var personSet = Data.GetPersons(filter).ToList();

            ShowData(personSet);
        }

        public void TestTest()
        {
            var filter = new Filter
            {
                ConstraintList = new List<Pair>
                {
                    new Pair
                    {
                        "BirthDate",
                        new FilterConstraint
                        {
                            Equals = DateTime.Parse("10/10/2001 12:00:00 AM") //DateTime.Now
                        }
                    }
                },
                Or = true
            };

            var personSet = Data.GetPersons(filter).ToList();

            ShowData(personSet);
        }

        private void ShowData(IEnumerable<Person> persons)
        {
            Console.WriteLine("--------------- Results : ---------------");
            foreach (var p in persons)
            {
                Console.WriteLine($"{p.Age} {p.BirthDate} {p.Id} {p.IsMan} {p.Name} {p.Salary}");
            }
        }
    }
}
