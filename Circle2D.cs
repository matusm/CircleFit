using System;

namespace CircleFit
{
    public class Circle2D
    {
        public Point2D Center { get; set; }
        public double Radius { get; set; }

        public Circle2D(Point2D Center, double Radius)
        {
            this.Center = Center;
            this.Radius = Radius;
        }

        public double Distance(Point2D point)
        {
            return Math.Abs(SignedDistance(point));
        }

        public double SignedDistance(Point2D point)
        {
            return Center.Distance(point) - Radius;
        }

        public override string ToString()
        {
            return $"Circle2D[{Center} , {Radius}]";
        }
    }
}
