using System.Collections.Generic;

namespace Example2
{
    public class Invoice
    {
        private readonly List<InvoiceItem> items;

        public Invoice()
        {
        }

        public Invoice(List<InvoiceItem> invoiceItems)
        {
            this.items = invoiceItems;
        }

        public static Invoice Empty { get; } = new Invoice();
        public List<InvoiceItem> Items 
        { 
            get
            {
                return items;
            }
        }
        public bool IsEmpty 
        { 
            get
            {
                return this == Empty;
            }
        }
    }
}