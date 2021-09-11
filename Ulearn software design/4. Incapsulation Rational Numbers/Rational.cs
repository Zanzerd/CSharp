using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incapsulation.RationalNumbers
{
    public struct Rational
    {
        public int Numerator { get; set; }
        public int Denominator { get; set; }
        public bool IsNan { get; }

        public Rational(int numerator, int denominator)
        {
            IsNan = false;
            if (denominator == 0)
            {
                Numerator = 0;
                Denominator = 0;
                IsNan = true;
                return;
            }
            if (numerator == 0)
            {
                Numerator = 0;
                Denominator = 1;
                return;
            }
            if (denominator < 0)
            {
                denominator = -denominator;
                numerator = -numerator;
            }    
            var reduced = Reduce(numerator, denominator);
            Numerator = reduced.Item1;
            Denominator = reduced.Item2;
        }

        public Rational(int numerator) : this(numerator, 1)
        {

        }

        private static (int, int) Reduce(int numerator, int denominator)
        {
            int biggest = 0;
            int smallest = 0;
            var gcd = 1;
            if (numerator == denominator)
                return (1, 1);
            else if (numerator > denominator)
            {
                biggest = numerator;
                smallest = denominator;
            }
            else
            {
                biggest = denominator;
                smallest = numerator;
            }
            if ((smallest < 0 || biggest < 0) && Math.Abs(smallest) > Math.Abs(biggest))
                findGCD(Math.Abs(biggest), Math.Abs(smallest));
            else gcd = findGCD(smallest, biggest);
            return (numerator / gcd, denominator / gcd);
        }

        private static int findGCD(int prev, int prePrev)
        {
            int curr = 1; // взаимно простые
            prev = Math.Abs(prev);
            prePrev = Math.Abs(prePrev);
            do // алгоритм Евклида
            {
                int q0 = 1;
                while (q0 * prev < prePrev)
                {
                    q0++;
                }
                if (q0 * prev > prePrev)
                    q0--;
                curr = (prePrev - q0 * prev);
                if (curr == 0)
                {
                    return prev;
                }
                prePrev = prev;
                prev = curr;
            }
            while (true);
        }

        private static int findLCM(int a, int b)
        {
            int gcd;
            if (a == b)
                return a;
            else if (a > b)
                gcd = findGCD(b, a);
            else
                gcd = findGCD(a, b);
            return Math.Abs(a * b) / gcd;

        }

        public static Rational operator+ (Rational a, Rational b)
        {
            if (Check(a, b))
                return new Rational(0, 0);
            var lcm = findLCM(a.Denominator, b.Denominator);
            return new Rational(a.Numerator * (lcm / a.Denominator) + b.Numerator * (lcm / b.Denominator), lcm);
        }

        public static Rational operator- (Rational a, Rational b)
        {
            if (Check(a, b))
                return new Rational(0, 0);
            var lcm = findLCM(a.Denominator, b.Denominator);
            return new Rational(a.Numerator * (lcm / a.Denominator) - b.Numerator * (lcm / b.Denominator), lcm);
        }

        public static Rational operator* (Rational a, Rational b)
        {
            if (Check(a, b))
                return new Rational(0, 0);
            return new Rational(a.Numerator * b.Numerator, a.Denominator * b.Denominator);
        }

        public static Rational operator/ (Rational a, Rational b)
        {
            if (Check(a, b))
                return new Rational(0, 0);
            return new Rational(a.Numerator * b.Denominator, a.Denominator * b.Numerator);
        }

        public static implicit operator double(Rational a)
        {
            return (double) a.Numerator / a.Denominator;
        }

        public static implicit operator Rational(int num)
        {
            return new Rational(num);
        }

        public static explicit operator int(Rational a)
        {
            if ((double)a.Numerator % a.Denominator == 0)
                return a.Numerator / a.Denominator;
            throw new InvalidCastException();
        }

        private static bool Check(Rational a, Rational b)
        {
            return a.IsNan || b.IsNan;
        }
    }
}
