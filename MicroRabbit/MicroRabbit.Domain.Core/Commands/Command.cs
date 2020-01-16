using System;
using MicroRabbit.Domain.Core.Event;

namespace MicroRabbit.Domain.Core.Commands
{
    public abstract class Command : Message
    {
        public DateTime Timespan { get; protected set; }

        protected Command()
        {
            Timespan = DateTime.Now;
        }
    }
}