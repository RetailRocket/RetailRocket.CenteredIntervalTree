namespace RetailRocket.CenteredIntervalTree
{
    using System.Collections.Generic;
    using Interval.Intervals;

    public class IntervalValuePair<TPoint, TPointComparer, TValue>
        where TPoint : notnull
        where TPointComparer : IComparer<TPoint>, new()
    {
        public IntervalValuePair(
            IBoundaryInterval<TPoint, TPointComparer> interval,
            TValue value)
        {
            this.Interval = interval;
            this.Value = value;
        }

        public IBoundaryInterval<TPoint, TPointComparer> Interval { get; set; }

        public TValue Value { get; set; }
    }
}