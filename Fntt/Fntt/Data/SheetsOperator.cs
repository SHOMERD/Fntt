using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.Text;



namespace Fntt.Data
{
    public class SheetsOperator
    {
        SheetsRequester sheetsRequester;

        public SheetsOperator(SheetsRequester sheetsRequester)
        {
            this.sheetsRequester = sheetsRequester;
        }

        public string[] GetSheetsNames()
        {
            string[] Titles = new string[sheetsRequester.spreadsheet.Sheets.Count];
            for (int i = 0; i < Titles.Length; i++)
            {
                Titles[i] = sheetsRequester.spreadsheet.Sheets[i].Properties.Title;
            }

            return Titles;
        }

        public List<string> GetNamesOfGroupsFromSheet()
        {
            List<string> NamesOfGroups = new List<string>();



            return NamesOfGroups;
        }




    }
}
