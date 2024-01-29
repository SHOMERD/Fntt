using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using Fntt.Data;
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
        public int DayOfTheWeek;
        public bool CanShouAll;
        SheetsOperator sheetsOperator;


        public TodayTametable(SheetsOperator sheetsOperator, int setedDey, bool ShouAll = false)
        {
            this.sheetsOperator = sheetsOperator;
            this.CanShouAll = ShouAll;
            DayOfTheWeek = setedDey; 
            sheetsOperator.SetData();
            InitializeComponent();

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
                listViweData.ItemsSource = TrasformeLesons(sheetsOperator.GetDayLesons(DayOfTheWeek - 1));
            }

        }

        public void SetTitle()
        {
            switch (DayOfTheWeek)
            {
                case 1:
                    ToolbarString.Text = "Понедельник";
                    break;
                case 2:
                    ToolbarString.Text = "Вторник";
                    break;
                case 3:
                    ToolbarString.Text = "Среда";
                    break;
                case 4:
                    ToolbarString.Text = "Четверг";
                    break;
                case 5:
                    ToolbarString.Text = "Пятница";
                    break;
                case 6:
                    ToolbarString.Text = "Суббота";
                    break;
                case 0:
                    ToolbarString.Text = "Воскресенье";
                    break;
            }

            if (DayOfTheWeek == (int)DateTime.Now.DayOfWeek)
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
            await Navigation.PushAsync(new TodayTametable( sheetsOperator ,0, true));
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
            DayOfTheWeek += S;
            if (DayOfTheWeek < 0)
            {
                for (; DayOfTheWeek < 0; DayOfTheWeek += 7) ;
            }
            DayOfTheWeek = (DayOfTheWeek + 7) % 7;
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
            sheetsOperator.sheetsRequester.CheckData();
            ((LoadPage)App.Current.MainPage).StartLoad(sheetsOperator);
        }
    }
}