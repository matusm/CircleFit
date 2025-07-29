using System;

namespace CircleFit
{
    public class Circle2D
    {
        public Point2D Center { get; }
        public double Radius { get; }

        public Circle2D(Point2D Center, double Radius)
        {
            this.Center = Center;
            this.Radius = Radius;
        }

        public double Distance(Point2D point) => Math.Abs(SignedDistance(point));

        public double SignedDistance(Point2D point) => Center.Distance(point) - Radius;

        public override string ToString() => $"[Circle2D: Center={Center} Radius={Radius}]";
    }
}
