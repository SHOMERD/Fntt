using Fntt.Logics;
using Fntt.Models.Local;
using Fntt.Models.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FnttErrorChecker
{
    public class Program
    {
        static void Main(string[] args)
        {
            Program program = new Program();
            program.Go();
            while (true) { }
        }

        public async void Go()
        {
            List<ResponseModel> responseModels = await GetData();

            if (responseModels == null)
            {
                Console.WriteLine("Данные не получены");
            }
            else
            {
                while (true)
                {
                    Console.WriteLine("\n\n\n\t\tВывести:" +
                        "\n 1 - Все пары" +
                        "\n 2 - Все учителя" +
                        "\n 3 - Места ошибок" +
                        "");
                    switch (Console.ReadLine())
                    {
                        case "1":
                            Console.WriteLine("Ожидайте");
                            ShouAllLessons(responseModels);                     
                            break;
                        case "2":
                            Console.WriteLine("Ожидайте");
                            List<string> strings = await GetTeacherNames(responseModels);
                            for (int i = 0; i < strings.Count; i++)
                            {
                                Console.WriteLine(strings[i]);
                            }
                            break;
                        case "3":
                            Console.WriteLine("Ожидайте");
                            ShouAllErrorLessons(responseModels);
                            break;
                        default :
                            Console.WriteLine("Пункт отсутствует в меню");
                            break;


                    }


                }

            }
        }


        public async void ShouAllLessons(List<ResponseModel> responseModels)
        {
            List<Lesson> Lesons = (await GetAllLessons(responseModels)).Lessons;

            for (int i = 0; i < Lesons.Count; i++)
            {
                Console.WriteLine($"________________________________" +
                    $"\nНазвание пары\t {Lesons[i].Name}" +
                    $"\nДата пары\t {Lesons[i].Date}" +
                    $"\nГруппы \t {Lesons[i].GroupName}" +
                    $"\nВремя начала\t {Lesons[i].StartTime}" +
                    $"\nВремя оканчания\t {Lesons[i].EndTime}" +
                    $"\nМесто провождения пары\t {Lesons[i].Сlassroom}" +
                    $"\nУчитель\t {Lesons[i].Teacher}" +
                    $"\n");
            }




        }

        public async void ShouAllErrorLessons(List<ResponseModel> responseModels)
        {
            DisplayedData teacherTimetable = new DisplayedData()
            {
                Lessons = new List<Lesson>() { }
            };

            for (int d = 0; d < responseModels.Count; d++)
            {
                responseModels[d].timetable = responseModels[d].timetable.Where(x => x[0].ToString() != "Дни").ToList();

                for (int i = 1; i < responseModels[d].timetable[0].Count; i++)
                {
                    if (!string.IsNullOrEmpty(responseModels[d].timetable[0][i].ToString()))
                    {
                        for (int s = 2; s < responseModels[d].timetable.Count; s++)
                        {
                            try
                            {
                                teacherTimetable.Lessons.Add(new Lesson()
                                {
                                    Name = responseModels[d].timetable[s][i].ToString(),
                                    GroupName = responseModels[d].timetable[0][i].ToString(),
                                    Teacher = responseModels[d].timetable[s][i + 1].ToString(),
                                    Сlassroom = responseModels[d].timetable[s][i + 2].ToString(),
                                    StartTime = SrtingToTimeConvertor(responseModels[d].timetable[s][2].ToString(), true),
                                    EndTime = SrtingToTimeConvertor(responseModels[d].timetable[s][2].ToString(), false),
                                    Date = ((DateTime)responseModels[d].timetable[s - ((s - 1) % 6)][1]).AddDays(1)
                                });
                            }
                            catch (Exception)
                            {
                                Console.WriteLine($"_________________" +
                                    $"\n Данные не обработаны на листе номер {d+1}" +
                                    $"\n\tДанные не обработаны по позиции {s + 1} и {i+1}");
                            }

                        }
                    }

                }
            }




        }





        public async Task<List<string>> GetTeacherNames(List<ResponseModel> responseModels)
        {
            List<ResponseModel> allSheets = responseModels;
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


        public async Task<DisplayedData> LessonShaker(DisplayedData data)
        {
            List<Lesson> sorted = data.Lessons.OrderBy(x => x.StartTime).ToList();

            List<Lesson> lessons = new List<Lesson>();
            lessons.Add(sorted[0]);
            for (int i = 1; i < sorted.Count; i++)
            {
                bool Flag = true;
                for (int s = 0; s < lessons.Count; s++)
                {
                    if (lessons[s].Name == sorted[i].Name && lessons[s].StartTime == sorted[i].StartTime && lessons[s].Сlassroom == sorted[i].Сlassroom && lessons[s].Date == sorted[i].Date)
                    {
                        lessons[s].GroupName = lessons[s].GroupName + " и " + sorted[i].GroupName;
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


        public async Task<DisplayedData> GetAllLessons(List<ResponseModel> responseModels)
        {
            DisplayedData teacherTimetable = new DisplayedData()
            {
                Lessons = new List<Lesson>() { }
            };

            for (int d = 0; d < responseModels.Count; d++)
            {
                responseModels[d].timetable = responseModels[d].timetable.Where(x => x[0].ToString() != "Дни").ToList();

                for (int i = 1; i < responseModels[d].timetable[0].Count; i++)
                {
                    if (!string.IsNullOrEmpty(responseModels[d].timetable[0][i].ToString()))
                    {
                        for (int s = 2; s < responseModels[d].timetable.Count; s++)
                        {                            
                            try
                            {
                                teacherTimetable.Lessons.Add(new Lesson()
                                {
                                    Name = responseModels[d].timetable[s][i].ToString(),
                                    GroupName = responseModels[d].timetable[0][i].ToString(),
                                    Teacher = responseModels[d].timetable[s][i + 1].ToString(),
                                    Сlassroom = responseModels[d].timetable[s][i + 2].ToString(),
                                    StartTime = SrtingToTimeConvertor(responseModels[d].timetable[s][2].ToString(), true),
                                    EndTime = SrtingToTimeConvertor(responseModels[d].timetable[s][2].ToString(), false),
                                    Date = ((DateTime)responseModels[d].timetable[s - ((s - 1) % 6)][1]).AddDays(1)
                                });
                            }
                            catch (Exception)
                            {
                            }
                            
                        }
                    }

                }
            }
            return await LessonShaker(teacherTimetable);

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

        public async Task<List<ResponseModel>> GetData()
        {
            return await SheetsRequester.SheetsRequeste("0");
        }


    }
}
