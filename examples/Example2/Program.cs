﻿using System;

namespace Example2
{
    class Program
    {
        private const double mmCubePerLiter = 1.0e6;
        private const double mmSquaredPerMeterSquared = 1.0e6;

        static void Main()
        {
            double[] availableGlassPanelThickness = new double[] { 4.0, 6.0, 8.0, 10.0, 12.0, 15.0, 20.0, 30.0, 40.0, 50.0 };
            double[] requiredGlassPanelArea = new double[availableGlassPanelThickness.Length];

            double width = GetInput("Enter width [mm]: ");
            double length = GetInput("Enter length [mm]: ");
            double waterLevel = GetInput("Enter water level (height) [mm]: ");

            double frontPanelAreaMeters = length * waterLevel / mmSquaredPerMeterSquared;
            double sidePanelAreaMeters = width * waterLevel / mmSquaredPerMeterSquared;
            double bottomPanelAreaMeters = length * width / mmSquaredPerMeterSquared;

            double betaBottomPanel = LookupBetaForBottomPanel(length, width);
            double bottomPanelMinimumThickness = GlassThickness(waterLevel, betaBottomPanel);
            double bottomPanelThickness = GetAvailableGlassThickness(bottomPanelMinimumThickness, availableGlassPanelThickness);
            AggregateRequiredPanels(bottomPanelThickness, bottomPanelAreaMeters, requiredGlassPanelArea, availableGlassPanelThickness);

            double betaFrontPanel = LookupBetaForSidePanel(length, waterLevel);
            double frontPanelMinimumThickness = GlassThickness(waterLevel, betaFrontPanel);
            double frontPanelThickness = GetAvailableGlassThickness(frontPanelMinimumThickness, availableGlassPanelThickness);
            AggregateRequiredPanels(frontPanelThickness, frontPanelAreaMeters, requiredGlassPanelArea, availableGlassPanelThickness);

            double betaSidePanel = LookupBetaForSidePanel(width, waterLevel);
            double sidePanelMinimumThickness = GlassThickness(waterLevel, betaSidePanel);
            double sidePanelThickness = GetAvailableGlassThickness(sidePanelMinimumThickness, availableGlassPanelThickness);
            AggregateRequiredPanels(sidePanelThickness, sidePanelAreaMeters, requiredGlassPanelArea, availableGlassPanelThickness);

            Console.WriteLine();
            Console.WriteLine($"Required glass thickness for bottom panel is {bottomPanelThickness:N0} mm, minimum was {bottomPanelMinimumThickness:N1} mm.");
            Console.WriteLine($"Required glass thickness for front/back panel {frontPanelThickness:N0} mm, minimum was is {frontPanelMinimumThickness:N1} mm.");
            Console.WriteLine($"Required glass thickness for left/right panel {sidePanelThickness:N0} mm, minimum was is {sidePanelMinimumThickness:N1} mm.");

            Console.WriteLine();
            Console.WriteLine($"Total glass area is {TotalGlassArea(bottomPanelAreaMeters, frontPanelAreaMeters, sidePanelAreaMeters):N1} m^2.");
            Console.WriteLine($"Total water volume is {WaterVolumeLiters(width, length, waterLevel):N1} l.");

            PrintInvoice(availableGlassPanelThickness, requiredGlassPanelArea);

        }

        private static void PrintInvoice(double[] availableGlassPanelThickness, double[] requiredGlassPanelArea)
        {
            Console.WriteLine();
            Console.WriteLine($"Item \t\t\t Qty");
            Console.WriteLine("---------------------------------");

            for (int i = 0; i < availableGlassPanelThickness.Length; i++)
            {
                if (requiredGlassPanelArea[i] > 0.0)
                {
                    Console.WriteLine($"Clear glass {availableGlassPanelThickness[i]:N0} mm\t{requiredGlassPanelArea[i]:N1} m^2");
                }
            }
        }

        private static void AggregateRequiredPanels(double panelThickness, double panelArea, double[] requiredGlassPanelArea, double[] availableGlassPanelThickness)
        {
            for (int i = 0; i < availableGlassPanelThickness.Length; i++)
            {
                if (panelThickness == availableGlassPanelThickness[i])
                {
                    requiredGlassPanelArea[i] += panelArea;
                }
            }
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

        private static object TotalGlassArea(double bottomPanelArea, double frontPanelArea, double sidePanelArea)
        {
            return (bottomPanelArea + 2 * frontPanelArea + 2 * sidePanelArea);
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
