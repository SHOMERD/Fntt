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

        public string StartTimeString  { get {
                string d = StartTime.Hour.ToString() + ":";
                if (StartTime.Minute < 10)
                {
                    d += "0";
                }
                d+= StartTime.Minute.ToString();
                return d; 
            } }
        public string EndTimeString {
            get
            {
                string d = EndTime.Hour.ToString() + ":";
                if (EndTime.Minute < 10)
                {
                    d += "0";
                }
                d += EndTime.Minute.ToString();
                return d;
            } }
    }
}
