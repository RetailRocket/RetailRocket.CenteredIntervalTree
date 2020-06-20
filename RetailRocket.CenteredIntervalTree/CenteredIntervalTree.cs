namespace RetailRocket.CenteredIntervalTree
{
    using System.Collections.Generic;
    using System.Linq;
    using Interval.Intervals.Operations.BoundaryIntervals;

    public class CenteredIntervalTree<TPoint, TPointComparer, TValue>
        where TPoint : notnull
        where TPointComparer : IComparer<TPoint>, new()
    {
        private readonly ICenteredIntervalTreeNode<TPoint, TPointComparer, TValue> root;

        private readonly TPointComparer pointComparer;

        public CenteredIntervalTree(
            ICenteredIntervalTreeNode<TPoint, TPointComparer, TValue> root,
            TPointComparer pointComparer)
        {
            this.root = root;
            this.pointComparer = pointComparer;
        }

        public IEnumerable<TValue> Query(
            TPoint point)
        {
            return Query(
                node: this.root,
                point: point,
                pointComparer: this.pointComparer);
        }

        private static IEnumerable<TValue> Query(
            ICenteredIntervalTreeNode<TPoint, TPointComparer, TValue> node,
            TPoint point,
            TPointComparer pointComparer)
        {
            if (node is CenteredIntervalTreeNode<TPoint, TPointComparer, TValue> centeredIntervalTreeNode)
            {
                return Query(
                    centeredIntervalTreeNode,
                    point,
                    pointComparer);
            }

            return Enumerable.Empty<TValue>();
        }

        private static IEnumerable<TValue> Query(
            CenteredIntervalTreeNode<TPoint, TPointComparer, TValue> node,
            TPoint point,
            TPointComparer pointComparer)
        {
            foreach (var intervalValuePair in node.CenterBelonged)
            {
                var interval = intervalValuePair.Interval;

                if (interval
                    .LowerBoundary()
                    .CompareToPoint(point, pointComparer) > 0)
                {
                    break;
                }

                if (interval.Contains(point, pointComparer))
                {
                    yield return intervalValuePair.Value;
                }
            }

            var centerComparisonResult =
                pointComparer.Compare(point, node.CentralPoint);

            if (centerComparisonResult < 0)
            {
                foreach (var result in Query(node.LeftBranch, point, pointComparer))
                {
                    yield return result;
                }
            }
            else if (centerComparisonResult > 0)
            {
                foreach (var result in Query(node.RightBranch, point, pointComparer))
                {
                    yield return result;
                }
            }
        }
    }
}