using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace ShamirSecret
{
    public class Number : IEquatable<Number>
    {
        // Represents the number n/z
        //   z
        //   -
        //   n
        public BigInteger Z { get; set; }
        public BigInteger N { get; set; }

        public Number() : this(0) { }
        public Number(BigInteger z) : this(z, 1) { }
        public Number(BigInteger z, BigInteger n)
        {
            Z = z;
            N = n;
            if (n == 0)
            {
                throw new DivideByZeroException("n was 0");
            }
            Reduce();
        }

        public void Reduce()
        {
            BigInteger gcd = BigInteger.GreatestCommonDivisor(N, Z);
            if (!gcd.IsOne)
            {
                Z = Z / gcd;
                N = N / gcd;
            }
        }

        public Number Inverse()
        {
            return new Number(N, Z);
        }

        public override string ToString()
        {
            if (N.IsOne)
            {
                return Z.ToString();
            }
            else if (N == -1)
            {
                return '-' + Z.ToString();
            }
            else
            {
                return String.Format("({0}/{1})", Z, N);
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is Number)
            {
                return Equals(obj as Number);
            }
            else
            {
                return base.Equals(obj);
            }
        }

        public bool Equals([AllowNull]Number obj)
        {
            if(obj == null)
            {
                return false;
            }
            this.Reduce();
            obj.Reduce();
            return this.Z == obj.Z && this.N == obj.N;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Z, N);
        }

        /*Operators*/
        private static BigInteger Lcm(BigInteger a, BigInteger b)
        {
            return (a * b) / BigInteger.GreatestCommonDivisor(a, b);
        }

        public static Number operator +(Number left, Number right)
        {
            if (left.N != right.N)
            {
                BigInteger lcm = Lcm(left.N, right.N);
                return new Number((left.Z * (lcm / left.N)) + right.Z * (lcm / right.N), lcm);
            }
            else
            {
                return new Number(left.Z + right.Z, left.N);
            }
        }
        public static Number operator -(Number left, Number right)
        {
            return left + new Number(0 - right.Z, right.N);
        }
        public static Number operator *(Number left, Number right)
        {
            return new Number(left.Z * right.Z, left.N * right.N);
        }
        public static Number operator /(Number left, Number right)
        {
            return left * right.Inverse();
        }


        public static implicit operator Number(int n)
        {
            return new Number(n);
        }
        public static implicit operator Number(BigInteger n)
        {
            return new Number(n);
        }
        public static explicit operator BigInteger(Number zn)
        {
            zn.Reduce();
            if (zn.N.IsOne)
            {
                return zn.Z;
            }
            else if (zn.N == -1)
            {
                return 0 - zn.Z;
            }
            else
            {
                return zn.Z / zn.N;
            }
        }
    }
}
