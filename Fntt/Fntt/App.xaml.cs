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
            MainPage = new LoadPage();
            ((LoadPage)MainPage).StartLoad(new SheetsOperator());

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
