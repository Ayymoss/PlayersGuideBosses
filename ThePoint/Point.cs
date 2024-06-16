namespace ThePoint;

public class Point(int x, int y)
{
    public int X { get; } = x;
    public int Y { get; } = y;

    public Point() : this(0, 0)
    {
    }
}
