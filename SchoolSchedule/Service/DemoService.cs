using SchoolSchedule.Model.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolSchedule.Service
{
    class DemoService
    {
        private readonly ScheduleContext dbContext;

        public DemoService(ScheduleContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void CleanAndGenerateDemoBase()
        {
            dbContext.CleanDb();
        }
    }
}
