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
            Random rand = new(0);
            int scale = 20;
            SolidBrush brush = new SolidBrush(Color.White);

            Bitmap bmp = new(28 * scale, 28 * scale);
            Graphics gfx = Graphics.FromImage(bmp);

            for (int i = 0; i < 28; i++)
                for (int j = 0; j < 28; j++)
                {
                    //Range range = new(Int32.MinValue, Int32.MaxValue);
                    brush.Color = Color.FromArgb(Int32.MaxValue * (1 / (frame[i][j] + 1)));
                    Rectangle rectangle = new(i * scale, j * scale, scale, scale);

                    gfx.FillRectangle(brush, rectangle);
                }

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