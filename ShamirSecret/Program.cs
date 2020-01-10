using System;

namespace ShamirSecret
{
    class Program
    {
        static void Main(string[] args)
        {
            var points1 = new Point[] { new Point(1, 1), new Point(2, 4), new Point(3, 9) };
            Console.WriteLine("\n"+Encryptor.LagrangeInterpolate(points1));




            Console.Read();
        }
    }
}
