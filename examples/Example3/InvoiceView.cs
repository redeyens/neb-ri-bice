using System;

namespace Example3
{
    class InvoiceView
    {
        public InvoiceView(Invoice invoice)
        {
            Invoice = invoice;
        }

        public Invoice Invoice { get; set; }
        public void Show()
        {
            Console.WriteLine();
            PrintInvoiceHeader();

            double totalPrice = 0.0;
            foreach (var item in Invoice.Items)
            {
                PrintInvoiceItem(item);
                totalPrice += item.Price;
            }

            PrintInvoiceFooter(totalPrice);
        }

        private void PrintInvoiceFooter(double totalPrice)
        {
            Console.WriteLine("================================================================================");
            object titleTotal = "Total:";
            Console.WriteLine($"{titleTotal,69} {totalPrice,10:N2}");
        }

        private void PrintInvoiceHeader()
        {
            string titleName = "Item";
            string titleQuantity = "Quantity";
            string titleUnit = "Unit";
            string titleUnitPrice = "Unit Price";
            string titlePrice = "Price";
            Console.WriteLine($"{titleName,-40} {titleQuantity,9:N1} {titleUnit,-7} {titleUnitPrice,10:N2} {titlePrice,10:N2}");
            Console.WriteLine("--------------------------------------------------------------------------------");
        }

        private void PrintInvoiceItem(InvoiceItem item)
        {
            Console.WriteLine($"{item.Name,-40} {item.Quantity,9:N1} {item.Unit,-7} {item.UnitPrice,10:N2} {item.Price,10:N2}");
        }

    }
}
