using System;

namespace Example2
{
    class Inventory
    {
        public readonly double[] availableGlassPanelThickness = new double[] { 4.0, 6.0, 8.0, 10.0, 12.0, 15.0, 20.0, 30.0, 40.0, 50.0 };
        public readonly double[] glassPanelPrices = new double[] { 14.0, 18.0, 25.0, 31.0, 45.0, 100.0, 100.0, 55.0, 215.0, 300.0 };

        public double[] SelectFromAvailablePanels(GlassPanel[] panels)
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

    }
}
