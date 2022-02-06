using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace CircleFit
{
    class Program
    {
        static void Main(string[] args)
        {

            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            #region command line interface
            string inputFilename = string.Empty;
            string outputFilename = string.Empty;
            if (args.Length == 0)
            {
                Console.WriteLine("No filename provided!");
                Environment.Exit(1);
            }
            if (args.Length >= 1)
            {
                inputFilename = args[0];

                outputFilename = Path.ChangeExtension(inputFilename, ".prn");
            }
            if (args.Length == 2)
            {
                outputFilename = args[1];
            }
            if (Path.GetExtension(inputFilename) == "")
                inputFilename = Path.ChangeExtension(inputFilename, ".csv");
            if (Path.GetExtension(outputFilename) == "")
                outputFilename = Path.ChangeExtension(outputFilename, ".prn");
            #endregion

            CircleFitter fitter = new CircleFitter(GetData(inputFilename));

            using (StreamWriter writer = new StreamWriter(outputFilename, false))
            {
                writer.WriteLine($"Center x:       {fitter.FittedCircle.Center.X:F10} m");
                writer.WriteLine($"Center y:       {fitter.FittedCircle.Center.Y:F10} m");
                writer.WriteLine($"Radius:         {fitter.FittedCircle.Radius:F10} m");
                writer.WriteLine($"Points:         {fitter.NumberPoints}");
                writer.WriteLine($"RangeResiduals: {fitter.RangeResiduals*1e6:F3} µm");
                foreach (var c in GetComments(inputFilename))
                {
                    writer.WriteLine(c);
                }
                writer.WriteLine("index , x_global , y_global , x_N , y_N , phi , residual");
                writer.WriteLine("1 , m , m , 1 , 1 , ° , µm");
                foreach (var pod in fitter.Pod)
                {
                    writer.WriteLine(pod.ToCsvString());
                }
            }

            Console.WriteLine($"Center x: {fitter.FittedCircle.Center.X * 1000:F6} mm");
            Console.WriteLine($"Center y: {fitter.FittedCircle.Center.Y * 1000:F6} mm");
            Console.WriteLine($"Radius:   {fitter.FittedCircle.Radius * 1000:F6} mm");
            Console.WriteLine($"# Points: {fitter.NumberPoints}");
            Console.WriteLine();
        }

        private static List<string> GetComments(string filename)
        {
            List<string> comments = new List<string>();
            using (StreamReader reader = new StreamReader(File.OpenRead(filename)))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var p = PointParse(line);
                    if (p == null)
                    {
                        comments.Add($">>> {line.Trim()}");
                    }
                }
            }
            return comments;
        }


        private static Point2D[] GetData(string filename)
        {
            List<Point2D> data = new List<Point2D>();
            List<string> comments = new List<string>();
            using (StreamReader reader = new StreamReader(File.OpenRead(filename)))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var p = PointParse(line);
                    if (p != null)
                    {
                        data.Add(p);
                    }
                    else
                    {
                        comments.Add($">>> {line.Trim()}");
                    }
                }
            }
            return data.ToArray();
        }

        private static Point2D PointParse(string line)
        {
            var tokens = line.Split(',');
            if (tokens.Length == 2)
            {
                double x = MyParse(tokens[0]);
                double y = MyParse(tokens[1]);
                if (!double.IsNaN(x + y))
                {
                    return new Point2D(x, y);
                }
            }
            return null;
        }

        private static double MyParse(string token)
        {
            if (double.TryParse(token, out double value))
                return value;
            else
                return double.NaN;
        }

    }
}
