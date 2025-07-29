using System;

namespace CircleFit
{
    public class Point2D
    {
        public double X { get; }
        public double Y { get; }

        public Point2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double Distance(Point2D point)
        {
            double dx = X - point.X;
            double dy = Y - point.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public static Point2D operator -(Point2D p1, Point2D p2) => new Point2D(p1.X - p2.X, p1.Y - p2.Y);

        public static Point2D operator *(Point2D p1, double k) => new Point2D(p1.X * k, p1.Y * k);

        public override string ToString() => $"[Point2D: X={X} Y={Y}]";
    }
}
