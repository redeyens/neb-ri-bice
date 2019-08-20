using System;
using System.Diagnostics;

namespace Example1
{
    class Program
    {
        static void Main()
        {
            double width = 500;
            double length = 2000;
            double waterLevel = 500;
            
            double beta = LookupBetaForBottomPanel(length, width);

            double bottomThickness = GlassThickness(waterLevel, beta);

            Console.WriteLine($"Required glass thickness is {bottomThickness}");

            if(Debugger.IsAttached)
            {
                Console.WriteLine("Press any key to close...");
                Console.ReadKey();
            }
        }

        private static double LookupBetaForBottomPanel(double length, double width)
        {
            // algorithm defines we always treat length as greater dimension
            // this implies ratio is always >= 1
            double sidesRatio = (length > width) ? length / width : width / length;

            if(sidesRatio < 1.5)
                return 0.4530;
            if(sidesRatio < 2.0)
                return 0.5172;
            if(sidesRatio < 2.5)
                return 0.5688;
            if(sidesRatio < 3.0)
                return 0.6102;
            return 0.7134;
        }

        private static double GlassThickness(double waterLevel, double beta)
        {
            double scalingFactor = 0.00001;
            double glassTensileStrength = 19.2;
            double safetyFactor = 3.8;
            
            double maxBendingStress = glassTensileStrength / safetyFactor;
            
            return Math.Sqrt(beta * Math.Pow(waterLevel, 3) * scalingFactor / maxBendingStress);
        }
    }
}
