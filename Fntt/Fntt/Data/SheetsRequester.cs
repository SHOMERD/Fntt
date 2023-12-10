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




namespace Fntt.Data
{
    public class SheetsRequester
    { 
        public object allSheets { get; set; }
        public List<string> allSheetsNames { get; set; }




        public SheetsRequester()
        {
            UpdateData();


        }


        public void СhecData()
        {
            UpdateData();
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
        



        public async Task UpdateData()
        {
            object sheetRespone = await SheetsRequeste("0");
            allSheets = sheetRespone;
            App.Current.Properties["AllSheetsCash"] = sheetRespone; 


        }





        public async Task<bool> IsDataCurrent()
        {
            object sheetCashObject = null;
            App.Current.Properties.TryGetValue("AllSheetsCash", out sheetCashObject);

            object sheetRespone = await SheetsRequeste("0", null, null, sheetCashObject);


            if (sheetRespone.ToString() != "true")
            {
                allSheets = sheetRespone;
                App.Current.Properties["AllSheetsCash"] = sheetRespone;
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

