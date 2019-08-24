using System;

namespace neb_ri_bice
{
    class Program
    {
        static void Main(string[] args)
        {  
            const double bendingStressAllowed = 5.05;

            int height = GetDimension("height");
            int width = GetDimension("width");
            int depth = GetDimension("depth");

            int bottomPanelArea = CalculateArea(width, depth);
            int sidePanelsArea = CalculateArea(height, depth);
            int frontAndBackPanelsArea = CalculateArea(width, height);
            int totalGlassArea = bottomPanelArea + sidePanelsArea * 2 + frontAndBackPanelsArea * 2;

            double volume = CalculateVolume(height, width, depth);

            double frontAndBackThicknessDeflection = CalculateSideGlassThickness(height, width, bendingStressAllowed);
            double sidesThicknessDeflection = CalculateSideGlassThickness(height, depth, bendingStressAllowed);
            double botomThicknessDeflection = CalculateBottomGlassThickness(width, depth, height, bendingStressAllowed);

            PrintResult(height, width, depth, totalGlassArea, volume, 
                sidesThicknessDeflection, frontAndBackThicknessDeflection, botomThicknessDeflection);            
        }

        static void PrintResult(int height, int width, int depth, int totalGlassArea, double waterVolume, double sideThickness, double frontAndBackThickness, double bottomThickness)
        {
            Console.WriteLine($"\n\n\nDimensions of the aquarium are {width} mm (base) x {depth} mm (base) x {height} mm (height).\n");
            Console.WriteLine($"Total glass area is {Math.Round(((double)(totalGlassArea / 1000000)), 2)} m\u00b2.\n");
            Console.WriteLine($"Maximum water volume is {waterVolume / 1000000} litres.\n");
            Console.WriteLine($"Side panels thickness is {Math.Round(sideThickness, 2)} mm.");
            Console.WriteLine($"Front and back panels thickness is {Math.Round(frontAndBackThickness, 2)} mm.");
            Console.WriteLine($"Bottom panel thickness is {Math.Round(bottomThickness, 2)} mm.\n");

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

        static int CalculateWaterPressure(int height)
        {
            return height * 10;
        }

        static double CalculateVolume(double a, double b, double c)
        {
            double retVal = a * b * c;
            return retVal;
        }

        static double CalculateSideGlassThickness(int verticalX, int horizontalY, double bendingStress)
        {
            double ratio = (double)horizontalY / (double)verticalX;
            int waterPressure = CalculateWaterPressure(verticalX);
            
            double beta = 0;            

            switch(ratio)
            {
                case double x when (x >= 0 && x <= 0.5):                    
                    beta = 0.085;                    
                    break;                  
                case double x when (x > 0.5 && x <= 0.7):                   
                    beta = 0.116;                    
                    break;               
                case double x when (x > 0.7 && x <= 1):                    
                    beta = 0.160;                    
                    break;               
                case double x when (x > 1 && x <= 1.5):                    
                    beta = 0.260;                    
                    break;               
                case double x when (x > 1.5 && x <= 2):                    
                    beta = 0.320;                    
                    break;               
                case double x when (x > 2 && x <= 2.5):                    
                    beta = 0.350;                    
                    break;               
                case double x when (x > 2.5):                    
                    beta = 0.370;                    
                    break;               
            }

            double thickness = Math.Sqrt(beta * Math.Pow(verticalX, 3) * (Math.Pow(10, -5) / bendingStress));

            return thickness;
        }

        static double CalculateBottomGlassThickness(int horizontalX, int horizontalY, int verticalZ, double bendingStress)
        {
            double ratio = (double)horizontalX / (double)horizontalY;
            int waterPressure = CalculateWaterPressure(verticalZ);
            
            double beta = 0;            

            switch(ratio)
            {
                case double x when (x >= 0 && x <= 1):                    
                    beta = 0.4530;                    
                    break;                  
                case double x when (x > 1 && x <= 1.5):                    
                    beta = 0.5172;                    
                    break;               
                case double x when (x > 1.5 && x <= 2):                    
                    beta = 0.5688;                    
                    break;               
                case double x when (x > 2 && x <= 2.5):                    
                    beta = 0.6102;                    
                    break;               
                case double x when (x > 2.5):                    
                    beta = 0.7134;                    
                    break;                               
            }

            double thickness = Math.Sqrt(beta * Math.Pow(verticalZ, 3) * (Math.Pow(10, -5) / bendingStress));            

            return thickness;
        }
    }
}

