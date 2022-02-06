//****************************************************************************************
//
// Class to fit a circle to an array of points in 2D cartesian coordinates.
//
// Usage:
// 1.) create an instance of the CircleFitter class providing an array of Point2D
// 2.) consume the properties (all calculations are performed in the constructor)
// 
// Michael Krystek, Ausgleichskreise in der Ebene
// Technisches Messen 71 (2004) 5, p319 
// 
// Author: Michael Matus, 2022
//
//****************************************************************************************


using System;
using System.Linq;

namespace CircleFit
{
    public class CircleFitter
    {
        private const double epsilon = 1.0E-10; // for NMM applications, 0.1 nm

        public Circle2D FittedCircle { get; }
        public Point2D[] Points { get; }
        public int NumberPoints => Points.Length;
        public Point2D[] NormalizedPoints { get; private set; }
        public double[] Residuals { get; private set; }
        public double[] Phi { get; private set; }
        public DataPod[] Pod { get; private set; }
        public double RangeResiduals => Residuals.Max() - Residuals.Min();

        public CircleFitter(Point2D[] points)
        {
            Points = points;
            FittedCircle = FitCircleToPoints();
            NormalizePoints();
            CalculateResiduals();
            PopulatePod();
        }

        private void NormalizePoints()
        {
            NormalizedPoints = new Point2D[NumberPoints];
            for (int i = 0; i < NumberPoints; i++)
            {
                NormalizedPoints[i] = (Points[i] - FittedCircle.Center) * (1.0 / FittedCircle.Radius);
            }
        }

        private void CalculateResiduals()
        {
            Residuals = new double[NumberPoints];
            Phi = new double[NumberPoints];
            for (int i = 0; i < NumberPoints; i++)
            {
                Residuals[i] = FittedCircle.SignedDistance(Points[i]);
                double phi = Math.Atan2(NormalizedPoints[i].Y, NormalizedPoints[i].X) * (180 / Math.PI);
                if (phi < 0) phi = 360 + phi;
                Phi[i] = phi;
            }
        }

        private void PopulatePod()
        {
            Pod = new DataPod[NumberPoints];
            for (int i = 0; i < NumberPoints; i++)
            {
                Pod[i] = new DataPod(i, Points[i], NormalizedPoints[i], Phi[i], Residuals[i]);
            }
            Array.Sort(Pod);
        }

        private Point2D CenterOfGravity()
        {
            double xQuer = 0;
            double yQuer = 0;
            foreach (var point in Points)
            {
                xQuer += point.X;
                yQuer += point.Y;
            }
            xQuer /= NumberPoints;
            yQuer /= NumberPoints;
            return new Point2D(xQuer, yQuer);
        }

        private Circle2D FitCircleToPoints()
        {
            double radius;
            var cog = CenterOfGravity();
            var tempCenter = cog;
            while (true)
            {
                Point2D newCenter = Iterate(tempCenter);
                if (newCenter.Distance(tempCenter) <= epsilon)
                {
                    return new Circle2D(newCenter, radius);
                }
                tempCenter = newCenter;
            }

            Point2D Iterate(Point2D temp)
            {
                double C = 0;
                double S = 0;
                double L = 0;
                foreach (var p in Points)
                {
                    double distance = p.Distance(temp);
                    C += (p.X - temp.X) / distance;
                    S += (p.Y - temp.Y) / distance;
                    L += distance;
                }
                C /= NumberPoints;
                S /= NumberPoints;
                L /= NumberPoints;
                radius = L;
                return new Point2D(cog.X - C * radius, cog.Y - S * radius);
            }

        }

    }
}
