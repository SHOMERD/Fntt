using Fntt.Models.Local;
using Fntt.Models.Web;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.Text;



namespace Fntt.Data
{
    public class SheetsOperator
    {
        public SheetsRequester sheetsRequester {  get; set; }
        public ResponseModel aktiveCourse { get; set; }
        public Group activeGroup { get; set; }
        public YouAre user { get; set; }
        public bool DataExsist { get; set; }

        public SheetsOperator()
        {
            sheetsRequester = new SheetsRequester();
            bool userExists = UploudeUser();
            
            




        }


        private bool UploudeUser()
        {
            object emptyUser = null;
            App.Current.Properties.TryGetValue("UserData", out emptyUser);
            YouAre youAre = (YouAre)emptyUser;
            if (youAre == null) { return false; }
            if (youAre.UsetType == 0)
            {
                if (string.IsNullOrEmpty(youAre.Course) || string.IsNullOrEmpty(youAre.Group)) { return false; }
            }
            if (youAre.UsetType == 1 && string.IsNullOrEmpty(youAre.Name)) { return false; }
            user = youAre;
            return true;
        }

        public int CheckUser()
        {
            object emptyUser = null;
            App.Current.Properties.TryGetValue("UserData", out emptyUser);
            YouAre youAre = (YouAre)emptyUser;
            if (youAre == null) { return -1; }
            if (youAre.UsetType == 0)
            {
                if (string.IsNullOrEmpty(youAre.Course) || string.IsNullOrEmpty(youAre.Group) ) { return -1; }
            }
            if (youAre.UsetType == 1 && string.IsNullOrEmpty(youAre.Name)) { return -1; } 
            return youAre.UsetType;

        }



        public bool SetAktiveСourse(string courseName)
        {
            for (int i = 0; i < sheetsRequester.allSheets.Count; i++)
            {
                if (courseName == sheetsRequester.allSheets[i].name)
                {
                    aktiveCourse = sheetsRequester.allSheets[i];
                    return true;
                }
            }
            return false;
        }

        public bool SetAktiveGrup(string courseName)
        {
            Group group = null;

            for (int i = 1; i < aktiveCourse.timetable[0].Count; i++)
            {
                if (aktiveCourse.timetable[0][i].ToString() == user.Group)
                {
                    group = new Group()
                    {
                        Name = user.Group,
                        Lessons = new List<Lesson>() { }
                    };

                    for (int s = 2; s < aktiveCourse.timetable.Count; s++)
                    {
                        group.Lessons.Add(new Lesson()
                        {
                            Name = aktiveCourse.timetable[s][i].ToString(),
                            Teacher = aktiveCourse.timetable[s][i + 1].ToString(),
                            Сlassroom = aktiveCourse.timetable[s][i + 2].ToString(),
                            StartTime = StingToTimeConvertor(aktiveCourse.timetable[s][1].ToString(), true),
                            EndTime = StingToTimeConvertor(aktiveCourse.timetable[s][1].ToString(), false),
                            DayOfTheWeek = (s - 2) / 6
                        });
                    }
                    activeGroup = group;
                    return true;
                }
            }
            activeGroup = null;
            return false;
        }



        public List<String> GetGrupsNames(string CourseName)
        {
            List<String> grupsNames = new List<String>();
            
            if (aktiveCourse == null) { return null; }

            for (int i = 1; i < aktiveCourse.timetable[0].Count; i++)
            {
                if (!string.IsNullOrEmpty(aktiveCourse.timetable[0][i].ToString()))
                {
                    grupsNames.Add(aktiveCourse.timetable[0][i].ToString());
                }
            }
            return grupsNames;
        }


        public List<String> GetСourseNames() 
        {
            try
            {
                List<String> result = new List<String>();
                for (int i = 0; i < sheetsRequester.allSheets.Count; i++)
                {
                    result.Add(sheetsRequester.allSheets[i].name);
                }
                return result;
            }
            catch { return null; }

        }

        public void SetUser(int usetType,string usetName, string courseName, string groupName )
        {
            user = new YouAre();
            user.UsetType = usetType;
            user.Name = usetName;
            user.Course = courseName;
            user.Group = groupName;

            App.Current.Properties.Add("UserData", user);
            object v = App.Current.Properties;
        }


        public YouAre GetUserData() 
        {
            object AreYou = null;
            App.Current.Properties.TryGetValue("UserData", out AreYou);
            YouAre youAre = (YouAre)AreYou;
            if (youAre != null && !string.IsNullOrEmpty(youAre.Course) && !string.IsNullOrEmpty(youAre.Group)) 
            {
                return youAre;
            }
            else
            {
                return null;
            }
        }


        private DateTime StingToTimeConvertor(string stringTime, bool IsFerst)
        {
            if (IsFerst)
            {
                return new DateTime(0, 0, 0, Convert.ToInt32(stringTime.ToString().Split('-')[0].Split('.')[0]), Convert.ToInt32(stringTime.ToString().Split('-')[0].Split('.')[1]), 0);
            }
            else
            {
                return new DateTime(0, 0, 0, Convert.ToInt32(stringTime.ToString().Split('-')[1].Split('.')[0]), Convert.ToInt32(stringTime.ToString().Split('-')[1].Split('.')[1]), 0);
            }

             
        }

        public List<Lesson> GetWeekLesons()
        {
            return activeGroup.Lessons;
        }

        public List<Lesson> GetDayLesons(int Day)
        {

            List<Lesson> lessons = new List<Lesson>();
            for (int i = 0; i < activeGroup.Lessons.Count; i++)
            {
                if (activeGroup.Lessons[i].DayOfTheWeek == Day)
                {
                    lessons.Add(activeGroup.Lessons[i]);
                }
            }

            return lessons;

        }

        public List<string> GetTeacherNames()
        {
            try
            {
                return new List<string>() { "Андрей" };
            }
            catch { return null; }

        }





    }
}
