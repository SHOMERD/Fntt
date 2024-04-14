using Fntt.Logics;
using Fntt.Visual;
using System;
using Fntt.Visual.BufferPages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fntt
{
    public partial class App : Application
    {
        public SheetsOperator sheetsOperator { get; set; }

        public App()
        {
            InitializeComponent();
            MainPage = new LoadPage();
            sheetsOperator = new SheetsOperator();
            ((LoadPage)MainPage).StartLoad(sheetsOperator);

        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
            
            MainPage = new LoadPage();
            sheetsOperator.sheetsRequester.RestartSheetReqester();
            ((LoadPage)MainPage).StartLoad(sheetsOperator);
        }
    }
}
