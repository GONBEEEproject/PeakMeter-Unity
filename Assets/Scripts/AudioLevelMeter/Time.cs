using System;

namespace ouchi
{
    public struct Time 
    {
        public Time(int value, Ratio unit)
        {
            this.value = value;
            this.unit = unit;
        }

        public Time CastTime(Ratio newunit)
            => new Time((int)(value / unit * newunit).ToDouble(), newunit);
        public static Time operator+(Time a, Time b)
        {
            Ratio min = a.unit.CompareTo(b.unit) < 0 ? a.unit : b.unit;
            return new Time(a.CastTime(min).value + b.CastTime(min).value, min);
        }
        public static Time operator-(Time a, Time b)
        {
            Ratio min = a.unit.CompareTo(b.unit) < 0 ? a.unit : b.unit;
            return new Time(a.CastTime(min).value - b.CastTime(min).value, min);
        }

        public Ratio unit;
        public int value;

        public static Time Seconds(int s)
            => new Time(s, units.seconds);
        public static Time Milliseconds(int ms)
            => new Time(ms, units.milliseconds);
        public static Time Minutes(int m)
            => new Time(m, units.minutes);
        public static Time Hours(int h)
            => new Time(h, units.hours);
        public static Time Days(int d)
            => new Time(d, units.days);

        public readonly struct units
        {
            public static readonly Ratio milliseconds = new Ratio(1, 1000);
            public static readonly Ratio seconds = new Ratio(1);
            public static readonly Ratio minutes = new Ratio(60);
            public static readonly Ratio hours = new Ratio(60 * 60);
            public static readonly Ratio days = new Ratio(60 * 60 * 24);
        }
    }

}

