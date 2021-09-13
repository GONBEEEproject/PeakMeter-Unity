using System;

namespace ouchi
{
    public readonly struct Ratio : IComparable<Ratio>
    {
        public readonly int num;
        public readonly int den;

        public Ratio(int numerator, int denominator = 1)
        {
            if (denominator == 0)
            {
                throw new ArgumentException("Denominator cannot be zero.", nameof(denominator));
            }
            num = numerator;
            den = denominator;
        }

        public static Ratio operator +(Ratio a) => a;
        public static Ratio operator -(Ratio a) => new Ratio(-a.num, a.den);

        public static Ratio operator *(Ratio a, Ratio b)
            => new Ratio(a.num * b.num, a.den * b.den);

        public static Ratio operator *(Ratio a, int b)
            => new Ratio(a.num * b, a.den);
        public static Ratio operator *(int a, Ratio b)
            => b * a;

        public static Ratio operator /(Ratio a, Ratio b)
        {
            if (b.num == 0)
            {
                throw new DivideByZeroException();
            }
            return new Ratio(a.num * b.den, a.den * b.num);
        }
        public static Ratio operator /(Ratio a, int b)
        {
            return a / new Ratio(b);
        }
        public static Ratio operator /(int a, Ratio b)
        {
            return new Ratio(a) / b;
        }

        public override string ToString() => $"{num} / {den}";
        public double ToDouble() => (double)num / (double)den;

        public int CompareTo(Ratio other)
        {
            long a = num * other.den;
            long b = other.num * den;
            return b.CompareTo(b) - a.CompareTo(b);
        }
    }
}
