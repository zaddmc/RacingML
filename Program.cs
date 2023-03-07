using System;
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
            Console.WriteLine(MakeFrame(sr));
        }
        static (int label, byte[][] frame) MakeFrame(StreamReader sr)
        {

            byte[][] pixels = new byte[28][];
            for (int i = 0; i < 28; i++)
                pixels[i] = new byte[28];

            string[] strings = sr.ReadLine().Split(',');

            for (int i = 0; i < 28; i++)
                for (int j = 0; j < 28; j++)
                    pixels[i][j] = Convert.ToByte(strings[i * 28 + j + 1]);

            return (Convert.ToInt32(strings[0]),  pixels);
        }
    }
}