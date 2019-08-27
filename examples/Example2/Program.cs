using System;

namespace Example2
{
    class Program
    {
        static void Main()
        {
            double width = GetInput("Enter width [mm]: ");
            double length = GetInput("Enter length [mm]: ");
            double waterLevel = GetInput("Enter water level (height) [mm]: ");

            Aquarium aquarium = new Aquarium(width, length, waterLevel);

            Console.WriteLine();

            Inventory inventory = new Inventory();
            double[] selectedAreaPerThickness = inventory.SelectFromAvailablePanels(aquarium.Panels);

            Console.WriteLine();
            Console.WriteLine($"Total glass area is {TotalArea(aquarium.Panels):N1} m^2.");
            Console.WriteLine($"Total water volume is {aquarium.Volume:N1} l.");

            if (AquariumCanBeConstructed(selectedAreaPerThickness, aquarium.Panels))
            {
                PrintInvoice(inventory.availableGlassPanelThickness, selectedAreaPerThickness, inventory.glassPanelPrices);
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Could not construct aquarium from available glass panels.");
            }

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
