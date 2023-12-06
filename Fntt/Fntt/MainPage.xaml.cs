
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
using Xamarin.Forms;



using System.Net;
using System.Net.Http.Headers;



namespace Fntt
{


    public class Model
    {
        public string a { get; set; }
    }


    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            f();

        }
        public async Task f()
        {

            var client = new HttpClient();
            Model model = new Model()
            {
                a = t.Text
            };


            var uri = "https://script.google.com/macros/s/AKfycbxNOFVcKTe3j9qzNaQIT7AY5dSVad-kro7nW96jD8nsfMLrkFosNyDrwATPAu38sZVX/exec";
            var jsonString =  JsonConvert.SerializeObject(model);
            StringContent requestContent = new StringContent(jsonString);

            Console.WriteLine(requestContent);
            var result = await client.PostAsync(uri, requestContent);
            var resultContent = await result.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<object>(resultContent);
            
            SenterText.Text = response.ToString();

        }
    }
}
