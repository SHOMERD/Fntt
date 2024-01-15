using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fntt.Models.Local
{
    internal class Group
    {
        public string Name { get; set; }

        public List<Lesson> Lessons { get; set; }


    }
}
