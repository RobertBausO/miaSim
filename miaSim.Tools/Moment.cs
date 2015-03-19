using System;

namespace miaGame.Tools
{
    public class Moment
    {
        public Moment()
        {
            Ticks = 0;
        }

        private Moment(long ticks)
        {
            Ticks = ticks;
        }

        public static Moment CurrentMoment { get { return new Moment(DateTime.Now.Ticks); } }

        public long Ticks { get; set; }

        public Moment Subtract(Moment m)
        {
            var diff = Ticks - m.Ticks;
            return new Moment(diff);
        }

        public double TotalSeconds { get { return TimeSpan.FromTicks(Ticks).TotalSeconds; } }
        public double TotalMilliseconds { get { return 1000.0 * TotalSeconds; } }

    }
}
