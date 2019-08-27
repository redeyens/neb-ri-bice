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
            double[] glassPanelPrices = new double[] { 14.0, 18.0, 25.0, 31.0, 45.0, 100.0, 100.0, 55.0, 215.0, 300.0 };
            double[] requiredGlassPanelArea = new double[availableGlassPanelThickness.Length];

            double width = GetInput("Enter width [mm]: ");
            double length = GetInput("Enter length [mm]: ");
            double waterLevel = GetInput("Enter water level (height) [mm]: ");
            Console.WriteLine();

            double bottomPanelAreaMeters;
            double betaBottomPanel = LookupBetaForBottomPanel(length, width);
            GetPanel("bottom", availableGlassPanelThickness, requiredGlassPanelArea, width, length, waterLevel, out bottomPanelAreaMeters,  betaBottomPanel);

            double frontPanelAreaMeters, backPanelAreaMeters, leftPanelAreaMeters, rightPanelAreaMeters;
            double betaSidePanel = LookupBetaForSidePanel(width, length);
            GetPanel("front", availableGlassPanelThickness, requiredGlassPanelArea, length, waterLevel, waterLevel, out frontPanelAreaMeters, betaSidePanel);
            GetPanel("back", availableGlassPanelThickness, requiredGlassPanelArea, length, waterLevel, waterLevel, out backPanelAreaMeters, betaSidePanel);

            GetPanel("left", availableGlassPanelThickness, requiredGlassPanelArea, width, waterLevel, waterLevel, out leftPanelAreaMeters, betaSidePanel);
            GetPanel("right", availableGlassPanelThickness, requiredGlassPanelArea, width, waterLevel, waterLevel, out rightPanelAreaMeters, betaSidePanel);

            double totalGlassArea = bottomPanelAreaMeters + frontPanelAreaMeters + backPanelAreaMeters + leftPanelAreaMeters + rightPanelAreaMeters;
            double invoiceGlassArea = Sum(requiredGlassPanelArea);
            Console.WriteLine();
            Console.WriteLine($"Total glass area is {totalGlassArea:N1} m^2.");
            Console.WriteLine($"Total water volume is {WaterVolumeLiters(width, length, waterLevel):N1} l.");

            if (AquariumCanBeConstructed(totalGlassArea, invoiceGlassArea))
            {
                PrintInvoice(availableGlassPanelThickness, requiredGlassPanelArea, glassPanelPrices);
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Could not construct aquarium from available glass panels.");
            }

        }

        private static double Sum(double[] values)
        {
            double sum = 0;
            for (int i = 0; i < values.Length; i++)
            {
                sum += values[i];
            }
            return sum;
        }

        private static void GetPanel(string whichPanel, double[] availableGlassPanelThickness, double[] requiredGlassPanelArea, double width, double length, double waterLevel, out double panelAreaMeters, double betaSidePanel)
        {
            panelAreaMeters = width * length / mmSquaredPerMeterSquared;
            double panelMinimumThickness = GlassThickness(waterLevel, betaSidePanel);
            double panelThickness = GetAvailableGlassThickness(panelMinimumThickness, availableGlassPanelThickness);
            AggregateRequiredPanels(panelThickness, panelAreaMeters, requiredGlassPanelArea, availableGlassPanelThickness);
            Console.WriteLine($"Required glass thickness for {whichPanel} panel {panelThickness:N0} mm, minimum was {panelMinimumThickness:N1} mm.");
        }

        private static bool AquariumCanBeConstructed(double totalGlassArea, double invoiceGlassArea)
        {
            return totalGlassArea == invoiceGlassArea;
        }

        private static void PrintInvoice(double[] availableGlassPanelThickness, double[] requiredGlassPanelArea, double[] glassPanelPrices)
        {
            PrintInvoiceHeader();

            double totalPrice = 0.0;
            for (int i = 0; i < availableGlassPanelThickness.Length; i++)
            {
                if (requiredGlassPanelArea[i] > 0.0)
                {
                    double itemPrice = requiredGlassPanelArea[i] * glassPanelPrices[i];
                    totalPrice += itemPrice;

                    PrintInvoiceItem($"Clear glass {availableGlassPanelThickness[i]:N0} mm",
                                     requiredGlassPanelArea[i],
                                     "m^2",
                                     glassPanelPrices[i],
                                     itemPrice);
                }
            }

            PrintInvoiceFooter(totalPrice);
        }

        private static void PrintInvoiceFooter(double totalPrice)
        {
            Console.WriteLine(new String('=', 80));
            object titleTotal = "Total:";
            Console.WriteLine($"{titleTotal,69} {totalPrice,10:N2}");
        }

        private static void PrintInvoiceHeader()
        {
            string titleName = "Item";
            string titleQuantity = "Quantity";
            string titleUnit = "Unit";
            string titleUnitPrice = "Unit Price";
            string titlePrice = "Price";
            Console.WriteLine();
            Console.WriteLine($"{titleName,-40} {titleQuantity,9:N1} {titleUnit,-7} {titleUnitPrice,10:N2} {titlePrice,10:N2}");
            Console.WriteLine(new String('-', 80));
        }

        private static void PrintInvoiceItem(string name, double quantity, string unit, double unitPrice, double price)
        {
            Console.WriteLine($"{name,-40} {quantity,9:N1} {unit,-7} {unitPrice,10:N2} {price,10:N2}");
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
