using System;

namespace ShamirSecret
{
    class Program
    {
        static void Main(string[] args)
        {
            var val = 42;
            var points = Encryptor.Encrypt(val, 2);
             Encryptor.LagrangeInterpolate(points);




            Console.Read();
        }
    }
}
