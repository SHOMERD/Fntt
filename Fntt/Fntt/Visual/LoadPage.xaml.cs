using Fntt.Data;
using Fntt.Visual.BufferPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fntt.Visual
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadPage : ContentPage
    {
        SheetsOperator sheetsOperator { get; set; }



        public LoadPage()
        {
            InitializeComponent();
            
        }

        public void StartLoad(SheetsOperator sheetsOperator)
        {
            this.sheetsOperator = sheetsOperator;
            ChekData(-1000);
        }



        public async void ChekData(int dataStatus)
        {
            

            LoadLable.Text = LoadLable.Text + ".";


            if (dataStatus == 1)
            {
                ChagePage();
            }

            if (dataStatus == 2)
            {
                if (!await DisplayAlert("Внимание", "Возможно данные устарели(нет соиденения)", "Понимаю", "Попробовать обновить данные"))
                {
                    sheetsOperator.sheetsRequester.CheckData();
                }
                else
                {
                    ChagePage();
                }
            }

            if (dataStatus == -1)
            {
                if (!(await DisplayAlert("Внимание", "Данных и доступа к сервису нет", "Попробовать обновить данные", "Закрыть приложение")))
                {
                    Application.Current.Quit();
                }
                else { sheetsOperator.sheetsRequester.CheckData(); }
            }


        }

        public void ChagePage()
        {            
            
            int userStatus = sheetsOperator.CheckUser();

            if (userStatus == -1)
            {
                App.Current.MainPage = new UserForm(sheetsOperator);
            }
            else if (userStatus == 1 || userStatus == 0)
            {
                sheetsOperator.SetData();
                new CarouselCreater(sheetsOperator, (int)DateTime.Now.DayOfWeek);
            }
        }




    }
}