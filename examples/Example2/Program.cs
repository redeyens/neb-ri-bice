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

            Console.WriteLine();
            Console.WriteLine($"Total glass area is {TotalArea(aquarium.Panels):N1} m^2.");
            Console.WriteLine($"Total water volume is {aquarium.Volume:N1} l.");

            Invoice invoice = inventory.CreateInvoice(aquarium.Panels);
            if (invoice.IsEmpty)
            {
                Console.WriteLine();
                Console.WriteLine("Could not construct aquarium from available glass panels.");
            }
            else
            {
                PrintInvoice(invoice);
            }

        }

        private static void PrintInvoice(Invoice invoice)
        {
            Console.WriteLine();
            PrintInvoiceHeader();

            double totalPrice = 0.0;
            foreach (var item in invoice.Items)
            {
                PrintInvoiceItem(item);
                totalPrice += item.Price;
            }

            PrintInvoiceFooter(totalPrice);
        }

        private static void PrintInvoiceFooter(double totalPrice)
        {
            Console.WriteLine("================================================================================");
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
            Console.WriteLine($"{titleName,-40} {titleQuantity,9:N1} {titleUnit,-7} {titleUnitPrice,10:N2} {titlePrice,10:N2}");
            Console.WriteLine("--------------------------------------------------------------------------------");
        }

        private static void PrintInvoiceItem(InvoiceItem item)
        {
            Console.WriteLine($"{item.Name,-40} {item.Quantity,9:N1} {item.Unit,-7} {item.UnitPrice,10:N2} {item.Price,10:N2}");
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
