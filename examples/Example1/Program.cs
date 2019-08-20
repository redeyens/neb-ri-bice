using System;
using System.Diagnostics;

namespace Example1
{
    class Program
    {
        static void Main()
        {
            // double width = 500;
            // double length = 500;
            double waterLevel = 500;

            double bottomThickness = GlassThickness(waterLevel);

            Console.WriteLine($"Required glass thickness is {bottomThickness}");

            if(Debugger.IsAttached)
            {
                Console.WriteLine("Press any key to close...");
                Console.ReadKey();
            }
        }

        private static double GlassThickness(double waterLevel)
        {
            double scalingFactor = 0.00001;
            double glassTensileStrength = 19.2;
            double safetyFactor = 3.8;
            
            double maxBendingStress = glassTensileStrength / safetyFactor;
            
            double beta = 0.4530;

            return Math.Sqrt(beta * Math.Pow(waterLevel, 3) * scalingFactor / maxBendingStress);
        }
    }
}
