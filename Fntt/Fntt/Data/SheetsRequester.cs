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


////////////////////////////////////////////////////////////////////////////
///
/// 
/// 
/// !!!!!!!!!!!!!!!!!!интернет обернуть в Try!!!!!!!!!!!!!!!!!!!!!!!!!!!!
/// 
/// 
/// 
////////////////////////////////////////////////////////////////////////////



namespace Fntt.Data
{
    public class SheetsRequester
    {
        static string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        static string ApplicationName = "Fntt";

        String spreadsheetId = "1FiMov0r4UUDKT6A56NWMImpoUakDC2YDevgaOpJQ7Qc";
        SheetsService sheetsService { get; set; }

        public Google.Apis.Sheets.v4.Data.Spreadsheet spreadsheet { get; private set; }

        public List<ValueRange> valueRanges { get; private set; }

        List<Google.Apis.Sheets.v4.Data.ValueRange> RealGigalist { get; set; }

        string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        string JsonCredentialsName = "credentials.json";

        string JsonTokenFileName = "token.json";



        public SheetsRequester()
        {
            if (IsDataCurrent())
            {
                Console.WriteLine("Fl");
            }


        }



        public bool IsDataCurrent()
        {
            object spreadSheetCashObject = null;
            Spreadsheet spreadsheetRespone = GetSpreadsheet();

            if (App.Current.Properties.TryGetValue("spreadSheetCash", out spreadSheetCashObject))
            {
                Spreadsheet spreadSheetCash = (Spreadsheet)spreadSheetCashObject;

                if (spreadSheetCash != null && !spreadsheetRespone.Equals(spreadSheetCash))
                {
                    return true;

                }
                else
                {
                    spreadsheet = spreadsheetRespone;
                    App.Current.Properties["spreadSheetCash"] = spreadsheetRespone;
                    UpdateAllSheetsCash();


                    return true;
                }

            }
            else
            {
                spreadsheet = spreadsheetRespone;
                App.Current.Properties["spreadSheetCash"] = spreadsheetRespone;
                UpdateAllSheetsCash();


                return true;
            }


            return false;
        }






        public Google.Apis.Sheets.v4.Data.Spreadsheet GetSpreadsheet()
        {
            try
            {
                UserCredential credential;

                var assembly = typeof(MainPage).Assembly;




                using (var stream =
                    assembly.GetManifestResourceStream("Fntt.credentials.json"))
                {
                    string credPath = "token.json";

                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.FromStream(stream).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None
                        
                        ).Result;

                }




                // Create Google Sheets API service.
                sheetsService = new SheetsService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName
                });

                // Define request parameters.
                String spreadsheetId = "1FiMov0r4UUDKT6A56NWMImpoUakDC2YDevgaOpJQ7Qc";

                SpreadsheetsResource.GetRequest request =
                    sheetsService.Spreadsheets.Get(spreadsheetId);

                return request.Execute();
            }
            catch (FileNotFoundException e)
            {
                 Console.WriteLine(e.Message);
                return null;
            }
        }


        public Google.Apis.Sheets.v4.Data.ValueRange GetSheetFromGoogle(string Title, int NamberOfSheet = -1)
        {
            String range;
            if (!string.IsNullOrEmpty(Title))
            {
                range = Title;
            }
            else if (NamberOfSheet != -1 && spreadsheet.Sheets.Count > NamberOfSheet)
            {
                range = $"{spreadsheet.Sheets[NamberOfSheet].Properties.Title}";
            }
            else
            {
                return null;
            }

            SpreadsheetsResource.ValuesResource.GetRequest request2 =
                sheetsService.Spreadsheets.Values.Get(spreadsheetId, range);


            ValueRange response = request2.Execute();

            return response;

        }




        public List<ValueRange> UpdateAllSheetsCash()
        {
            List<Google.Apis.Sheets.v4.Data.ValueRange> RealGigalistL = new List<Google.Apis.Sheets.v4.Data.ValueRange>();
            for (int i = 0; i < spreadsheet.Sheets.Count; i++)
            {
                RealGigalist.Add(GetSheetFromGoogle("", i));
            }


            App.Current.Properties["AllSheetsCash"] = RealGigalistL;
            this.valueRanges = RealGigalistL;

            return RealGigalist;
        }


        public ValueRange RequestToSheets()
        {
            try
            {
                UserCredential credential;
                // Load client secrets.
                using (var stream =
                       new FileStream("F:\\C#Progects\\NotMy\\sheets\\SheetsQuickstart\\credentials.json", FileMode.Open, FileAccess.Read))
                {

                    string credPath = "token.json";
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.FromStream(stream).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credPath, true)).Result;
                    Console.WriteLine("Credential file saved to: " + credPath);
                }


                var service = new SheetsService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName
                });


                String spreadsheetId = "1FiMov0r4UUDKT6A56NWMImpoUakDC2YDevgaOpJQ7Qc";
                String range = "'4 курс (копия)'";
                SpreadsheetsResource.ValuesResource.GetRequest request =
                    service.Spreadsheets.Values.Get(spreadsheetId, range);




                ValueRange response = request.Execute();
                return response;
            }
            catch (FileNotFoundException e)
            {
                return null;
            }
        }



        //    /* Global instance of the scopes required by this quickstart.
        //     If modifying these scopes, delete your previously saved token.json/ folder. */
        //    static string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        //    static string ApplicationName = "Google Sheets API .NET Quickstart";

        //    static void Main(string[] args)
        //    {
        //        try
        //        {
        //            UserCredential credential;
        //            // Load client secrets.
        //            using (var stream =
        //                   new FileStream("F:\\C#Progects\\NotMy\\sheets\\SheetsQuickstart\\credentials.json", FileMode.Open, FileAccess.Read))
        //            {
        //                /* The file token.json stores the user's access and refresh tokens, and is created
        //                 automatically when the authorization flow completes for the first time. */
        //                string credPath = "token.json";
        //                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
        //                    GoogleClientSecrets.FromStream(stream).Secrets,
        //                    Scopes,
        //                    "user",
        //                    CancellationToken.None,
        //                    new FileDataStore(credPath, true)).Result;
        //                Console.WriteLine("Credential file saved to: " + credPath);
        //            }

        //            // Create Google Sheets API service.
        //            var service = new SheetsService(new BaseClientService.Initializer
        //            {
        //                HttpClientInitializer = credential,
        //                ApplicationName = ApplicationName
        //            });

        //            // Define request parameters.
        //            String spreadsheetId = "1FiMov0r4UUDKT6A56NWMImpoUakDC2YDevgaOpJQ7Qc";
        //            String range = "'4 курс (копия)'";
        //            SpreadsheetsResource.ValuesResource.GetRequest request =
        //                service.Spreadsheets.Values.Get(spreadsheetId, range);

        //            // Prints the names and majors of students in a sample spreadsheet:
        //            // https://docs.google.com/spreadsheets/d/1BxiMVs0XRA5nFMdKvBdBZjgmUUqptlbs74OgvE2upms/edit


        //            ValueRange response = request.Execute();
        //            IList<IList<Object>> values = response.Values;
        //            if (values == null || values.Count == 0)
        //            {
        //                Console.WriteLine("No data found.");
        //                return;
        //            }
        //            Console.WriteLine("Name, Major");
        //            foreach (var row in values)
        //            {
        //                for (int i = 0; i < row.Count; i++)
        //                {
        //                    Console.Write(row[i] + "\t");
        //                }

        //                // Print columns A and E, which correspond to indices 0 and 4.
        //                Console.WriteLine();
        //            }
        //        }
        //        catch (FileNotFoundException e)
        //        {
        //            Console.WriteLine(e.Message);
        //        }
        //    }
        //}


    }
}

