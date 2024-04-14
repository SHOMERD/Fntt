using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using Fntt.Logics;
using Fntt.Models;
using Fntt.Models.Local;
using Fntt.Visual.BufferPages;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;

namespace Fntt.Visual
{
    public partial class TodayTametable : ContentPage
    {
        public DateTime DayOfTheWeek;
        public bool CanShouAll;
        SheetsOperator sheetsOperator;


        public TodayTametable(SheetsOperator sheetsOperator, DateTime setedDey, bool ShouAll = false)
        {
            this.sheetsOperator = sheetsOperator;
            this.CanShouAll = ShouAll;
            DayOfTheWeek = setedDey; 
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

        }



        protected override async void OnAppearing()
        {
            base.OnAppearing();

            SetTitle();

            if (CanShouAll)
            {
                listViweData.ItemsSource = TrasformeLesons( sheetsOperator.GetWeekLesons());
            }
            else
            {
                listViweData.ItemsSource = TrasformeLesons(sheetsOperator.GetDayLesons(DayOfTheWeek));
            }

        }

        public void SetTitle()
        {
            //switch ((int)DayOfTheWeek.DayOfWeek)
            //{
            //    case 1:
            //        ToolbarString.Text = "Понедельник";
            //        break;
            //    case 2:
            //        ToolbarString.Text = "Вторник";
            //        break;
            //    case 3:
            //        ToolbarString.Text = "Среда";
            //        break;
            //    case 4:
            //        ToolbarString.Text = "Четверг";
            //        break;
            //    case 5:
            //        ToolbarString.Text = "Пятница";
            //        break;
            //    case 6:
            //        ToolbarString.Text = "Суббота";
            //        break;
            //    case 0:
            //        ToolbarString.Text = "Воскресенье";
            //        break;
            //}
            ToolbarString.Text = DayOfTheWeek.ToShortDateString();
            if (DayOfTheWeek.Date == DateTime.Now.Date)
            {
                ToolbarString.Text = "Сегодня";
            }
            if (CanShouAll)
            {
                ToolbarString.Text = "Всё расписание";
            }


        }

        async void ShouAll(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TodayTametable( sheetsOperator ,DateTime.MinValue, true));
        }

        async void OnListItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                try
                {
                    await Navigation.PushAsync(new LessonInfo((Lesson)e.SelectedItem));
                }
                catch (Exception)
                {
                }
                
            }
        }

        void LastDay(object sender, EventArgs e)
        {
            ChangeDay(-1);
        }

        void NextDay(object sender, EventArgs e)
        {
            ChangeDay(+1);
        }

        public void ChangeDay(int S)
        {
            DayOfTheWeek = DayOfTheWeek.AddDays(S);
            OnAppearing();
        }

        private List<Lesson> TrasformeLesons(List<Lesson> lessons)
        {
            for (int i = 0; i < lessons.Count; i++)
            {
                if (lessons[i].Name.Length > 34)
                {
                    lessons[i].TransformedName = lessons[i].Name.Substring(0, 31) + "...";
                }
                else
                {
                    lessons[i].TransformedName = lessons[i].Name;
                }
                try
                {
                    new Uri(lessons[i].Сlassroom);
                    lessons[i].TransformedСlassroom = "-->";
                }
                catch (Exception) { lessons[i].TransformedСlassroom = lessons[i].Сlassroom; }
            }
            return lessons;
        }

        private void ResetData(object sender, EventArgs e)
        {
            Preferences.Clear();
            App.Current.MainPage = new LoadPage();
            sheetsOperator.sheetsRequester.RestartSheetReqester();
            ((LoadPage)App.Current.MainPage).StartLoad(sheetsOperator);
        }
    }
}