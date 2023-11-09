using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fntt.Models
{
    internal class Group
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }
        
        List<Lesson> Lessons { get; set; }


    }
}
