namespace Example2
{
    public class AquariumDimensions
    {
        private readonly double width;
        private readonly double length;
        private readonly double waterLevel;

        public AquariumDimensions(double width, double length, double waterLevel)
        {
            this.width = width;
            this.length = length;
            this.waterLevel = waterLevel;
        }

        public double Width => width;

        public double Length => length;

        public double WaterLevel => waterLevel;
    }
}
