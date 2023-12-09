using System;
using System.Collections.Generic;
using System.Text;

namespace Fntt.Models.Web
{

    // Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);
    internal class ResponseModel ///////ответ
    {
        
        public string name { get; set; }
        public List<List<object>> timetable { get; set; }
    
    
    }
}
