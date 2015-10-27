using System;
using System.Collections.Generic;

namespace Phosphaze.Core.Dml
{

    /// <summary>
    /// A base exception for all Timeline related errors.
    /// </summary>
    public class TimelineException : Exception
    {

        public TimelineException(string msg) : base(msg) { }

        public TimelineException(string msg, Exception inner) : base(msg, inner) { }

    }

    /// <summary>
    /// A timeline that maintains an ordered list of events to perform.
    /// </summary>
    public class DmlTimeline
    {

        /// <summary>
        /// The sorted list of timestamps, ordered by start time.
        /// </summary>
        private List<DmlTimestamp> Timestamps = new List<DmlTimestamp>();

        /// <summary>
        /// The list of all currently active timestamps (for which Start <= LocalTime < End).
        /// </summary>
        private List<DmlTimestamp> currentlyActive = new List<DmlTimestamp>();

        /// <summary>
        /// Whether or not the timeline has begun running.
        /// </summary>
        private bool Begun = false;

        /// <summary>
        /// The local time of the system in milliseconds.
        /// </summary>
        public double LocalTime = 0;

        public DmlTimeline() { }

        public void Begin()
        {
            Begun = true;
            Timestamps.Sort((t1, t2) => (t1.Start.CompareTo(t2.Start)));
        }

        /// <summary>
        /// Add a timestamp
        /// </summary>
        /// <param name="timestamp"></param>
        public void AddTimestamp(DmlTimestamp timestamp) 
        {
            if (Begun)
                throw new TimelineException("Cannot add new timestamp to already begun timeline.");
            Timestamps.Add(timestamp);
        }

        public void Update(DmlSystem system)
        {
            // Append all the new timestamps to the currentlyActive list.
            while (Timestamps.Count > 0 && Timestamps[0].Start <= LocalTime)
            {
                currentlyActive.Add(Timestamps[0]);
                Timestamps.RemoveAt(0);
            }

            // Execute all the currently active Timestamps.
            foreach (DmlTimestamp timestamp in currentlyActive)
                if (timestamp.Active(LocalTime))
                    timestamp.Code.Execute(null, system);

            // Remove all the Timestamps whose times are up.
            currentlyActive.RemoveAll(t => t.End < LocalTime);

            LocalTime += Globals.deltaTime;
        }

    }
}
