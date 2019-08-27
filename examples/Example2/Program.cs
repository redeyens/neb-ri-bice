using System;

namespace Example2
{
    class Program
    {
        static void Main()
        {
            double[] availableGlassPanelThickness = new double[] { 4.0, 6.0, 8.0, 10.0, 12.0, 15.0, 20.0, 30.0, 40.0, 50.0 };
            double[] glassPanelPrices = new double[] { 14.0, 18.0, 25.0, 31.0, 45.0, 100.0, 100.0, 55.0, 215.0, 300.0 };

            double width = GetInput("Enter width [mm]: ");
            double length = GetInput("Enter length [mm]: ");
            double waterLevel = GetInput("Enter water level (height) [mm]: ");

            Aquarium aquarium = new Aquarium(width, length, waterLevel);

            Console.WriteLine();

            double[] selectedAreaPerThickness = SelectFromAvailablePanels(aquarium.Panels, availableGlassPanelThickness);

            Console.WriteLine();
            Console.WriteLine($"Total glass area is {TotalArea(aquarium.Panels):N1} m^2.");
            Console.WriteLine($"Total water volume is {aquarium.Volume:N1} l.");

            if (AquariumCanBeConstructed(selectedAreaPerThickness, aquarium.Panels))
            {
                PrintInvoice(availableGlassPanelThickness, selectedAreaPerThickness, glassPanelPrices);
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Could not construct aquarium from available glass panels.");
            }

        }

        private static double[] SelectFromAvailablePanels(GlassPanel[] panels, double[] availableGlassPanelThickness)
        {
            double[] selectedAreaPerThickness = new double[availableGlassPanelThickness.Length];
            for (int i = 0; i < panels.Length; i++)
            {
                GlassPanel currentPanel = panels[i];
                double currentPanelThickness = GetAvailableGlassThickness(currentPanel.MinimumAllowedThickness, availableGlassPanelThickness);
                AggregateRequiredPanels(currentPanelThickness, currentPanel.Area, selectedAreaPerThickness, availableGlassPanelThickness);
                Console.WriteLine($"Required glass thickness for {currentPanel.Name} is {currentPanelThickness:N0} mm, minimum was {currentPanel.MinimumAllowedThickness:N1} mm.");
            }

            return selectedAreaPerThickness;
        }

        private static bool AquariumCanBeConstructed(double[] selectedAreaPerThickness, GlassPanel[] panels)
        {
            return Sum(selectedAreaPerThickness) == TotalArea(panels);
        }

        private static double TotalArea(GlassPanel[] panels)
        {
            double total = 0;
            for (int i = 0; i < panels.Length; i++)
            {
                total += panels[i].Area;
            }
            return total;
        }

        private static double Sum(double[] selectedAreaPerThickness)
        {
            double total = 0;
            for (int i = 0; i < selectedAreaPerThickness.Length; i++)
            {
                total += selectedAreaPerThickness[i];
            }
            return total;
        }

        private static void PrintInvoice(double[] availableGlassPanelThickness, double[] requiredGlassPanelArea, double[] glassPanelPrices)
        {
            Console.WriteLine();
            Console.WriteLine($"Item \t\t\t Qty \t Unit \t Unit Price \t Price");
            Console.WriteLine("------------------------------------------------------------------");

            double totalPrice = 0.0;
            for (int i = 0; i < availableGlassPanelThickness.Length; i++)
            {
                if (requiredGlassPanelArea[i] > 0.0)
                {
                    double itemPrice = requiredGlassPanelArea[i] * glassPanelPrices[i];
                    totalPrice += itemPrice;

                    Console.WriteLine($"Clear glass {availableGlassPanelThickness[i]:N0} mm\t{requiredGlassPanelArea[i]:N1} \t m^2 \t {glassPanelPrices[i]:N2} \t\t {itemPrice}");
                }
            }

            Console.WriteLine("==================================================================");
            Console.WriteLine($"\t\t\t\t\t\tTotal:\t {totalPrice}");

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


    }
}
