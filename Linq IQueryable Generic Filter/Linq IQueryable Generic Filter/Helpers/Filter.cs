using System.Collections.Generic;
using Pair = Linq_IQueryable_Generic_Filter.Pair<string, Linq_IQueryable_Generic_Filter.FilterConstraint>;

namespace Linq_IQueryable_Generic_Filter
{
    public class Filter
    {
        public Filter(List<Pair> list = null)
        {
            ConstraintList = list ?? new List<Pair>();
        }

        public List<Pair> ConstraintList { get; set; }

        public bool? Or { get; set; }

        public int? Take { get; set; }
        public int? Skip { get; set; }

    }
}
