namespace Example2
{
    public class InvoiceItem
    {
        private readonly string name;
        private readonly double quantity;
        private readonly string unit;
        private readonly double unitPrice;

        public InvoiceItem(string name, double quantity, string unit, double unitPrice)
        {
            this.name = name;
            this.quantity = quantity;
            this.unit = unit;
            this.unitPrice = unitPrice;
        }

        public string Name => name;

        public double Quantity => quantity;

        public string Unit => unit;

        public double UnitPrice => unitPrice;
        public double Price => quantity * unitPrice;
    }
}