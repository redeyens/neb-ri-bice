using System;

namespace Example2
{
    class GlassPanel
    {
        private const double mmSquaredPerMeterSquared = 1.0e6;

        private readonly double waterLevel;
        private readonly double betaCoef;
        private readonly double length;
        private readonly double width;

        public GlassPanel(double length, double width, double waterLevel, double betaCoef)
        {
            this.length = length;
            this.width = width;
            this.waterLevel = waterLevel;
            this.betaCoef = betaCoef;
        }

        public double MinimumAllowedThickness
        {
            get
            {
                return GlassThickness(waterLevel, betaCoef);
            }
        }

        public double Area
        {
            get
            {
                return length * width / mmSquaredPerMeterSquared;
            }
        }

        private double GlassThickness(double waterLevel, double beta)
        {
            double scalingFactor = 0.00001;
            double glassTensileStrength = 19.2;
            double safetyFactor = 3.8;

            double maxBendingStress = glassTensileStrength / safetyFactor;

            return Math.Sqrt(beta * Math.Pow(waterLevel, 3) * scalingFactor / maxBendingStress);
        }
    }
}
