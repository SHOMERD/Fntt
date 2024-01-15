﻿using Fntt.Data;
using Fntt.Visual;
using System;
using Fntt.Visual.BufferPages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fntt
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            SheetsOperator sheetsOperator = new SheetsOperator();
            int userStatus = SheetsOperator.CheckUser();
            if (userStatus == -1)
            {
                MainPage = new UserForm();
            }
            else
            {

            }




            TodayTametable d = new TodayTametable(sheetsOperator, (int)DateTime.Now.DayOfWeek);
            CarouselPage CP = new CarouselPage();
            CP.Children.Add(new Minus1());
            CP.Children.Add(d);
            CP.Children.Add(new Plas1());
            CP.CurrentPage = CP.Children[1];

            MainPage = new NavigationPage(CP);
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
