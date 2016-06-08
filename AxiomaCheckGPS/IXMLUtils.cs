using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AxiomaCheckGPS
{
    public class IXMLUtils
    {
        public static string RemoveAllNamespacesStr(string xmlDocument)
        {
            string pattern = " xmlns=[^>]*";//Explanation here: "https://regex101.com"
            Regex rgx = new Regex(pattern);
            return rgx.Replace(xmlDocument, "");
        }
    }
}
