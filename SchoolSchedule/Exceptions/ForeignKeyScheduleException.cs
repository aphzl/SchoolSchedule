using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolSchedule.Exceptions
{
    class ForeignKeyScheduleException : Exception
    {
        public ForeignKeyScheduleException()
        {
        }

        public ForeignKeyScheduleException(string message)
            : base(message)
        {
        }

        public ForeignKeyScheduleException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
