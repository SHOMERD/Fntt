using System;
using Fntt.Data;
using Fntt.Models;
using Xamarin.Forms;

namespace Fntt.Visual
{
    public partial class TodayTametable : ContentPage
    {
        public int DayOfTheWeek;
        public bool CanShouAll;
        SheetsOperator sheetsOperator;


        public TodayTametable(SheetsOperator sheetsOperator, int l, bool ShouAll = false)
        {
            InitializeComponent();
            this.CanShouAll = ShouAll;
            DayOfTheWeek = l;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            switch (DayOfTheWeek)
            {
                case 1:
                    Day.Text = "Понедельник";
                    break;
                case 2:
                    Day.Text = "Вторник";
                    break;
                case 3:
                    Day.Text = "Среда";
                    break;
                case 4:
                    Day.Text = "Четверг";
                    break;
                case 5:
                    Day.Text = "Пятница";
                    break;
                case 6:
                    Day.Text = "Суббота";
                    break;
                case 0:
                    Day.Text = "Воскресенье";
                    break;
            }
            //NotesDB database = await NotesDB.Instance;
            //if (CanShouAll)
            //{
            //    listView.ItemsSource = await database.GetItemsAsync();
            //}
            //else
            //{
            //    listView.ItemsSource = await database.SortingPendingNotes(DayOfTheWeek);
            //}

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