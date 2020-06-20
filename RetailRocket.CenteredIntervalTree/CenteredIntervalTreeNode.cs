﻿namespace RetailRocket.CenteredIntervalTree
{
    using System.Collections.Generic;
    using Interval.Intervals.Operations.BoundaryIntervals;

    public readonly struct CenteredIntervalTreeNode<TPoint, TPointComparer, TValue> :
        ICenteredIntervalTreeNode<TPoint, TPointComparer, TValue>
        where TPoint : notnull
        where TPointComparer : IComparer<TPoint>, new()
    {
        public CenteredIntervalTreeNode(
            ICenteredIntervalTreeNode<TPoint, TPointComparer, TValue> leftBranch,
            ICenteredIntervalTreeNode<TPoint, TPointComparer, TValue> rightBranch,
            List<IntervalValuePair<TPoint, TPointComparer, TValue>> centerBelonged,
            TPoint centralPoint)
        {
            this.LeftBranch = leftBranch;
            this.RightBranch = rightBranch;
            this.CenterBelonged = centerBelonged;
            this.CentralPoint = centralPoint;
        }

        public ICenteredIntervalTreeNode<TPoint, TPointComparer, TValue> LeftBranch { get; }

        public ICenteredIntervalTreeNode<TPoint, TPointComparer, TValue> RightBranch { get; }

        public List<IntervalValuePair<TPoint, TPointComparer, TValue>> CenterBelonged { get; }

        public TPoint CentralPoint { get; }
    }
}