using System.Collections.Generic;
using Args = Linq_IQueryable_Generic_Filter.Trio<string, System.TypeCode, Linq_IQueryable_Generic_Filter.FilterConstraint>;

namespace Linq_IQueryable_Generic_Filter
{
    public class Filter
    {
        public Filter(List<Args> list = null)
        {
            ConstraintList = list ?? new List<Args>();
        }

        public List<Args> ConstraintList { get; set; }

        public bool? Or { get; set; }

        public int? Take { get; set; }
        public int? Skip { get; set; }

        public List<int> IncludedIDs { get; set; }
        public List<int> ExcludedIDs { get; set; }
    }
}
