using System;

namespace neb_ri_bice
{
    class Program
    {
        static void Main(string[] args)
        {            
            const double SafetyFactor = 3.8;
            const int GravityAcceleration = 10;
            const int GlassElasticity = 69000;
            const int WaterPressure = 5000;
            

            double bendingStressAllowed = CalculateBendingStressAllowed(SafetyFactor);            

            int height = GetDimension("height");
            int width = GetDimension("width");
            int depth = GetDimension("depth");

            int bottomPanelArea = CalculateArea(width, depth);
            int sidePanelsArea = CalculateArea(height, depth);
            int frontAndBackPanelsArea = CalculateArea(width, height);
            int totalGlassArea = bottomPanelArea + sidePanelsArea * 2 + frontAndBackPanelsArea * 2;

            double volume = CalculateVolume(height, width, depth);

            double[] frontAndBackThicknessDeflection = CalculateSideGlassThicknessAndDeflection(height, width, bendingStressAllowed, WaterPressure, GlassElasticity);
            double[] sidesThicknessDeflection = CalculateSideGlassThicknessAndDeflection(height, depth, bendingStressAllowed, WaterPressure, GlassElasticity);
            double[] botomThicknessDeflection = CalculateBottomGlassThicknessAndDeflection(width, depth, height, bendingStressAllowed, WaterPressure, GlassElasticity);

            double totalGlassVolume = CalculateVolume(bottomPanelArea, botomThicknessDeflection[0]) 
                + CalculateVolume(sidePanelsArea, sidesThicknessDeflection[0]) 
                + CalculateVolume(frontAndBackPanelsArea, frontAndBackThicknessDeflection[0]);

            PrintResult(height,width,depth,totalGlassArea,CalculateVolume(height, width, depth), 
                sidesThicknessDeflection[0], frontAndBackThicknessDeflection[0], botomThicknessDeflection[0], totalGlassVolume);
            
        }

        static void PrintResult(int height, int width, int depth, int totalGlassArea, double waterVolume, double sideThickness, double frontAndBackThickness, double bottomThickness, double totalGlassVolume)
        {
            Console.WriteLine($"\n\n\nDimensions of the aquarium are {width} mm (base) x {depth} mm (base) x {height} mm (height).\n");
            Console.WriteLine($"Total glass area is {totalGlassArea / 100} cm\u00b2.\n");
            Console.WriteLine($"Maximum water volume is {waterVolume / 1000000} litres.\n");
            Console.WriteLine($"Side panels thickness is {Math.Round(sideThickness, 2)} mm.");
            Console.WriteLine($"Front and back panels thickness is {Math.Round(frontAndBackThickness, 2)} mm.");
            Console.WriteLine($"Bottom panel thickness is {Math.Round(bottomThickness, 2)} mm.\n");
            Console.WriteLine($"Total required glass volume amounts to {Math.Round(totalGlassVolume / 1000, 2)} cm\u00b3.");
            Console.ReadKey();
        }

        static int GetDimension(string dimensionType)        
        {
            int retVal = 0;
            Console.WriteLine($"Input aquarium {dimensionType} in mm and press ENTER: ");

            while (!int.TryParse(Console.ReadLine(), out retVal))
            {
                Console.WriteLine("Must be a whole number! Try again...");
                Console.WriteLine($"Input aquarium {dimensionType} in mm and press ENTER: ");
            }

            return retVal;
        }

        static int CalculateArea(int a, int b)
        {
            int retVal = a * b;
            return retVal;
        }

        static double CalculateVolume(double a, double b, double c)
        {
            double retVal = a * b * c;
            return retVal;
        }

        static double CalculateVolume(double area, double x)
        {
            double retVal = area * x;
            return retVal;
        }

        static double CalculateBendingStressAllowed(double safetyFactor)
        {
            double retVal = 19.2 / safetyFactor;
            return retVal;
        }

        static double[] CalculateSideGlassThicknessAndDeflection(int verticalX, int horizontalY, double bendingStress, int waterPressure, int elasticity)
        {
            double ratio = (double)horizontalY / (double)verticalX;

            double alpha = 0;
            double beta = 0;            

            switch(ratio)
            {
                case double x when (ratio >= 0 && ratio <= 0.5):
                    alpha = 0.003;
                    beta = 0.085;                    
                    break;                  
                case double x when (ratio > 0.5 && ratio <= 0.7):
                    alpha = 0.009;
                    beta = 0.116;                    
                    break;               
                case double x when (ratio > 0.7 && ratio <= 1):
                    alpha = 0.022;
                    beta = 0.160;                    
                    break;               
                case double x when (ratio > 1 && ratio <= 1.5):
                    alpha = 0.0442;
                    beta = 0.260;                    
                    break;               
                case double x when (ratio > 1.5 && ratio <= 2):
                    alpha = 0.056;
                    beta = 0.320;                    
                    break;               
                case double x when (ratio > 2 && ratio <= 2.5):
                    alpha = 0.063;
                    beta = 0.350;                    
                    break;               
                case double x when (ratio > 2.5):
                    alpha = 0.067;
                    beta = 0.370;                    
                    break;               
            }

            double thickness = Math.Sqrt(beta * Math.Pow(verticalX, 3) * (Math.Pow(10, -5) / bendingStress));

            double deflection = alpha * waterPressure * (Math.Pow(10, -6) * Math.Pow(verticalX, 4)) / ( elasticity * Math.Pow(thickness, 3));

            double[] retVal = new double[2] { thickness, deflection };

            return retVal;
        }

        static double[] CalculateBottomGlassThicknessAndDeflection(int horizontalX, int horizontalY, int verticalZ, double bendingStress, int waterPressure, int elasticity)
        {
            double ratio = (double)horizontalX / (double)horizontalY;

            double alpha = 0;
            double beta = 0;            

            switch(ratio)
            {
                case double x when (ratio >= 0 && ratio <= 1):
                    alpha = 0.0770;
                    beta = 0.4530;                    
                    break;                  
                case double x when (ratio > 1 && ratio <= 1.5):
                    alpha = 0.0906;
                    beta = 0.5172;                    
                    break;               
                case double x when (ratio > 1.5 && ratio <= 2):
                    alpha = 0.1017;
                    beta = 0.5688;                    
                    break;               
                case double x when (ratio > 2 && ratio <= 2.5):
                    alpha = 0.1110;
                    beta = 0.6102;                    
                    break;               
                case double x when (ratio > 2.5):
                    alpha = 0.1335;
                    beta = 0.7134;                    
                    break;                               
            }

            double thickness = Math.Sqrt(beta * Math.Pow(verticalZ, 3) * (Math.Pow(10, -5) / bendingStress));
            double deflection = alpha * waterPressure * (Math.Pow(10, -6) * Math.Pow(verticalZ, 4)) / ( elasticity * Math.Pow(thickness, 3));
            double[] retVal = new double[2] { thickness, deflection };

            return retVal;
        }
    }
}

