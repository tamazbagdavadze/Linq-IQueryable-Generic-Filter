# Linq-IQueryable-Generic-Filter
Linq IQueryable Generic Filter


Simple example : 


            IQueryable<Person> users = Data.GetPersons();

            Filter filter = new Filter
            {
                ConstraintList = new List<Pair>
                {
                    new Pair
                    {
                        "Name",
                        new FilterConstraint
                        {
                            Equals = "Tazo"
                        }
                    },
                    new Pair
                    {
                        "Name",
                        new FilterConstraint
                        {
                            ContainsString = "2"
                        }
                    }
                }
            };

            List<Person> resultList = users.Filter(filter).ToList();
