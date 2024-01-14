using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fntt.Visual.BufferPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Plas1 : ContentPage
    {
        public Plas1()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            CarouselPage CP = Parent as CarouselPage;
            TodayTametable NP = CP.Children[1] as TodayTametable;
            NP.ChangeDay(+1);
            CP.CurrentPage = CP.Children[1];
        }

    }
}