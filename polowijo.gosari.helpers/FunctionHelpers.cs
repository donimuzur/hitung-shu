using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace polowijo.gosari.helpers
{
    public class JsonModel
    {
        public string id { set; get; }
        public string data { set; get; }
    }
    public static class FunctionHelpers
    {
        public static bool IsValidDecimal(string str)
        {
            double i;
            return double.TryParse(str, out i) && i >= 1;
        }
        public static bool IsValidInteger(string str)
        {
            long i;
            return long.TryParse(str, out i) && i >= 1;
        }
        public static List<JsonModel> LoadJson(string Path)
        {
            var Items = new List<JsonModel>();
            using (StreamReader r = new StreamReader(Path))
            {
                string json = r.ReadToEnd();
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Items = jss.Deserialize<List<JsonModel>>(json);
            }

            return Items;
        }
    }
}
