﻿using System;

namespace MicroRabbit.Domain.Core.Event
{
    public abstract class Event
    {
        public DateTime Timestamp { get;protected set; }

        protected Event()
        {
            Timestamp = DateTime.Now;
        }
    }
}