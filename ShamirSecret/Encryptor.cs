using System;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography;

namespace ShamirSecret
{
    //TODO: mod with prime
    public static class Encryptor
    {
        private static int mersenPrimeExponent = 127;
        public static BigInteger Prime { get; private set; } = BigInteger.Pow(2, mersenPrimeExponent) - 1;
        public static ICollection<Point> Encrypt(BigInteger secret, int partners)
        {
            return Encrypt(secret, partners, partners);
        }
        public static ICollection<Point> Encrypt(BigInteger secret, int partners, int order, RandomNumberGenerator rng = null)
        {
            if (partners < 2)
            {
                throw new System.Exception("Sharing between less than two parties?");
            }
            if (rng == null)
            {
                rng = new RNGCryptoServiceProvider();
            }
            byte[] secByte = secret.ToByteArray();
            //generate #order coefficients
            BigInteger[] coefficients = new BigInteger[order];
            for (int i = 0; i < coefficients.Length; i++)
            {
                coefficients[i] = GetRandom(rng, secByte.Length);
            }

            //foreach partner generate random Point on the polynomial
            Point[] points = new Point[partners];
            for (int i = 0; i < partners; i++)
            {
                points[i] = CalculatePolynomialPointAt(i + 1, secret, coefficients);
                points[i].Y = points[i].Y;
            }
            return points;
        }

        public static BigInteger CalculatePolynomialAt(BigInteger x, BigInteger intercept, BigInteger[] coefficients)
        {
            BigInteger y = 0;
            for (int i = 0; i < coefficients.Length; i++)
            {
                y += BigInteger.Pow(x, i + 1) * coefficients[i];
            }
            return y + intercept;
        }
        public static Point CalculatePolynomialPointAt(BigInteger x, BigInteger intercept, BigInteger[] coefficients)
        {
            return new Point()
            {
                X = x,
                Y = CalculatePolynomialAt(x, intercept, coefficients)
            };
        }

        public static BigInteger GetRandom(RandomNumberGenerator rng, int size)
        {
            byte[] bytes = new byte[size];
            rng.GetBytes(bytes);
            return new BigInteger(bytes);
        }


        /// <summary>
        /// calc at x=0
        /// </summary>
        public static BigInteger LagrangeInterpolate(ICollection<Point> points)
        {
            Point[] pointsAr = new Point[points.Count];
            points.CopyTo(pointsAr, 0);
            return LagrangeInterpolate(pointsAr);

        }

        /// <summary>
        /// calc at x=0
        /// </summary>
        public static BigInteger LagrangeInterpolate(Point[] points)
        {
            //https://en.wikipedia.org/wiki/Lagrange_polynomial#Examples
            BigInteger y = 0;

            for (int i = 0; i < points.Length; i++)
            {
                Console.Write("+");
                y += CalculateLagrangeComponent(i, points);
            }
            return y;
        }
        private static BigInteger CalculateLagrangeComponent(int at, Point[] points)
        {
            BigInteger val = points[at].Y;
            Console.Write(val);
            for (int i = 0; i < points.Length; i++)
            {
                if (i == at)
                {
                    continue;
                }
                Console.Write("*(-{0} / ({1} - {2}))", points[i].X, points[at].X, points[i].X);
                val *= (0 - points[i].X) / (points[at].X - points[i].X);
            }

            return val;
        }
    }

    public class Point
    {
        public BigInteger X { get; set; }
        public BigInteger Y { get; set; }
    }
}
