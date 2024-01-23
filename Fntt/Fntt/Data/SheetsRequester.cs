using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Fntt;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Fntt.Models.Web;
using static Google.Apis.Requests.BatchRequest;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Fntt.Visual;
using Fntt.Models.Local;




namespace Fntt.Data
{
    public class SheetsRequester
    { 
        public object allSheetsString { get; set; }
        public List<ResponseModel> allSheets { get; set; }
        public List<string> allSheetsNames { get; set; }

        //
        public int dataStatus
        {
            get { return DataStatus; }
            set
            {
                DataStatus = value;
                try
                {
                    ((LoadPage)App.Current.MainPage).ChekData(DataStatus);

                }catch (Exception ex) { }
            }
        }
        int DataStatus;



        public bool DataAccepted = false;
        





        public SheetsRequester()
        {
            DataAccepted = false;
            dataStatus = -1000;
            CheckData();
            

        }





        public async void CheckData()
        {
            var current = Connectivity.NetworkAccess;
            object sheetCashObject = null;

            if (current == NetworkAccess.Internet)
            {
                dataStatus = 0;
                UpdateData();

            }
            else if (Preferences.ContainsKey("AllSheetsCash")) 
            {
                dataStatus = 2;
            }
            else
            {   
                dataStatus = -1;
            }
        }


        public async Task<List<ResponseModel>> SheetsRequeste(string requesType = null, string sheetName = null, string sheetID = null, object referenceObject = null)
        {

            HttpClient client = new HttpClient();
            string uri = "https://script.google.com/macros/s/AKfycbxNOFVcKTe3j9qzNaQIT7AY5dSVad-kro7nW96jD8nsfMLrkFosNyDrwATPAu38sZVX/exec";

            var jsonString = JsonConvert.SerializeObject(RequestModelConstructorC.RequestModelConstructor(requesType, sheetName, sheetID, referenceObject));
            var requestContent = new StringContent(jsonString);
           
            var result = await client.PostAsync(uri, requestContent);
            var resultContent = await result.Content.ReadAsStringAsync();
            List<ResponseModel> response = JsonConvert.DeserializeObject<List<ResponseModel>>(resultContent);


            return response;
        }


        public async Task<List<string>> GetSheetsNamesFromGoogle()
        {
            string strings = SheetsRequeste("1").ToString();
            allSheetsNames = strings.Split(' ').ToList<string>();

            return allSheetsNames;
        }
        



        public async Task<bool> UpdateData()
        {
            List<ResponseModel> sheetRespone = await SheetsRequeste("0");
            allSheets = sheetRespone;
            Preferences.Set("AllSheetsCash", JsonConvert.SerializeObject(sheetRespone));
            dataStatus = 1;
            return true;

        }





        public async Task<bool> IsDataCurrent()
        {
            ResponseModel sheetCashObject;

            sheetCashObject = JsonConvert.DeserializeObject<ResponseModel>(Preferences.Get("AllSheetsCash", ""));

            object sheetRespone = await SheetsRequeste("0", null, null, sheetCashObject);


            if (sheetRespone.ToString() != "true")
            {
                allSheetsString = sheetRespone;

                Preferences.Set("AllSheetsCash", JsonConvert.SerializeObject(sheetRespone));
                return true;

            }
            else if (sheetRespone == null)
            {
                return false;
            }
            return false;
        }




        public async Task<object> GetSheetFromGoogle(string Title, int NamberOfSheet = -1)
        {
            object response = null;
            
            
            if (!string.IsNullOrEmpty(Title))
            {
                response = SheetsRequeste("2", Title) ;
            }

            if (string.IsNullOrEmpty(Title) && NamberOfSheet != -1)
            {
                response = SheetsRequeste("3", sheetID: NamberOfSheet.ToString());
            }
            


            return response;

        }


    }
}

