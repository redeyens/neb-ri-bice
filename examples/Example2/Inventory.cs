using System;
using System.Collections.Generic;

namespace Example2
{
    class Inventory
    {
        private readonly double[] availableGlassPanelThickness = new double[] { 4.0, 6.0, 8.0, 10.0, 12.0, 15.0, 20.0, 30.0, 40.0, 50.0 };
        private readonly double[] glassPanelPrices = new double[] { 14.0, 18.0, 25.0, 31.0, 45.0, 100.0, 100.0, 55.0, 215.0, 300.0 };

        public Invoice CreateInvoice(GlassPanel[] panels)
        {
            double[] requiredGlassPanelArea = SelectFromAvailablePanels(panels);
            Invoice result = Invoice.Empty;

            List<InvoiceItem> invoiceItems = new List<InvoiceItem>();
            for (int i = 0; i < requiredGlassPanelArea.Length; i++)
            {
                if (requiredGlassPanelArea[i] > 0.0)
                {
                    InvoiceItem item = new InvoiceItem($"Clear glass {availableGlassPanelThickness[i]:N0} mm", requiredGlassPanelArea[i], "m^2", glassPanelPrices[i]);
                    invoiceItems.Add(item);
                }
            }
            if(OrderCoveredCompletely(invoiceItems, panels))
            {
                result = new Invoice(invoiceItems);
            }

            return result;
        }

        private double[] SelectFromAvailablePanels(GlassPanel[] panels)
        {
            double[] selectedAreaPerThickness = new double[availableGlassPanelThickness.Length];
            for (int i = 0; i < panels.Length; i++)
            {
                GlassPanel currentPanel = panels[i];
                double currentPanelThickness = GetAvailableGlassThickness(currentPanel.MinimumAllowedThickness);
                AggregateRequiredPanels(currentPanelThickness, currentPanel.Area, selectedAreaPerThickness);
                Console.WriteLine($"Required glass thickness for {currentPanel.Name} is {currentPanelThickness:N0} mm, minimum was {currentPanel.MinimumAllowedThickness:N1} mm.");
            }

            return selectedAreaPerThickness;
        }

        private double GetAvailableGlassThickness(double minimumThickness)
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

        private void AggregateRequiredPanels(double panelThickness, double panelArea, double[] requiredGlassPanelArea)
        {
            for (int i = 0; i < availableGlassPanelThickness.Length; i++)
            {
                if (panelThickness == availableGlassPanelThickness[i])
                {
                    requiredGlassPanelArea[i] += panelArea;
                }
            }
        }

        private bool OrderCoveredCompletely(List<InvoiceItem> invoiceItems, GlassPanel[] panels)
        {
            return SumQuantity(invoiceItems) == TotalArea(panels);
        }

        private double TotalArea(GlassPanel[] panels)
        {
            double total = 0;
            for (int i = 0; i < panels.Length; i++)
            {
                total += panels[i].Area;
            }
            return total;
        }

        private double SumQuantity(List<InvoiceItem> invoiceItems)
        {
            double total = 0;
            for (int i = 0; i < invoiceItems.Count; i++)
            {
                total += invoiceItems[i].Quantity;
            }
            return total;
        }


    }
}
