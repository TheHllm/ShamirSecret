using System;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography;

namespace ShamirSecret
{
    //TODO: mod with prime
    public static class Encryptor
    {
        public static ICollection<Point> Encrypt(BigInteger secret, int partners)
        {
            return Encrypt(secret, partners, partners);
        }
        public static ICollection<Point> Encrypt(BigInteger secret, int partners, int numCoefficients, RandomNumberGenerator rng = null)
        {
            if (numCoefficients < 2)
            {
                throw new System.Exception("Sharing between less than two parties?");
            }
            if(!(partners <= numCoefficients))
            {
                throw new System.Exception("Can't share to more partners than curve has order (partners <= order has to hold)");
            }
            if (rng == null)
            {
                rng = new RNGCryptoServiceProvider();
            }
            byte[] secByte = secret.ToByteArray();
            //generate #order-1 coefficients since "secret" is the 0th coefficient
            BigInteger[] coefficients = new BigInteger[numCoefficients-1];
            for (int i = 0; i < coefficients.Length -1; i++)
            {
                coefficients[i] = GetRandom(rng, secByte.Length);
            }

            //foreach partner generate random Point on the polynomial
            Point[] points = new Point[partners];
            for (int i = 0; i < partners; i++)
            {
                points[i] = CalculatePolynomialPointAt(i + 1, secret, coefficients);
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
            Number y = 0;

            for (int i = 0; i < points.Length; i++)
            {
                var dy = CalculateLagrangeComponent(i, points);
                Console.Write("+"+dy);
                y = y + dy;
            }
            return (BigInteger)y;
        }
        private static Number CalculateLagrangeComponent(int at, Point[] points)
        {
            Number val = (Number)points[at].Y;
            //Console.Write(val);
            for (int i = 0; i < points.Length; i++)
            {
                if (i == at)
                {
                    continue;
                }
                //Console.Write("*(-{0} / ({1} - {2}))", points[i].X, points[at].X, points[i].X);
                val = val * ((0 - (Number)points[i].X) / (points[at].X - (Number)points[i].X));
            }

            return val;
        }
    }

    public class Point
    {
        public BigInteger X { get; set; }
        public BigInteger Y { get; set; }

        public Point() : this(0) { }
        public Point(BigInteger xy) : this(xy, xy) { }
        public Point(BigInteger x, BigInteger y)
        {
            X = x;
            Y = y;
        }
    }
}
