namespace RetailRocket.CenteredIntervalTree
{
    using System.Collections.Generic;

    public readonly struct EmptyCenteredIntervalTreeNode<TPoint, TPointComparer, TValue> :
        ICenteredIntervalTreeNode<TPoint, TPointComparer, TValue>
        where TPoint : notnull
        where TPointComparer : IComparer<TPoint>, new()
    {
    }
}