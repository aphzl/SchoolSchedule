using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolSchedule.Model.Entity
{
    interface IExercised
    {
        List<Exercise> Exercises { get; }
    }
}
