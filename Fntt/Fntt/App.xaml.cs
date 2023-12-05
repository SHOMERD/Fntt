﻿using Fntt.Data;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fntt
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            SheetsRequester sheetsRequester = new SheetsRequester();
            MainPage = new MainPage();
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
