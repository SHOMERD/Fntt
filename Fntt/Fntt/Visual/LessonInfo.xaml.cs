using Fntt.Models.Local;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fntt.Visual
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LessonInfo : ContentPage
    {
        Lesson Lesson { get; set; }
        public LessonInfo(Lesson lesson)
        {
            InitializeComponent();
            Lesson = lesson;
            LessonName.Text = lesson.Name;
            TecherName.Text = Lesson.Teacher;
            GroupName.Text = lesson.GroupName;
            LessonСlassroom.Text = lesson.Сlassroom;
            LessonTimeString.Text = lesson.StartTimeString + " - " + lesson.EndTimeString; 

        }

        private async void LessonСlassroom_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Launcher.OpenAsync(new Uri(Lesson.Сlassroom));
            }
            catch (Exception) { Console.WriteLine("Not URL"); } 
            
        }
    }
}