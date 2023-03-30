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

        static private float[][] desiredNeurons;
        static private float[][] biasesSmudge;
        static private float[][][] weightsSmudge;


        static private float learningRate = 1f;
        static private float weightDecay = 0.001f;

        static void Main(string[] args)
        {
            Console.WriteLine("Begin");

            StreamReader sr = new StreamReader(@"mnist_train.csv");
            sr.ReadLine();


            //debugging script 
            InitBiases(false);
            InitNeurons(false);
            InitWeights(false);
            InitSmudge();
            ActivateBart(sr, 1000, false);
            //PropagateBart(1000);
            //end debugging script


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
                                DrawFrame(MakeFrame(sr.ReadLine()).frame);
                        else
                            DrawFrame(MakeFrame(sr.ReadLine()).frame);
                        Console.WriteLine("-Succesfully drawn {0} Frame", num);
                        break;

                    // if the user wishes to feedforawrd for however many times they wish
                    case "forward" or "feedforward":
                        int cycles = 1;
                        if (command.Length > 1)
                            cycles = int.Parse(command[1]);
                        Console.WriteLine("-Succesfully calculated {0} Frame, in {1} miliseconds", cycles, ActivateBart(sr, cycles, false));
                        break;


                    case "run":
                        int cyclesRun = 1;
                        if (command.Length > 1)
                        {
                            if (command.Length > 2)
                                cyclesRun = int.Parse(command[2]);
                            Console.WriteLine("-Succesfully calculated {0} Frame, in {1} miliseconds", cyclesRun, ActivateBart(sr, cyclesRun, bool.Parse(command[1])));
                        }
                        else
                            Console.WriteLine("-Missing or invalid argument");

                        break;

                    /*
                // if the user wishes to propegate the neural network they can do as many times as they specify
                case "propagate":
                    int cyclesProp = 1;
                    if (command.Length > 1)
                        cyclesProp = int.Parse(command[1]);
                    Console.WriteLine("-Succesfully propegated {0} Frame, in {1} miliseconds", cyclesProp, ActivateBart(sr, cyclesProp));
                    break;
                    */

                    // different values can be saved if the user wishes to
                    case "save":
                        {
                            if (command.Length > 1)
                                switch (command[1])
                                {
                                    default:
                                        Console.WriteLine("-Missing or invalid argument");
                                        break;
                                    case "neurons":
                                        Console.WriteLine("-Saving neurons to 'Neurons.csv'");
                                        SaveNeurons();
                                        break;
                                    case "biases":
                                        Console.WriteLine("-Saving biases to 'Biases.csv'");
                                        SaveBiases();
                                        break;
                                    case "weights":
                                        Console.WriteLine("-Saving weights to 'Weights.csv'");
                                        SaveWeights();
                                        break;
                                    case "all":
                                        Console.WriteLine("-Saving neurons, biases and weights to to their respective csv file");
                                        SaveNeurons();
                                        SaveBiases();
                                        SaveWeights();
                                        break;

                                }
                            else
                            {
                                Console.WriteLine("missing argument");
                                Console.WriteLine("add neurons or similar argument");
                            }
                            break;
                        }
                    // different values can be loaded if the user wishes to
                    case "load":
                        {
                            if (command.Length > 1)
                                switch (command[1])
                                {
                                    default:
                                        Console.WriteLine("-Missing or invalid argument");
                                        break;
                                    case "neurons":
                                        if (command.Length > 2)
                                        {
                                            Console.WriteLine("-Allocating memory space for Neurons");
                                            InitNeurons(true);
                                        }
                                        else
                                        {
                                            Console.WriteLine("-Loading neurons from 'Neurons.csv' or Allocating memory space for Neurons");
                                            InitNeurons(false);
                                        }
                                        break;
                                    case "biases":
                                        if (command.Length > 2)
                                        {
                                            Console.WriteLine("-Allocating memory space with random values for Biases");
                                            InitBiases(true);
                                        }
                                        else
                                        {
                                            Console.WriteLine("-Loading biases from 'Biases.csv' or Allocating memory space with random values for Biases");
                                            InitBiases(false);
                                        }
                                        break;
                                    case "weights":
                                        if (command.Length > 2)
                                        {
                                            Console.WriteLine("-Allocating memory space for with random values Weights");
                                            InitWeights(true);
                                        }
                                        else
                                        {
                                            Console.WriteLine("-Loading weights from 'Weights.csv' or Allocating memory space for with random values Weights");
                                            InitWeights(false);
                                        }
                                        break;
                                    case "all":
                                        if (command.Length > 2)
                                        {
                                            Console.WriteLine("-Allocating memory space with random values for Biases and Weights");
                                            InitNeurons(true);
                                            InitBiases(true);
                                            InitWeights(true);
                                        }
                                        else
                                        {
                                            Console.WriteLine("-Loading weights from 'Weights.csv' or Allocating memory space with random values for Biases and Weights");
                                            InitNeurons(false);
                                            InitBiases(false);
                                            InitWeights(false);
                                        }
                                        break;
                                }
                            else
                            {
                                Console.WriteLine("Missing argument");
                                Console.WriteLine("Add neurons or similar argument");
                            }
                            break;
                        }
                } // main switch
            } // while opperational













            Console.WriteLine("End");
        } // Main
        static int DoTheThingBart(string strRed)
        {
            // i start by loading the first layer of neurons
            string[] strings = strRed.Split(',');
            for (int i = 0; i < neurons[0].Length; i++)
                if (strings[i + 1] != "0")
                    neurons[0][i] = 1 / float.Parse(strings[i + 1]);
                else
                    neurons[0][i] = 0;

            // the bulk of calculations
            for (int i = 1; i < neurons.Length; i++)
                for (int j = 0; j < neurons[i].Length; j++)
                {
                    float value = biases[i][j];
                    for (int k = 0; k < neurons[i - 1].Length; k++)
                        value += weights[i - 1][j][k] * neurons[i - 1][k];
                    neurons[i][j] = (float)Math.Tanh(value);
                    desiredNeurons[i][j] = neurons[i][j];
                }

            return int.Parse(strings[0]);
        } // DoTheThingBart
        static double ActivateBart(StreamReader sr, int cycles, bool propagate)
        {
            StreamWriter activationsCSV = new StreamWriter(@"Activations.csv");

            // i make a time stamp to know how long it takes
            DateTime timeStamp = DateTime.UtcNow;

            // i call the forward feeding network as many times as wished and then i save the neuron activations aswell as the label os that i can later tell it how to behave properly
            for (int i = 0; i < cycles; i++)
            {
                int label = DoTheThingBart(sr.ReadLine());
                for (int j = 0; j < neurons[neurons.Length - 1].Length; j++)
                    activationsCSV.Write("{0}.", neurons[neurons.Length - 1][j]);
                activationsCSV.WriteLine(label);
                if (propagate)
                    PropagateBart(label);
            }
            double timeStampEnd = (double)(DateTime.UtcNow - timeStamp).TotalMilliseconds;

            activationsCSV.Close();
            return timeStampEnd;
        } // ActivateBart
        static void PropagateBart(int label)
        {
            //StreamReader activationsCSV = new StreamReader(@"Activations.csv");


            /*
            int i = weights.Length;
            while (i > 0)
            {
                // this is unneeded
                
                //reading the activated neurons and the associated label
                if (activationsCSV.EndOfStream == true)
                {
                    // if there arent enoguh values in the activated list then it will break and therefore there is this simple check that will return how long it took to get to this point and will just say it failed
                    Console.WriteLine("--Insufecient lines to read");
                    return (double)(DateTime.UtcNow - timeStamp).TotalMilliseconds;
                }
                else if (a == weights.Length)
                {
                    string[] quickConersion = activationsCSV.ReadLine().Split('.');
                    incomming = new float[quickConersion.Length - 1];
                    for (int j = 0; j < incomming.Length; j++)
                    {
                        incomming[j] = float.Parse(quickConersion[j]);
                    }
                    label = int.Parse(quickConersion[quickConersion.Length]);
                }
                else
                {
                incomming = new float[neurons[a].Length];
                    for (int j = 0; j < neurons[a].Length; j++)
                    {
                        incomming[j] = neurons[a][j];

                    }    
                }
                



                // reading incomming neurons
                float[] incomming = new float[neurons[i].Length];
                for (int j = 0; j < neurons[i].Length; j++)
                {
                    incomming[j] = neurons[i][j];

                }

                // calculatiung how far from the wanted value it is
                float[] wantedChange = new float[incomming.Length];
                if (i == weights.Length)
                    for (int j = 0; j < wantedChange.Length; j++)
                    {
                        if (label == j)
                            wantedChange[j] = MathF.Pow(incomming[j] - 1, 2);
                        else
                            wantedChange[j] = MathF.Pow(incomming[j], 2) * -1;
                    }
                else
                    for (int j = 0; j < wantedChange.Length; j++)
                    {
                        wantedChange[j] = MathF.Pow(incomming[j]   - insert wanted value here , 2);
                    }

                

                i--;
            }
        */ //scrap


            


            for (int i = neurons.Length - 1; i >= 1; i--)
            {
                for (int j = 0; j < neurons[i].Length; j++)
                {
                    var biasSmudge = TanhDerivative(neurons[i][j]) * (desiredNeurons[i][j] - neurons[i][j]);
                    biasesSmudge[i][j] += biasSmudge;
                    for (int k = 0; k < neurons[i-1].Length; k++)
                    {
                        var weightSmudge = neurons[i - 1][k] * biasSmudge;
                        weightsSmudge[i - 1][j][k] += weightSmudge;

                        var neuronSmudge = weights[i - 1][j][k] * biasSmudge;
                        desiredNeurons[i - 1][k] += neuronSmudge;
                    }
                }
            }


            for (int i = neurons.Length-1; i >= 1; i--)
            {
                for (int j = 0; j < neurons[i].Length; j++)
                {
                    biases[i][j] += biasesSmudge[i][j] * learningRate;
                    biases[i][j] *= 1 - weightDecay;
                    biasesSmudge[i][j] = 0;

                    for (var k = 0; k < neurons[i - 1].Length; k++)
                    {
                        weights[i - 1][j][k] += weightsSmudge[i - 1][j][k] * learningRate;
                        weights[i - 1][j][k] *= 1 - weightDecay;
                        weightsSmudge[i - 1][j][k] = 0;
                    }

                    desiredNeurons[i][j] = 0;

                }
            }






            //activationsCSV.Close();
        } // PropagateBart
        static float TanhDerivative(float x)
        {
            return x * (1 - x);
        }
        static void InitSmudge()
        {
            // this is only needed in the backpropagation algorithim

            // allocates memory for smudge of neurons
            desiredNeurons = new float[layers.Length][];
            for (int i = 0; i < layers.Length; i++)
                desiredNeurons[i] = new float[layers[i]];

            // allocates memory for smudge of biases
            biasesSmudge = new float[layers.Length][];
            for (int i = 0; i < layers.Length; i++)
                biasesSmudge[i] = new float[layers[i]];

            // allocates memory for smudge of weights
            weightsSmudge = new float[layers.Length - 1][][];
            for (int i = 0; i < weightsSmudge.Length; i++)
            {
                weightsSmudge[i] = new float[layers[i + 1]][];
                for (int j = 0; j < layers[i + 1]; j++)
                    weightsSmudge[i][j] = new float[layers[i]];
            }

        } // InitSmudge
        static void InitNeurons(bool nullify)
        {
            StreamReader neuronsCSV = new StreamReader(@"Neurons.csv");
            string[] strings;

            // allocates all needed memory space 
            neurons = new float[layers.Length][];
            for (int i = 0; i < layers.Length; i++)
                neurons[i] = new float[layers[i]];

            if (neuronsCSV.Peek() != -1 && !nullify) // if true will load known values
                for (int i = 0; i < layers.Length; i++)
                {
                    strings = neuronsCSV.ReadLine().Split('.');
                    for (int j = 0; j < layers[i]; j++)
                        neurons[i][j] = float.Parse(strings[j]);
                }
            else // allocates the memory of them with 0 as the value
                for (int i = 0; i < layers.Length; i++)
                    for (int j = 0; j < layers[i]; j++)
                        neurons[i][j] = 0;

            neuronsCSV.Close();
        } // InitNeurons
        static void InitBiases(bool randomize)
        {
            StreamReader biasesCSV = new StreamReader(@"Biases.csv");
            Random random = new Random();
            string[] strings;

            // allocates all needed memory space 
            biases = new float[layers.Length][];
            for (int i = 0; i < layers.Length; i++)
                biases[i] = new float[layers[i]];

            if (biasesCSV.Peek() != -1 && !randomize) // if true will load known values
                for (int i = 0; i < layers.Length; i++)
                {
                    strings = biasesCSV.ReadLine().Split('.');
                    for (int j = 0; j < layers[i]; j++)
                        biases[i][j] = float.Parse(strings[j]);
                }
            else // allocates the memory of them with a random value between -5 and 5
                for (int i = 0; i < layers.Length; i++)
                    for (int j = 0; j < layers[i]; j++)
                        biases[i][j] = random.NextSingle() * 10 - 5;

            biasesCSV.Close();
        } // InitBiases
        static void InitWeights(bool randomize)
        {
            StreamReader weightsCSV = new StreamReader(@"Weights.csv");
            Random random = new Random();
            string[] strings;

            // allocates all needed memory space 
            weights = new float[layers.Length - 1][][];
            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] = new float[layers[i + 1]][];
                for (int j = 0; j < layers[i + 1]; j++)
                    weights[i][j] = new float[layers[i]];
            }

            if (weightsCSV.Peek() != -1 && !randomize) // if true will load known values
                for (int i = 0; i < weights.Length; i++)
                    for (int j = 0; j < weights[i].Length; j++)
                    {
                        strings = weightsCSV.ReadLine().Split('.');
                        for (int k = 0; k < weights[i][j].Length; k++)
                            weights[i][j][k] = float.Parse(strings[k]);
                    }
            else // allocates the memory of them with a random value between -5 and 5
                for (int i = 0; i < weights.Length; i++)
                    for (int j = 0; j < weights[i].Length; j++)
                        for (int k = 0; k < weights[i][j].Length; k++)
                            weights[i][j][k] = random.NextSingle() * 10 - 5;


            weightsCSV.Close();
        } // InitWeights
        static void SaveNeurons()
        {
            StreamWriter neuronsCSV = new StreamWriter(@"Neurons.csv");

            // writes all known values
            for (int i = 0; i < layers.Length; i++)
            {
                for (int j = 0; j < layers[i] - 1; j++)
                    neuronsCSV.Write("{0}.", neurons[i][j]);
                neuronsCSV.WriteLine(neurons[i][layers[i] - 1]);
            }

            neuronsCSV.Close();
        } // SaveNeurons
        static void SaveBiases()
        {
            StreamWriter biasesCSV = new StreamWriter(@"Biases.csv");

            // writes all known values
            for (int i = 0; i < layers.Length; i++)
            {
                for (int j = 0; j < layers[i] - 1; j++)
                    biasesCSV.Write("{0}.", biases[i][j]);
                biasesCSV.WriteLine(biases[i][layers[i] - 1]);
            }

            biasesCSV.Close();
        } // SaveBiases
        static void SaveWeights()
        {
            StreamWriter weightsCSV = new StreamWriter(@"Weights.csv");

            // writes all known values
            for (int i = 0; i < weights.Length; i++)
                for (int j = 0; j < weights[i].Length; j++)
                {
                    for (int k = 0; k < weights[i][j].Length - 1; k++)
                        weightsCSV.Write("{0}.", weights[i][j][k]);
                    weightsCSV.WriteLine(weights[i][j][weights[i][j].Length - 1]);
                }

            weightsCSV.Close();
        } // SaveWeights
        static (int label, byte[][] frame) MakeFrame(string incoming)
        {
            // the first thing is to initialzize the memory, i do this in a byte as its value is from 0 to 255 and that is perfect for my grayscaled input
            // if i did not use byte the memory would be hit quite hard and i dont need more information than a byte anyways
            byte[][] pixels = new byte[28][];
            for (int i = 0; i < 28; i++)
                pixels[i] = new byte[28];

            // i read the next line in my training set and save it in a string array
            string[] strings = incoming.Split(',');

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