using System;

namespace Example3
{
    class AquariumSummaryView
    {
        public AquariumSummaryView(Aquarium aquarium)
        {
            Aquarium = aquarium;
        }

        public Aquarium Aquarium { get; set; }

        public void Show()
        {
            Console.WriteLine();
            Console.WriteLine($"Total glass area is {TotalArea(Aquarium.Panels):N1} m^2.");
            Console.WriteLine($"Total water volume is {Aquarium.Volume:N1} l.");
            Console.WriteLine();
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

    }
}
