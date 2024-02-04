using Fntt.Models.Local;
using Fntt.Models.Web;
using Fntt.Visual;
using Google.Apis.Sheets.v4.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;



namespace Fntt.Data
{
    public class SheetsOperator
    {
        public SheetsRequester sheetsRequester {  get; set; }
        public ResponseModel aktiveCourse { get; set; }
        public DisplayedData activeTimetable { get; set; }
        public YouAre user { get; set; }
        public bool DataExsist { get; set; }

        public SheetsOperator()
        {
            sheetsRequester = new SheetsRequester();
            bool userExists = UploudeUser();
        }
        


        public async Task SetData()
        {
            if (user.UsetType == 0)
            {
                if ((await GetСourseNames()).Contains(user.Course))
                {
                    SetAktiveСourse(user.Course);
                    if ( (await GetGrupsNames(user.Course)).Contains(user.Group))
                    {
                        SetAktiveGrup(user.Group);
                    }
                    else
                    {
                        Preferences.Clear("UserData");
                        App.Current.MainPage = new UserForm(this);
                    }
                }
                else
                {
                    Preferences.Clear("UserData");
                    App.Current.MainPage = new UserForm(this);
                }
            }

            if (user.UsetType == 1)
            {
                if ((await GetTeacherNames()).Contains(user.Name))
                {
                    SetTeacherTimetable(user.Name);     
                }

            }
        }



        public void SetGrupData()
        {
            SetAktiveСourse(user.Course);
            SetAktiveGrup(user.Group);
        }


        private bool UploudeUser()
        {
            YouAre emptyUser;
            if (!Preferences.ContainsKey("UserData"))
            {
                return false;
            }
            emptyUser = JsonConvert.DeserializeObject<YouAre>(Preferences.Get("UserData", ""));
            YouAre youAre = emptyUser;
            if (youAre == null) { return false; }
            this.user = youAre;
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
            YouAre emptyUser ;
            
            emptyUser = JsonConvert.DeserializeObject<YouAre>(Preferences.Get("UserData", ""));

            if (emptyUser == null) { return -1; }
            if (emptyUser.UsetType == 0)
            {
                if (string.IsNullOrEmpty(emptyUser.Course) || string.IsNullOrEmpty(emptyUser.Group) ) { return -1; }
            }
            if (emptyUser.UsetType == 1 && string.IsNullOrEmpty(emptyUser.Name)) { return -1; } 
            return emptyUser.UsetType;

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
            DisplayedData group = null;

            for (int i = 1; i < aktiveCourse.timetable[0].Count; i++)
            {
                if (aktiveCourse.timetable[0][i].ToString() == user.Group)
                {
                    group = new DisplayedData()
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
                            StartTime = SrtingToTimeConvertor(aktiveCourse.timetable[s][1].ToString(), true),
                            EndTime = SrtingToTimeConvertor(aktiveCourse.timetable[s][1].ToString(), false),
                            DayOfTheWeek = (s - 2) / 6
                        });
                    }
                    activeTimetable = group;
                    return true;
                }
            }
            activeTimetable = null;
            return false;
        }



        public async Task<List<String>> GetGrupsNames(string CourseName)
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


        public async Task<List<String>> GetСourseNames() 
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
            this.user = user;
            Preferences.Set("UserData", JsonConvert.SerializeObject(user));
        }


        public YouAre GetUserData() 
        {
            YouAre emptyUser;

            emptyUser = JsonConvert.DeserializeObject<YouAre>(Preferences.Get("UserData",null));

            YouAre youAre = (YouAre)emptyUser;
            if (youAre != null && !string.IsNullOrEmpty(youAre.Course) && !string.IsNullOrEmpty(youAre.Group)) 
            {
                return youAre;
            }
            else
            {
                return null;
            }
        }


        public DateTime SrtingToTimeConvertor(string stringTime, bool IsFerst)
        {
            int h;
            int m;
            DateTime rData = new DateTime();
            if (IsFerst)
            {
                h = Convert.ToInt32(stringTime.ToString().Split('-')[0].Split('.')[0]);
                m = Convert.ToInt32(stringTime.ToString().Split('-')[0].Split('.')[1]);
            }
            else
            {
                h = Convert.ToInt32(stringTime.ToString().Split('-')[1].Split('.')[0]);
                m = Convert.ToInt32(stringTime.ToString().Split('-')[1].Split('.')[1]);
            }
            rData = rData.AddHours(h);
            rData = rData.AddMinutes(m);
            return rData;

        }


        public List<Lesson> GetWeekLesons()
        {
            List<Lesson> lesons = new List<Lesson>();
            List<Lesson> Aktive = activeTimetable.Lessons.OrderBy(x => x.DayOfTheWeek).ToList();

            string[] DayArrey = {"_______Понедельник_______", "_______Вторник_______", "_______Среда_______", "_______Четверг_______", "_______Пятница_______", "_______Суббота_______", "_______Воскресенье_______" };

            lesons.Add(new Lesson(){ 
                StartTime = DateTime.MinValue,
                Name = DayArrey[0].ToUpper(),
                DayOfTheWeek = 0,
            });
            lesons.Add(Aktive[0]);
            for (int i = 1; i < Aktive.Count; i++)
            {
                if (Aktive[i].DayOfTheWeek > Aktive[i-1].DayOfTheWeek)
                {
                    lesons.Add(new Lesson()
                    {
                        StartTime = DateTime.MinValue,
                        Name = DayArrey[Aktive[i].DayOfTheWeek].ToUpper(),
                        DayOfTheWeek = Aktive[i].DayOfTheWeek,
                    });
                }
                lesons.Add(Aktive[i]);
            }
            return lesons;
        }



        public List<Lesson> GetDayLesons(int Day)
        {

            List<Lesson> lessons = new List<Lesson>();
            for (int i = 0; i < activeTimetable.Lessons.Count; i++)
            {
                if (activeTimetable.Lessons[i].DayOfTheWeek == Day)
                {
                    lessons.Add(activeTimetable.Lessons[i]);
                }
            }

            return lessons;

        }

        public async Task<List<string>> GetTeacherNames()
        {
            List<ResponseModel> allSheets = sheetsRequester.allSheets;
            List<string> teachersName = new List<string>();


            for (int d = 0; d < allSheets.Count; d++)
            {
                for (int i = 1; i < allSheets[d].timetable[0].Count; i++)
                {
                    if (!string.IsNullOrEmpty(allSheets[d].timetable[0][i].ToString()))
                    {
                        for (int s = 2; s < allSheets[d].timetable.Count; s++)
                        {

                            if (!string.IsNullOrEmpty(allSheets[d].timetable[s][i + 1].ToString()) && !teachersName.Contains(allSheets[d].timetable[s][i + 1].ToString()))
                            {
                                teachersName.Add(allSheets[d].timetable[s][i + 1].ToString());
                            }

                        }
                    }

                }
            }
            teachersName.Sort();
            return teachersName;
        }

        public void SetTeacherTimetable(string teacherName)
        {
            List<ResponseModel> allSheets = sheetsRequester.allSheets;
            DisplayedData teacherTimetable = new DisplayedData()
            {
                Lessons = new List<Lesson>() { }
            };
            teacherTimetable.Name = teacherName;

            for (int d = 0; d < allSheets.Count; d++)
            {
                for (int i = 1; i < allSheets[d].timetable[0].Count; i++)
                {
                    if (!string.IsNullOrEmpty(allSheets[d].timetable[0][i].ToString()))
                    {
                        for (int s = 2; s < allSheets[d].timetable.Count; s++)
                        {
                            if (allSheets[d].timetable[s][i + 1].ToString().Contains(teacherName))
                            {
                                teacherTimetable.Lessons.Add(new Lesson()
                                {
                                    Name = allSheets[d].timetable[s][i].ToString(),
                                    GroupName = allSheets[d].timetable[0][i].ToString(),
                                    Teacher = allSheets[d].timetable[s][i + 1].ToString(),
                                    Сlassroom = allSheets[d].timetable[s][i + 2].ToString(),
                                    StartTime = SrtingToTimeConvertor(allSheets[d].timetable[s][1].ToString(), true),
                                    EndTime = SrtingToTimeConvertor(allSheets[d].timetable[s][1].ToString(), false),
                                    DayOfTheWeek = (s - 2) / 6
                                });
                            }

                        }
                    }

                }
            }



            activeTimetable = LessonShaker(teacherTimetable);

        }

        public DisplayedData LessonShaker(DisplayedData data)
        { 
            List<Lesson> sorted = data.Lessons.OrderBy(x => x.StartTime).ToList();

            List<Lesson> lessons = new List<Lesson>();
            lessons.Add(sorted[0]);
            for (int i = 1; i < sorted.Count; i++)
            {
                bool Flag = true;
                for (int s = 0; s < lessons.Count; s++)
                {
                    if (lessons[s].Name == sorted[i].Name && lessons[s].StartTime == sorted[i].StartTime && lessons[s].Сlassroom == sorted[i].Сlassroom && lessons[s].DayOfTheWeek == sorted[i].DayOfTheWeek)
                    {
                        lessons[s].GroupName = lessons[s].GroupName + " и " +sorted[i].GroupName;
                        Flag = false;
                    }
                }
                if (Flag)
                {
                    lessons.Add(sorted[i]);
                }
                
            }
            data.Lessons = lessons;


            return data;
        }





    }
}
