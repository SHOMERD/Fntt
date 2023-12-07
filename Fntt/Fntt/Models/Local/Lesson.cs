using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fntt.Models.Local
{
    internal class Lesson
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Cabinet { get; set; }
        public Teacher Teacher { get; set; }
        public TimeSpan TimeSpan { get; set; }

        public int DayOfTheWeek { get; set; }

    }
}
