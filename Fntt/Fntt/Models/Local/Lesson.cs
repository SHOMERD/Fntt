using System;
using System.Collections.Generic;
using System.Text;

namespace Fntt.Models.Local
{
    public class Lesson
    {
        public string Name { get; set; }
        public string Сlassroom { get; set; }
        public string Teacher { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public string StartTimeString  { get { return StartTime.ToShortTimeString(); } }
        public string EndTimeString { get { return EndTime.ToShortTimeString(); } }

        public int DayOfTheWeek { get; set; }
    }
}
