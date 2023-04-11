using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RacingML
{
    internal class LearningDistrict
    {
    }
    public class Propagation
    {
        static float TanhDerivative(float x)
        {
            return 1 - MathF.Pow(MathF.Tanh(x), 2);
        }
        public void PropagateBart(int label, ref float[][] neurons, ref float[][] desiredNeurons, ref float[][][] weights, ref float[][] biasesSmudge, ref float[][][] weightsSmudge)
        {

            for (int i = 0; i < neurons[neurons.Length - 1].Length; i++)
            {
                if (i == label)
                    desiredNeurons[neurons.Length - 1][i] = 1f;
                else
                    desiredNeurons[neurons.Length - 1][i] = 0f;
            }


            for (int i = neurons.Length - 1; i >= 1; i--)
            {
                for (int j = 0; j < neurons[i].Length; j++)
                {
                    var biasSmudge = TanhDerivative(neurons[i][j]) * (desiredNeurons[i][j] - neurons[i][j]);
                    biasesSmudge[i][j] += biasSmudge;
                    for (int k = 0; k < neurons[i - 1].Length; k++)
                    {
                        var weightSmudge = neurons[i - 1][k] * biasSmudge;
                        weightsSmudge[i - 1][j][k] += weightSmudge;

                        var neuronSmudge = weights[i - 1][j][k] * biasSmudge;
                        desiredNeurons[i - 1][k] += neuronSmudge;
                    }
                }
            }

        } // PropagateBart
    }


}