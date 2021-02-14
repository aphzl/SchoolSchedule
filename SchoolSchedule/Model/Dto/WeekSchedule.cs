using System;
using System.Collections.Generic;

namespace SchoolSchedule.Model.Dto
{
    public class WeekSchedule
    {
        public Dictionary<DayOfWeek, DaySchedule> ScheduleByDay { get; set; }
    }
}
