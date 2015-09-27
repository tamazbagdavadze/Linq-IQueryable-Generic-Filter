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


    [StructLayout(LayoutKind.Auto)]
    public struct FilterConstraint
    {
        public new object Equals { get; set; }

        public object LessThen { get; set; }
        public object MoreThen { get; set; }
        

        public string ContainsString { get; set; }
        public string StartsWith { get; set; }
        public string EndsWith { get; set; }
        
        //public DateTimeRange? DateTimeRange { get; set; } 
        

        //TODO Random shit
    }

    [StructLayout(LayoutKind.Auto)]
    public struct Pair<T,TL> : IEnumerable
    {
        public T Key;

        public TL Constraints;

        public void Add(TL param)
        {
            Constraints = param;
        }

        public void Add(T param)
        {
            Key = param;
        }

        public IEnumerator GetEnumerator()
        {
            throw new System.NotImplementedException("omfb");
        }
    }
}
