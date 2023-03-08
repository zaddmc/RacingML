using System;
using System.Drawing;
using System.IO;

namespace RacingML
{
    class Program
    {
        static private int[] layers = { 784, 16, 16, 10 };
        static private float[][] neurons;
        static private float[][] biases;
        static private float[][][] weights;

        static public float fitness = 0;

        static void Main(string[] args)
        {
            Console.WriteLine("Begin");

            StreamReader sr = new StreamReader("E:\\Stuff\\Programming\\Test\\RacingML\\mnist_train.csv");
            sr.ReadLine();



            bool isActive = true;
            while (isActive)
            {
                string[] command = Console.ReadLine().ToLower().Split(' ');
                switch (command[0])
                {
                    // if the input is not something that have been implementet yet it will just give a failure message and give another try
                    default:
                        Console.WriteLine("'{0}' is not recognized as an command", command[0]);
                        break;

                    //if the user wishes to exit the program they can safely use this command
                    case "exit":
                        isActive = false;
                        break;

                    // the user can clear the command prompt
                    case "clear":
                        Console.Clear();
                        break;

                    // the user can draw as many frames as wanted, note that if there isnt specified an amount it will only draw 1 frame
                    case "drawframe":
                        int num = 1;
                        if (command.Length > 1 && int.TryParse(command[1], out num))
                            for (int i = 0; i < num; i++)
                                DrawFrame(MakeFrame(sr).frame);
                        else
                            DrawFrame(MakeFrame(sr).frame);
                        Console.WriteLine("-Succesfully drawn {0} Frame", num);
                        break;

                    case "save":
                        if (command.Length > 1)
                            switch (command[1])
                            {
                                default:
                                    Console.WriteLine("Missing or invalid argument");
                                    break;
                                case "neurons":
                                    Console.WriteLine("Saving neurons to 'Neurons.csv'");
                                    SaveNeurons();
                                    break;
                            }
                        else
                        {
                            Console.WriteLine("missing argument");
                            Console.WriteLine("add neurons or similar argument");
                        }
                        break;
                } // main switch
            } // while opperational














            Console.WriteLine("End");
        } // Main
        static void InitNeurons(bool randomize)
        {
            StreamReader neuronsCSV = new StreamReader("E:\\Stuff\\Programming\\Test\\RacingML\\Neurons.csv");
            Random random = new Random();

            // allocates all needed memory space 
            neurons = new float[layers.Length][];
            for (int i = 0; i < layers.Length; i++)
                neurons[i] = new float[layers[i]];

            /* i am not sure if this is neccesarry, it will randomly create values, but it seems they dont need valeus
            if (neuronsCSV.ReadLine() != null && !randomize) // if true will load known values
                for (int i = 0; i < layers.Length; i++)
                    for (int j = 0; j < layers[i]; j++)
                        neurons[i][j] = float.Parse(neuronsCSV.ReadLine());
            else // makes random values
                for (int i = 0; i < layers.Length; i++)
                    for (int j = 0; j < layers[i]; j++)
                        neurons[i][j] = float.Parse(random.NextDouble());
            */

            // will load all known values
            for (int i = 0; i < layers.Length; i++)
                for (int j = 0; j < layers[i]; j++)
                    neurons[i][j] = float.Parse(neuronsCSV.ReadLine());


            neuronsCSV.Close();
        } // InitNeurons
        static void SaveNeurons()
        {
            StreamWriter neuronsCSV = new StreamWriter("E:\\Stuff\\Programming\\Test\\RacingML\\Neurons.csv");

            // writes all known values
            for (int i = 0; i < layers.Length; i++)
                for (int j = 0; j < layers[i]; j++)
                    neuronsCSV.WriteLine(neurons[i][j]);


            neuronsCSV.Close();
        } // SaveNeurons
        static (int label, byte[][] frame) MakeFrame(StreamReader sr)
        {
            // the first thing is to initialzize the memory, i do this in a byte as its value is from 0 to 255 and that is perfect for my grayscaled input
            // if i did not use byte the memory would be hit quite hard and i dont need more information than a byte anyways
            byte[][] pixels = new byte[28][];
            for (int i = 0; i < 28; i++)
                pixels[i] = new byte[28];

            // i read the next line in my training set and save it in a string array
            string[] strings = sr.ReadLine().Split(',');

            // i go through every value and position it in the byte array with an array inside
            for (int i = 0; i < 28; i++)
                for (int j = 0; j < 28; j++)
                    pixels[j][i] = Convert.ToByte(strings[i * 28 + j + 1]);

            // i return the the first value in the sequnce as it contains the label, the rest is given as the frame
            return (Convert.ToInt32(strings[0]), pixels);
        } // MakeFrame
        static void DrawFrame(byte[][] frame)
        {
            // if the program doesnt recognize the following commands then use this code in powershell: dotnet add package System.Drawing.Common --version 7.0.0
            Random rand = new(0);
            int scale = 25;
            SolidBrush brush = new SolidBrush(Color.White);

            // i make the map or canvas
            Bitmap bmp = new(28 * scale, 28 * scale);
            Graphics gfx = Graphics.FromImage(bmp);

            // i go through every pixel
            for (int i = 0; i < 28; i++)
                for (int j = 0; j < 28; j++)
                {
                    byte grayScale = frame[i][j];
                    brush.Color = Color.FromArgb(grayScale, grayScale, grayScale);
                    Rectangle rectangle = new(i * scale, j * scale, scale, scale);

                    gfx.FillRectangle(brush, rectangle);
                }

            // i save it to: E:\Stuff\Programming\Test\RacingML\bin\Debug\net7.0
            bmp.Save("test.jpg");
        } // DrawFrame
    }
}