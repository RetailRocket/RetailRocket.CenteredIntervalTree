namespace RetailRocket.CenteredIntervalTree
{
    using System.Collections.Generic;

    public interface ICenteredIntervalTreeNode<TPoint, TPointComparer, TValue>
        where TPoint : notnull
        where TPointComparer : IComparer<TPoint>, new()
    {
    }
}