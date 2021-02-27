using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolSchedule.Model.Entity
{
    public interface IKeyable
    {
        object[] Key { get; }
    }
}
