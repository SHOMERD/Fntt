using System;
using System.Collections.Generic;
using System.Text;

namespace Fntt.Models.Local
{
    public class Lesson
    {
        public string Name { get; set; }
        public string TransformedName {  get; set; }
        public string Сlassroom { get; set; }
        public string TransformedСlassroom { get; set; }
        public string GroupName {  get; set; }


        public string Teacher { get; set; }
        public DateTime Date { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public string StartTimeString  { get { return (StartTime.Hour.ToString() + ":" + StartTime.Minute.ToString()); } }
        public string EndTimeString { get { return (EndTime.Hour.ToString() + ":" + EndTime.Minute.ToString()); } }
    }
}
