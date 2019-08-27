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
            Console.WriteLine($"Total glass area is {TotalArea(aquarium.Panels):N1} m^2.");
            Console.WriteLine($"Total water volume is {aquarium.Volume:N1} l.");
            Console.WriteLine();

            Inventory inventory = new Inventory();
            Invoice invoice = inventory.CreateInvoice(aquarium.Panels);
            InvoiceView invoiceView = new InvoiceView(invoice);

            if (invoice.IsEmpty)
            {
                Console.WriteLine();
                Console.WriteLine("Could not construct aquarium from available glass panels.");
            }
            else
            {
                invoiceView.Show();
            }

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

        private static double TotalArea(GlassPanel[] panels)
        {
            double total = 0;
            for (int i = 0; i < panels.Length; i++)
            {
                total += panels[i].Area;
            }
            return total;
        }


    }
}
