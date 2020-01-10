using ShamirSecret;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Xunit;

namespace CryptoTests
{
    public class UnitTest1
    {
        [Fact]
        public void MathMakesSense()
        {
            Assert.Equal(1, Encryptor.CalculatePolynomialAt(0, 1, new BigInteger[] { 0, 0 }));
            Assert.Equal(-1, Encryptor.CalculatePolynomialAt(0, -1, new BigInteger[] { 0, 0 }));
            Assert.Equal(1, Encryptor.CalculatePolynomialAt(0, 1, new BigInteger[] { 1, 1 }));
            Assert.Equal(1, Encryptor.CalculatePolynomialAt(0, 1, new BigInteger[] { 42, -42 }));
            Assert.Equal(1, Encryptor.CalculatePolynomialAt(0, 1, new BigInteger[] { 1, 2, 3, 4, 5, 6, 77, 8, 23, 23, 42, 5, 345, 34, 5, 345, 35, 34, 5, 345, 34, 5, 345, 4, 35 }));
            Assert.Equal(15, Encryptor.CalculatePolynomialAt(1, 0, new BigInteger[] { 1, 2, 3, 4, 5 }));
        }
        [Fact]
        public void CanRecover()
        {
            byte[][] vals = new byte[][] { BitConverter.GetBytes(42), UnicodeEncoding.UTF8.GetBytes("hello world!"), UnicodeEncoding.UTF8.GetBytes("hello world! long and lots of data let's hope it can handle this or maybe even something longer lorem ipsum et dolor sit amet") };
            foreach (byte[] bVal in vals)
            {
                BigInteger intVal = new BigInteger(bVal);
                ICollection<Point> points = Encryptor.Encrypt(intVal, 2);
                Assert.Equal(intVal, Encryptor.LagrangeInterpolate(points));
            }
        }

        [Fact]
        public void CheckLagrange()
        {
            Point[] points1 = new Point[] { new Point(1, 1), new Point(2, 4), new Point(3, 9) };
            Point[] points2 = new Point[] { new Point(1, 1), new Point(2, 8), new Point(3, 27), new Point(4, 64) };
            Assert.Equal(0, Encryptor.LagrangeInterpolate(points1));
            Assert.Equal(0, Encryptor.LagrangeInterpolate(points2));
        }

        [Fact]
        public void CanMath()
        {
            Assert.Equal((Number)4, ((Number)2) + (Number)2);
            Assert.Equal(0, -2 + (Number)2);
            Assert.Equal(0, 2 + (Number)(-2));
            Assert.Equal((Number)0, ((Number)(-9)) + (Number)9);
        }
    }
}
