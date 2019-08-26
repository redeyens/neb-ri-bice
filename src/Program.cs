using System;

namespace neb_ri_bice
{
    class Program
    {
        static void Main(string[] args)
        {  
            const double bendingStressAllowed = 5.05;
            
            double height = GetDimension("height");
            double width = GetDimension("width");
            double depth = GetDimension("depth");

            double bottomPanelArea = CalculateAreaMeter(width, depth);
            double sidePanelsArea = CalculateAreaMeter(height, depth);
            double frontPanelsArea = CalculateAreaMeter(width, height);
            double totalGlassArea = bottomPanelArea + sidePanelsArea * 2 + frontPanelsArea * 2;

            double volume = CalculateVolumeLitres(height, width, depth);

            double frontMinimumThickness = CalculateSideGlassThickness(height, width, bendingStressAllowed);
            double sidesMinimumThickness = CalculateSideGlassThickness(height, depth, bendingStressAllowed);
            double bottomMinimumThickness = CalculateBottomGlassThickness(width, depth, height, bendingStressAllowed);

            double frontThickness = FinalThickness(frontMinimumThickness);
            double sidesThickness = FinalThickness(sidesMinimumThickness);
            double bottomThickness = FinalThickness(bottomMinimumThickness);

            double frontPriceMeterSquared = PanelPriceMeterSquared(frontThickness);
            double sidesPriceMeterSquared = PanelPriceMeterSquared(sidesThickness);
            double bottomPriceMeterSquared = PanelPriceMeterSquared(bottomThickness);

            double frontTotalPrice = frontPriceMeterSquared * frontPanelsArea;
            double sideTotalPrice = sidesPriceMeterSquared * sidePanelsArea;
            double bottomTotalPrice = bottomPriceMeterSquared * bottomPanelArea;

            double totalPanelPrice = totalPrice(frontTotalPrice, sideTotalPrice, bottomTotalPrice);                

            Console.WriteLine($"\n\n\nDimensions of the aquarium are {width} mm (base) x {depth} mm (base) x {height} mm (height).\n");
            Console.WriteLine($"Total glass area is {Math.Round(totalGlassArea, 2)} m\u00b2.\n");
            Console.WriteLine($"Maximum water volume is {volume} litres.\n");
            Console.WriteLine($"Side panels minimum thickness is {Math.Round(sidesMinimumThickness, 2)} mm.");
            Console.WriteLine($"Front and back panels minimum thickness is {Math.Round(frontMinimumThickness, 2)} mm.");
            Console.WriteLine($"Bottom panel minimum thickness is {Math.Round(bottomMinimumThickness, 2)} mm.\n\n\n");
            Console.WriteLine("Panels needed:");
            Console.WriteLine("\tFront and back panels:"); 
            Console.WriteLine($"\t\t2 panels with dimensions {height} mm x {width} mm x {frontThickness} mm");
            Console.WriteLine($"\t\tTotal area {frontPanelsArea * 2} m\u00b2");
            Console.WriteLine($"\t\tprice per m\u00b2 {frontPriceMeterSquared} EUR"); 
            Console.WriteLine($"\t\t\tTotal price {frontTotalPrice * 2}\n");
            Console.WriteLine("\tSide panels:"); 
            Console.WriteLine($"\t\t2 panels with dimensions {height} mm x {width} mm x {sidesThickness} mm");
            Console.WriteLine($"\t\tTotal area {sidePanelsArea * 2} m\u00b2");
            Console.WriteLine($"\t\tprice per m\u00b2 {sidesPriceMeterSquared} EUR"); 
            Console.WriteLine($"\t\t\tTotal price {sideTotalPrice * 2}\n");
            Console.WriteLine("\tBottom panel:"); 
            Console.WriteLine($"\t\t1 panel with dimensions {height} mm x {width} mm x {bottomThickness} mm");
            Console.WriteLine($"\t\tTotal area {bottomPanelArea} m\u00b2");
            Console.WriteLine($"\t\tprice per m\u00b2 {bottomPriceMeterSquared} EUR"); 
            Console.WriteLine($"\t\t\tTotal price {bottomTotalPrice}\n\n");            
            Console.WriteLine($"TOTAL PRICE FOR ALL PANELS: {totalPanelPrice} EUR.");
        }       

        static double GetDimension(string dimensionType)        
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

        static double CalculateAreaMeter(double a, double b)
        {
            double retVal = a * b / 1000000;
            return retVal;
        }

        static double CalculateVolumeLitres(double a, double b, double c)
        {
            double retVal = a * b * c / 1000000;
            return retVal;
        }

        static double CalculateSideGlassThickness(double verticalX, double horizontalY, double bendingStress)
        {
            double ratio = horizontalY / verticalX;            
            
            double beta = 0;            

            if (ratio < 0.7)
                beta = 0.085;
            if (ratio < 1.0)
                beta = 0.116;
            if (ratio < 1.5)
                beta = 0.160;
            if (ratio < 2.0)
                beta = 0.260;
            if (ratio < 2.5)
                beta = 0.320;
            if (ratio < 3.0)
                beta = 0.350;
            beta = 0.370;           

            double thickness = Math.Sqrt(beta * Math.Pow(verticalX, 3) * (Math.Pow(10, -5) / bendingStress));

            return thickness;
        }

        static double CalculateBottomGlassThickness(double horizontalX, double horizontalY, double verticalZ, double bendingStress)
        {
            double ratio = 0;
            double beta = 0;

            if (horizontalX > horizontalY)
            {
                ratio = horizontalX / horizontalY;            
            }
            else
            {
                ratio = horizontalY / horizontalX;
            }                        

            if (ratio < 1.5)
                beta = 0.4530;
            if (ratio < 2.0)
                beta = 0.5172;
            if (ratio < 2.5)
                beta = 0.5688;
            if (ratio < 3.0)
                beta = 0.6102;
            beta = 0.7134;

            double thickness = Math.Sqrt(beta * Math.Pow(verticalZ, 3) * (Math.Pow(10, -5) / bendingStress));            

            return thickness;
        }

        static double FinalThickness(double thickenss)
        {
            if(thickenss <= 4)
                return 4;
            if(thickenss <= 6)
                return 6;
            if(thickenss <= 8)
                return 8;
            if(thickenss <= 10)
                return 10;
            if(thickenss <= 12)
                return 12;
            if(thickenss <= 15)
                return 15;
            if(thickenss <= 20)
                return 20;
            if(thickenss <= 30)
                return 30;
            if(thickenss <= 40)
                return 40;
            if(thickenss <= 50)
                return 50;
            return -1;            
        }

        static double PanelPriceMeterSquared (double thickness)
        {           
            if (thickness == 4)
                return 14;
            if (thickness == 6)
                return 18;
            if (thickness == 8)
                return 25;
            if (thickness == 10)
                return 31;
            if (thickness == 12)
                return 45;
            if (thickness == 15)
                return 100;
            if (thickness == 20)
                return 100;
            if (thickness == 30)
                return 55;
            if (thickness == 40)
                return 215;
            if (thickness == 50)
                return 300;
            return -1;
        }

        static double totalPrice(double front, double side, double bottom)
        {
            return front * 2 + side * 2 + bottom;
        }
    }
}

