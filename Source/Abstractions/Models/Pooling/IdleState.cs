using System;

namespace ReusableLibrary.Abstractions.Models
{
    public sealed class IdleState
    {
        public IdleState()
            : this(DateTime.UtcNow)
        {
        }

        public IdleState(DateTime current)
        {
            CreatedOn = current;
            UsedOn = current;
        }

        public DateTime CreatedOn { get; private set; }

        public DateTime UsedOn { get; set; }

        public void Reset()
        {
            CreatedOn = UsedOn = DateTime.UtcNow;
        }
    }
}
