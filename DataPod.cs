using System;

namespace CircleFit
{
    public class DataPod : IComparable<DataPod>
    {
        public int Index { get; }
        public Point2D RawPoint { get; }
        public Point2D NormalizedPoint { get; }
        public double Phi { get; }
        public double Residual { get; }

        public DataPod(int index, Point2D raw, Point2D normalized, double phi, double res)
        {
            Index = index;
            RawPoint = raw;
            NormalizedPoint = normalized;
            Phi = phi;
            Residual = res;
        }

        public string ToCsvString()
        {
            return $"{Index,4} , {RawPoint.X:F10} , {RawPoint.Y:F10} , {NormalizedPoint.X,9:F6} , {NormalizedPoint.Y,9:F6} , {Phi,7:F2} , {Residual * 1e6,6:F3}";
        }

        public override string ToString()
        {
            return $"[DataPod: Index={Index} RawPoint={RawPoint}]";
        }

        public int CompareTo(DataPod other)
        {
            return Phi.CompareTo(other.Phi);
        }
    }
}
