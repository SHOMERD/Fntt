using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Fntt.Models.Web
{

    public class RequestModel /////////запрос
    {
        public string requesType { get; set; }
        public string sheetName { get; set; }
        public string sheetID { get; set; }
        public object referenceObject { get; set; }

    }



    static class RequesTypeDictionari
    {
        static public Dictionary<string, string> requesTypeDictionari = new Dictionary<string, string>()
        {
            {"IsDataCurrent", "0"},
            { "GetSheetsNames","1"},
            { "GetSheetByNameMy" , "2"},
            { "GetSheetByIDMy" , "3"},
            { "GetAllSheets" , "4" },
        };
    }



    public class RequestModelConstructorC
    {
        /// <summary>
        /// /
        /// </summary>
        /// <param name=RequesTypeDictionari.requesTypeDictionari></param>
        /// <returns></returns>
        public static RequestModel RequestModelConstructor(string requesType = null, string sheetName = null, string sheetID = null, object referenceObject = null)
        {
            RequestModel requestModel = new RequestModel()
            {
                requesType = null,
                sheetName = null,
                sheetID = null,
                referenceObject = null
            };
            try
            {
                if (requesType == "4")
                {
                    requestModel.requesType = "4";
                }
                else if (requesType == "0")
                {
                    requestModel.referenceObject = referenceObject;
                    requestModel.requesType = "0";
                }
                else if ((requesType == null || requesType == "2") && sheetName != null)
                {
                    requestModel.sheetName = sheetName;
                    requestModel.requesType = "2";
                }
                else if ((requesType == null || requesType == "3") && sheetID != null)
                {
                    requestModel.sheetID = sheetID;
                    requestModel.requesType = "3";
                }
                else
                {
                    throw new Exception("Wronge Request!!!!!!!!!!!!!!!!!!");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }

            return requestModel;
        }
    }










}
