using System;

namespace Example2
{

    class Program
    {
        static void Main()
        {
            NewAquariumDialog newAquariumDlg = new NewAquariumDialog();
            newAquariumDlg.Show();

            Aquarium aquarium = new Aquarium(newAquariumDlg.Dimensions);
            AquariumSummaryView aqSummary = new AquariumSummaryView(aquarium);

            aqSummary.Show();

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

    }
}
