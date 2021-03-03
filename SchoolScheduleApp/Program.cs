using System;

namespace SchoolScheduleApp
{
    public class Program
    {
        public static void Main()
        {
            var console = ScheduleConsole.Create();
            console.Start();
        }
    }
}
