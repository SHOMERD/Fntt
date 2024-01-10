
using Fntt.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;



using System.Net;
using System.Net.Http.Headers;
using Microsoft.Maui.Controls;
using Microsoft.Maui;



namespace Fntt
{


    public class Model
    {
        public string a { get; set; }
    }


    public partial class MainPage : ContentPage
    {
        SheetsRequester sheetsRequester;

        public MainPage()
        {

            sheetsRequester = new SheetsRequester();
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            sheetsRequester.chec();
        }
    }
}
