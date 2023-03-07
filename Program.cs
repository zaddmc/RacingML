using System;
using System.Drawing;
using System.IO;

namespace RacingML
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Begin");

            StreamReader sr = new StreamReader("E:\\Stuff\\Programming\\Test\\RacingML\\mnist_train.csv");
            sr.ReadLine();


            DrawFrame(MakeFrame(sr).frame);












            Console.WriteLine("End");
        }
        static void DrawFrame(byte[][] frame)
        {
            // if the program doesnt recognize the following commands then use this code in powershell : dotnet add package System.Drawing.Common --version 7.0.0
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
        }
        static (int label, byte[][] frame) MakeFrame(StreamReader sr)
        {

            byte[][] pixels = new byte[28][];
            for (int i = 0; i < 28; i++)
                pixels[i] = new byte[28];

            string[] strings = sr.ReadLine().Split(',');

            for (int i = 0; i < 28; i++)
                for (int j = 0; j < 28; j++)
                    pixels[j][i] = Convert.ToByte(strings[i * 28 + j + 1]);

            return (Convert.ToInt32(strings[0]), pixels);
        }
    }
}