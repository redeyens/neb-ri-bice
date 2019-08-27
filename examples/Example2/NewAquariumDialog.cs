using System;

namespace Example2
{
    class NewAquariumDialog
    {
        public AquariumDimensions Dimensions { get; set; }
        
        public void Show()
        {
            double width = GetInput("Enter width [mm]: ");
            double length = GetInput("Enter length [mm]: ");
            double waterLevel = GetInput("Enter water level (height) [mm]: ");

            Dimensions = new AquariumDimensions(width, length, waterLevel);
        }

        private double GetInput(string prompt)
        {
            double userInput;
            do
            {
                Console.Write(prompt);
            } while (!double.TryParse(Console.ReadLine(), out userInput) || !InputIsValid(userInput));

            return userInput;
        }

        private bool InputIsValid(double userInput)
        {
            return userInput > 0;
        }

    }
}
