using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fntt.Models
{
    internal class Teacher
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }   
        public List<Lesson> lessons { get; set; }




    }
}
