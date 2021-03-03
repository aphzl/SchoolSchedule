using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolScheduleApp.Exceptions
{
    class CommandException : Exception
    {
        public CommandException(string message) : base(message)
        {
        }
    }
}
