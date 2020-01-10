using ShamirSecret;
using System;
using System.Numerics;
using Xunit;

namespace CryptoTests
{
    public class UnitTest1
    {
        [Fact]
        public void MathMakesSense()
        {
            Assert.Equal(1,Encryptor.CalculatePolynomialAt(0, 1, new BigInteger[] { 0, 0}));
            Assert.Equal(-1, Encryptor.CalculatePolynomialAt(0, -1, new BigInteger[] { 0, 0 }));
            Assert.Equal(1, Encryptor.CalculatePolynomialAt(0, 1, new BigInteger[] { 1, 1 }));
            Assert.Equal(1, Encryptor.CalculatePolynomialAt(0, 1, new BigInteger[] { 42, -42 }));
            Assert.Equal(1, Encryptor.CalculatePolynomialAt(0, 1, new BigInteger[] { 1,2,3,4,5,6,77,8,23,23,42,5,345,34,5,345,35,34,5,345,34,5,345,4,35 }));
            Assert.Equal(10, Encryptor.CalculatePolynomialAt(1, 0, new BigInteger[] { 1,2,3,4 }));
        }
        [Fact]
        public void CanRecover()
        {
            var val = 42;
            var points = Encryptor.Encrypt(val, 2);
            Assert.Equal(val,Encryptor.LagrangeInterpolate(points));
        }
    }
}
