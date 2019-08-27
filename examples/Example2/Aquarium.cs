namespace Example2
{
    class Aquarium
    {
        private const double mmCubePerLiter = 1.0e6;
        private readonly GlassPanel[] panels;
        private readonly double width;
        private readonly double length;
        private readonly double waterLevel;

        public Aquarium(double width, double length, double waterLevel)
        {
            this.width = width;
            this.length = length;
            this.waterLevel = waterLevel;
            panels = CreatePanels(width, length, waterLevel);
        }

        public GlassPanel[] Panels
        {
            get
            {
                return panels;
            }
        }

        public double Volume
        {
            get
            {
                return width * length * waterLevel / mmCubePerLiter;
            }
        }

        private GlassPanel[] CreatePanels(double width, double length, double waterLevel)
        {
            GlassPanel[] requiredPanels = new GlassPanel[5];

            double betaBottomPanel = LookupBetaForBottomPanel(length, width);
            requiredPanels[0] = new GlassPanel("bottom panel", length, width, waterLevel, betaBottomPanel);

            double betaFrontPanel = LookupBetaForSidePanel(length, waterLevel);
            requiredPanels[1] = new GlassPanel("front panel", length, waterLevel, waterLevel, betaFrontPanel);
            requiredPanels[2] = new GlassPanel("back panel", length, waterLevel, waterLevel, betaFrontPanel);

            double betaSidePanel = LookupBetaForSidePanel(width, waterLevel);
            requiredPanels[3] = new GlassPanel("left panel", waterLevel, width, waterLevel, betaSidePanel);
            requiredPanels[4] = new GlassPanel("right panel", waterLevel, width, waterLevel, betaSidePanel);
            return requiredPanels;
        }

        private double LookupBetaForBottomPanel(double length, double width)
        {
            double sidesRatio;

            // algorithm defines we always treat length as greater dimension
            // this implies ratio is always >= 1
            if (length > width)
                sidesRatio = length / width;
            else
                sidesRatio = width / length;

            if (sidesRatio < 1.5)
                return 0.4530;
            if (sidesRatio < 2.0)
                return 0.5172;
            if (sidesRatio < 2.5)
                return 0.5688;
            if (sidesRatio < 3.0)
                return 0.6102;
            return 0.7134;
        }

        private double LookupBetaForSidePanel(double length, double waterLevel)
        {
            double sidesRatio = length / waterLevel;

            if (sidesRatio < 0.7)
                return 0.085;
            if (sidesRatio < 1.0)
                return 0.116;
            if (sidesRatio < 1.5)
                return 0.160;
            if (sidesRatio < 2.0)
                return 0.260;
            if (sidesRatio < 2.5)
                return 0.320;
            if (sidesRatio < 3.0)
                return 0.350;
            return 0.370;

        }

    }
}
