using System;

namespace MicroRabbit.Domain.Core.Event
{
    public abstract class Event
    {
        public DateTime Timespan { get;protected set; }

        protected Event()
        {
            Timespan = DateTime.Now;
        }
    }
}