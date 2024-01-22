using System;
using Fntt.Data;
using Fntt.Models;
using Fntt.Visual.BufferPages;
using Xamarin.Forms;

namespace Fntt.Visual
{
    public partial class TodayTametable : ContentPage
    {
        public int DayOfTheWeek;
        public bool CanShouAll;
        SheetsOperator sheetsOperator;


        public TodayTametable(SheetsOperator sheetsOperator, int setedDey, bool ShouAll = false)
        {
            this.CanShouAll = ShouAll;
            DayOfTheWeek = setedDey;
            InitializeComponent();
            
        }

      

        protected override async void OnAppearing()
        {
            base.OnAppearing();
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

            if (CanShouAll)
            {
                listView.ItemsSource = sheetsOperator.GetWeekLesons();
            }
            else
            {
                //listView.ItemsSource = sheetsOperator.GetDayLesons(DayOfTheWeek);
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

    }
}