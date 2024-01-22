using Fntt.Visual;
using Fntt.Visual.BufferPages;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Fntt.Data
{
     public class CarouselCreater
    {
        public CarouselCreater(SheetsOperator sheetsOperator, int setedDey)
        {
            TodayTametable todayTametable = new TodayTametable(sheetsOperator, setedDey);
            CarouselPage CP = new CarouselPage();
            CP.Children.Add(new Minus1());
            CP.Children.Add(todayTametable);
            CP.Children.Add(new Plas1());
            CP.CurrentPage = CP.Children[1];

            App.Current.MainPage = new NavigationPage(CP);

        }


    }
}
