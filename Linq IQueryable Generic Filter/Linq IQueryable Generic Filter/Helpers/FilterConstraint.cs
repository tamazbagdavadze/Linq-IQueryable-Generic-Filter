using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace Linq_IQueryable_Generic_Filter
{
    [StructLayout(LayoutKind.Auto)]
    public struct DateTimeRange
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }


    [StructLayout(LayoutKind.Auto)]
    public struct FilterConstraint
    {
        public new object Equals { get; set; }

        public long? LessThen { get; set; }
        public long? MoreThen { get; set; }
        

        public string ContainsString { get; set; }

        public string StartsWith { get; set; }
        public string EndsWith { get; set; }

        public bool? IsNull { get; set; }
        public bool? IsEmpty { get; set; }

        public DateTimeRange? DateTimeRange { get; set; } 

        public int? CountMorethan { get; set; }
        public int? CountLessthan { get; set; }


        //TODO Random shit
    }

    [StructLayout(LayoutKind.Auto)]
    public struct Trio<T,TR,TL> : IEnumerable
    {
        public T Key;

        public TR Type;

        public TL Constraints;

        public void Add(TL param)
        {
            Constraints = param;
        }

        public void Add(T param)
        {
            Key = param;
        }

        public void Add(TR param)
        {
            Type = param;
        }

        public IEnumerator GetEnumerator()
        {
            throw new System.NotImplementedException("omfb");
        }
    }
}
