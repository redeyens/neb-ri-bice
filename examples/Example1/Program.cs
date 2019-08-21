using System;
using System.Diagnostics;

namespace Example1
{
    class Program
    {
        private const double mmCubePerLiter = 1.0e6;
        private const double mmSquaredPerMeterSquared = 1.0e6;

        static void Main()
        {
            double width = GetInput("Enter width [mm]: ");
            double length = GetInput("Enter length [mm]: ");
            double waterLevel = GetInput("Enter water level (height) [mm]: ");

            double betaBottomPanel = LookupBetaForBottomPanel(length, width);
            double bottomThickness = GlassThickness(waterLevel, betaBottomPanel);

            double betaFrontPanel = LookupBetaForSidePanel(length, waterLevel);
            double frontPanelThickness = GlassThickness(waterLevel, betaFrontPanel);

            double betaSidePanel = LookupBetaForSidePanel(width, waterLevel);
            double sidePanelThickness = GlassThickness(waterLevel, betaSidePanel);

            Console.WriteLine();
            Console.WriteLine($"Required glass thickness for bottom panel is {bottomThickness:N1} mm.");
            Console.WriteLine($"Required glass thickness for front/back panel is {frontPanelThickness:N1} mm.");
            Console.WriteLine($"Required glass thickness for left/right panel is {sidePanelThickness:N1} mm.");

            Console.WriteLine();
            Console.WriteLine($"Total glass area is {TotalGlassAreaMeters(width, length, waterLevel):N1} m^2.");
            Console.WriteLine($"Total water volume is {WaterVolumeLiters(width, length, waterLevel):N1} l.");

            if (Debugger.IsAttached)
            {
                Console.WriteLine();
                Console.WriteLine("Press any key to close...");
                Console.ReadKey();
            }
        }

        private static object TotalGlassAreaMeters(double width, double length, double waterLevel)
        {
            double frontPanelArea = length * waterLevel;
            double sidePanelArea = width * waterLevel;
            double bottomPanelArea = length * width;

            return (bottomPanelArea + 2 * frontPanelArea + 2 * sidePanelArea) / mmSquaredPerMeterSquared;
        }

        private static object WaterVolumeLiters(double width, double length, double waterLevel)
        {
            return width * length * waterLevel / mmCubePerLiter;
        }

        private static double LookupBetaForSidePanel(double length, double waterLevel)
        {
            double sidesRatio = length / waterLevel;

            if (sidesRatio < 0.7)
                return 0.085;
            if (sidesRatio < 1.0)
                return 0.116;
            if (sidesRatio < 1.5)
                return 0.160;
            if (sidesRatio < 2.0)
                return 0.260;
            if (sidesRatio < 2.5)
                return 0.320;
            if (sidesRatio < 3.0)
                return 0.350;
            return 0.370;

        }

        private static double GetInput(string prompt)
        {
            double userInput;
            do
            {
                Console.Write(prompt);
            } while (!double.TryParse(Console.ReadLine(), out userInput) || !InputIsValid(userInput));

            return userInput;
        }

        private static bool InputIsValid(double userInput)
        {
            return userInput > 0;
        }

        private static double LookupBetaForBottomPanel(double length, double width)
        {
            double sidesRatio;

            // algorithm defines we always treat length as greater dimension
            // this implies ratio is always >= 1
            if (length > width)
                sidesRatio = length / width;
            else
                sidesRatio = width / length;

            if (sidesRatio < 1.5)
                return 0.4530;
            if (sidesRatio < 2.0)
                return 0.5172;
            if (sidesRatio < 2.5)
                return 0.5688;
            if (sidesRatio < 3.0)
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
