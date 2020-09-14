
using System;

namespace KMeansProject
{
    public class EuclideanDistance : IDistance
    {
        public double Run(double[] array1, double[] array2)             /* Diziler arası mesafe hesaplaması için
                                                                   { \sqrt {(p_{x}-q_{x})^{2}}}=|p_{x}-q_{x}|}  olan öklid formülünü kullandık. */
        {
            double res = 0;
            for (int i = 0; i < array1.Length; i++)
            {
                res += Math.Pow(array1[i] - array2[i], 2);               
            }
            return Math.Sqrt(res);
        }
    }
}
