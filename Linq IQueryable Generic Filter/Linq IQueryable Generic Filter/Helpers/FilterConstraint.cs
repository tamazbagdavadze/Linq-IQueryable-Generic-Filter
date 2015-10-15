using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace Linq_IQueryable_Generic_Filter
{
//    [StructLayout(LayoutKind.Auto)]
//    public struct DateTimeRange
//    {
//        public DateTime? From { get; set; }
//        public DateTime? To { get; set; }
//    }

        
    public class FilterConstraint
    {
        public new object Equals { get; set; }

        public object LessThan { get; set; }
        public object MoreThan { get; set; }
        

        public string ContainsString { get; set; }
        public string StartsWith { get; set; }
        public string EndsWith { get; set; }
        
        //public DateTimeRange? DateTimeRange { get; set; } 
        

        //TODO Random shit
    }
    
    public class Pair : IEnumerable // TODO :(
    {
        public string Key;

        public FilterConstraint Constraints;

        public void Add(FilterConstraint param)
        {
            Constraints = param;
        }

        public void Add(string param)
        {
            Key = param;
        }

        public IEnumerator GetEnumerator()
        {
            throw new System.NotImplementedException("omfb");
        }
    }
}
