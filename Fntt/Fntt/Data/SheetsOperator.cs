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
        SheetsRequester sheetsRequester;
        ResponseModel aktiveCourse;
        Group activeGroup;
        YouAre user;

        public SheetsOperator()
        {
            sheetsRequester = new SheetsRequester();
            bool userExists = UploudeUser();
            
            //определить коректность данных aktiveCourse, user, activeGroup




        }


        private bool UploudeUser()
        {
            object emptyUser = null;
            App.Current.Properties.TryGetValue("UserData", out emptyUser);
            YouAre youAre = (YouAre)emptyUser;
            if (youAre != null || !string.IsNullOrEmpty(youAre.Course) || !string.IsNullOrEmpty(youAre.Group)) { return false; }
            user = youAre;
            return true;
        }

        public static int CheckUser()
        {
            object emptyUser = null;
            App.Current.Properties.TryGetValue("UserData", out emptyUser);
            YouAre youAre = (YouAre)emptyUser;
            if (youAre != null) { return -1; }
            if (youAre.UsetType == 0)
            {
                if (string.IsNullOrEmpty(youAre.Course) || string.IsNullOrEmpty(youAre.Group) ) { return -1; }
            }
            if (youAre.UsetType == 1 && string.IsNullOrEmpty(youAre.Name)) { return -1; } 
            return youAre.UsetType;

        }



        public List<String> GetGrupsNames()
        {
            List<String> grupsNames = new List<String>();
            for (int i = 1; i < aktiveCourse.timetable[0].Count; i++)
            {
                if (!string.IsNullOrEmpty(aktiveCourse.timetable[0][i].ToString()))
                {
                    grupsNames.Add(aktiveCourse.timetable[0][i].ToString());
                } 
            }
            return grupsNames;
        }


        public List <String> GetСourseNames() 
        {
            List<String> result = new List<String>();
            for (int i = 0; i < sheetsRequester.allSheets.Count; i++)
            {
                result.Add(sheetsRequester.allSheets[i].name);
            }
            return result;
        }

        public void SetUser(string courseName, string groupName)
        {
            user = new YouAre();
            user.Course = courseName;
            user.Group = groupName;
            App.Current.Properties["UserData"] = user;
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

        private bool FindCorrectGroup()
        {
            YouAre u = GetUserData();
            Group group = null;

            if (u == null) { return false; }

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
                            StartTime = stingToTimeConvertor(aktiveCourse.timetable[s][1].ToString(), true),
                            EndTime = stingToTimeConvertor(aktiveCourse.timetable[s][1].ToString(), false),
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

        private DateTime stingToTimeConvertor(string stringTime, bool IsFerst)
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

        private bool FindCorrectCourse()
        {

            for (int i = 0; i < sheetsRequester.allSheets.Count; i++)
            {
                if (sheetsRequester.allSheets[i].name == user.Course)
                {
                    aktiveCourse = sheetsRequester.allSheets[i];
                    return true;
                }
            }
            return false;
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





    }
}
