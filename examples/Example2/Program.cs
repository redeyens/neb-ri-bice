using System;

namespace Example2
{
    class Program
    {
        private const double mmCubePerLiter = 1.0e6;
        private const double mmSquaredPerMeterSquared = 1.0e6;

        static void Main()
        {
            double[] availableGlassPanelThickness = new double[] { 4.0, 6.0, 8.0, 10.0, 12.0, 15.0, 20.0, 30.0, 40.0, 50.0 };


            double width = GetInput("Enter width [mm]: ");
            double length = GetInput("Enter length [mm]: ");
            double waterLevel = GetInput("Enter water level (height) [mm]: ");

            double betaBottomPanel = LookupBetaForBottomPanel(length, width);
            double bottomMinimumThickness = GlassThickness(waterLevel, betaBottomPanel);
            double bottomThickness = GetAvailableGlassThickness(bottomMinimumThickness, availableGlassPanelThickness);

            double betaFrontPanel = LookupBetaForSidePanel(length, waterLevel);
            double frontPanelMinimumThickness = GlassThickness(waterLevel, betaFrontPanel);
            double frontPanelThickness = GetAvailableGlassThickness(frontPanelMinimumThickness, availableGlassPanelThickness);

            double betaSidePanel = LookupBetaForSidePanel(width, waterLevel);
            double sidePanelMinimumThickness = GlassThickness(waterLevel, betaSidePanel);
            double sidePanelThickness = GetAvailableGlassThickness(sidePanelMinimumThickness, availableGlassPanelThickness);

            Console.WriteLine();
            Console.WriteLine($"Required glass thickness for bottom panel is {bottomThickness:N1} mm, minimum was {bottomMinimumThickness:N1} mm.");
            Console.WriteLine($"Required glass thickness for front/back panel {frontPanelThickness:N1} mm, minimum was is {frontPanelMinimumThickness:N1} mm.");
            Console.WriteLine($"Required glass thickness for left/right panel {sidePanelThickness:N1} mm, minimum was is {sidePanelMinimumThickness:N1} mm.");

            Console.WriteLine();
            Console.WriteLine($"Total glass area is {TotalGlassAreaMeters(width, length, waterLevel):N1} m^2.");
            Console.WriteLine($"Total water volume is {WaterVolumeLiters(width, length, waterLevel):N1} l.");

        }

        private static double GetAvailableGlassThickness(double minimumThickness, double[] availableGlassPanelThickness)
        {
            for (int i = 0; i < availableGlassPanelThickness.Length; i++)
            {
                if (availableGlassPanelThickness[i] >= minimumThickness)
                {
                    return availableGlassPanelThickness[i];
                }
            }

            return 0;
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
