using Phosphaze.Core.Dml.Parse;
using System;

namespace Phosphaze.Core.Dml
{
    public abstract class DmlTimestamp
    {

        /// <summary>
        /// The start of the timestamp. This is used by DmlTimeline to
        /// order timestamps in a stack.
        /// </summary>
        public double Start { get; protected set; }

        /// <summary>
        /// The end of the timestamp. This is used by DmlTimeline to
        /// determine when to stop worrying about running a timestamp.
        /// 
        /// This value should be set to the absolute end of a timestamp.
        /// For example, if a timestamp is periodically active and then
        /// inactive, End should NOT refer to the end of each period, but
        /// rather to the end of the LAST period.
        /// </summary>
        public double End { get; protected set; }

        public CodeBlock Code { get; private set; }

        public DmlTimestamp()
        {
            Code = null;
        }

        public void SetCode(CodeBlock code)
        {
            Code = code;
        }

        /// <summary>
        /// Check if the timestamp is active or not.
        /// </summary>
        public abstract bool Active(double time);

    }

    /// <summary>
    /// A DmlTimestamp that is active only once at a single point in time.
    /// </summary>
    public class TimestampAt : DmlTimestamp
    {

        public TimestampAt(double time)
            : base() 
        {
            Start = time;
            End = time + 50;
        }

        public override bool Active(double time) 
        {
            return Start <= time && time < Start + Globals.deltaTime;
        }

    }

    /// <summary>
    /// A DmlTimestamp that is only active before a given time.
    /// </summary>
    public class TimestampBefore : DmlTimestamp
    {

        public TimestampBefore(double time)
            : base() 
        {
            Start = 0;
            End = time;
        }

        public override bool Active(double time)
        {
            return time < End;
        }

    }

    /// <summary>
    /// A DmlTimestamp that is only active after a given time.
    /// </summary>
    public class TimestampAfter : DmlTimestamp
    {

        public TimestampAfter(double time)
            : base() 
        {
            Start = time;
            End = Double.PositiveInfinity;
        }

        public override bool Active(double time)
        {
            return Start <= time;
        }

    }

    /// <summary>
    /// A DmlTimestamp that is only active during a given time interval.
    /// </summary>
    public class TimestampFrom : DmlTimestamp
    {

        public TimestampFrom(double start, double end)
            : base() 
        {
            Start = start;
            End = end;
        }

        public override bool Active(double time)
        {
            return Start <= time && time < End;
        }
    }

    /// <summary>
    /// A DmlTimestamp that is only active outside a given time interval.
    /// </summary>
    public class TimestampOutside : DmlTimestamp
    {

        public TimestampOutside(double start, double end)
            : base()
        {
            Start = start;
            End = end;
        }

        public override bool Active(double time)
        {
            return Start >= time || time > End;
        }
    }

    /// <summary>
    /// A DmlTimestamp that is only active at evenly spaced points in time.
    /// </summary>
    public class TimestampAtIntervals : DmlTimestamp
    {

        private double interval;

        public TimestampAtIntervals(double interval)
            : this(interval, 0, Double.PositiveInfinity) { }

        public TimestampAtIntervals(double interval, double start, double end)
            : base() 
        {
            Start = start;
            End = end;
            this.interval = interval;
        }

        public override bool Active(double time)
        {
            double t2 = time - Start;
            return t2 >= 0 && time < End && (t2 % interval) <= Globals.deltaTime;
        }

    }

    /// <summary>
    /// A DmlTimestamp that is only active at evenly spaced durations of time.
    /// </summary>
    public class TimestampDuringIntervals : DmlTimestamp
    {

        private double interval, itdbl;

        public TimestampDuringIntervals(double interval)
            : this(interval, 0, Double.PositiveInfinity) { }

        public TimestampDuringIntervals(double interval, double start, double end)
            : base() 
        {
            Start = start;
            End = end;
            this.interval = interval;
            itdbl = 2*interval;
        }

        public override bool Active(double time)
        {
            double t2 = time - Start;
            return t2 >= 0 && time < End && (t2 % itdbl) < interval;
        }

    }

}
