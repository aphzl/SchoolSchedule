using SchoolSchedule.Model.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolSchedule.Model.Dto
{
    public class DaySchedule
    {
        public Dictionary<int, List<Exercise>> ExerciseByLessonNumber { get; set; }
    }
}
