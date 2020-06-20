namespace RetailRocket.CenteredIntervalTree
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Interval.Boundaries.Operations;
    using Interval.Intervals;
    using Interval.Intervals.Operations.BoundaryIntervals;
    using Optional;
    using Optional.Collections;
    using Optional.Unsafe;

    public static class CenteredIntervalTreeFactory
    {
        public static CenteredIntervalTree<TPoint, TPointComparer, TValue> Build<TPoint, TPointComparer, TValue>(
            List<IntervalValuePair<TPoint, TPointComparer, TValue>> intervalValuePairList,
            TPointComparer pointComparer)
            where TPoint : notnull
            where TPointComparer : IComparer<TPoint>, new()
        {
            return new CenteredIntervalTree<TPoint, TPointComparer, TValue>(
                root: BuildNode(
                    intervalValuePairList,
                    pointComparer),
                pointComparer: pointComparer);
        }

        private static ICenteredIntervalTreeNode<TPoint, TPointComparer, TValue> BuildNode<TPoint, TPointComparer, TValue>(
            List<IntervalValuePair<TPoint, TPointComparer, TValue>> intervalValuePairList,
            TPointComparer pointComparer)
            where TPoint : notnull
            where TPointComparer : IComparer<TPoint>, new()
        {
            if (!intervalValuePairList.Any())
            {
                return default(EmptyCenteredIntervalTreeNode<TPoint, TPointComparer, TValue>);
            }

            var centralPoint = CentralBoundaryPoint(
                intervalValuePairList,
                pointComparer).ValueOrFailure();

            var leftBranch = new List<IntervalValuePair<TPoint, TPointComparer, TValue>>();
            var rightBranch = new List<IntervalValuePair<TPoint, TPointComparer, TValue>>();
            var centerBelonged = new List<IntervalValuePair<TPoint, TPointComparer, TValue>>();

            foreach (var intervalValuePair in intervalValuePairList)
            {
                var interval = intervalValuePair.Interval;
                if (PointOnBoundaryOrWithinInterval(interval, centralPoint, pointComparer))
                {
                    centerBelonged.Add(intervalValuePair);
                }
                else if (interval.LowerBoundary()
                    .CompareToPoint(centralPoint, pointComparer) < 0)
                {
                    leftBranch.Add(intervalValuePair);
                }
                else
                {
                    rightBranch.Add(intervalValuePair);
                }
            }

            var lowerBoundaryComparer = new LowerBoundaryComparer<TPoint, TPointComparer>(
                pointComparer: pointComparer);

            return new CenteredIntervalTreeNode<TPoint, TPointComparer, TValue>(
                leftBranch: BuildNode(
                    intervalValuePairList: leftBranch,
                    pointComparer: pointComparer),
                rightBranch: BuildNode(
                    intervalValuePairList: rightBranch,
                    pointComparer: pointComparer),
                centerBelonged: centerBelonged.OrderBy(
                        i => i.Interval.LowerBoundary(),
                        comparer: lowerBoundaryComparer)
                    .ToList(),
                centralPoint: centralPoint);
        }

        private static bool PointOnBoundaryOrWithinInterval<TPoint, TPointComparer>(
            IBoundaryInterval<TPoint, TPointComparer> boundaryInterval,
            TPoint point,
            TPointComparer pointComparer)
            where TPoint : notnull
            where TPointComparer : IComparer<TPoint>, new()
        {
            return boundaryInterval.Contains(point, pointComparer) ||
                   boundaryInterval.IsBoundaryPoint(point, pointComparer);
        }

        private static Option<TPoint> CentralBoundaryPoint<TPoint, TPointComparer, TValue>(
            List<IntervalValuePair<TPoint, TPointComparer, TValue>> intervalValuePairList,
            TPointComparer pointComparer)
            where TPoint : notnull
            where TPointComparer : IComparer<TPoint>, new()
        {
            var boundaryPointList = intervalValuePairList
                .SelectMany(i => i.Interval.MapBoundaryPoints(p => p))
                .OrderBy(p => p, pointComparer)
                .ToArray();

            return boundaryPointList
                .ElementAtOrNone(
                    boundaryPointList.Length / 2);
        }
    }
}