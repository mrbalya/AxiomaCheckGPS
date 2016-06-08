using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Net;
using System.Data.SqlClient;
using System.Data;

namespace AxiomaCheckGPS
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("AxiomaCheckGPS is started.");
            #region test YMapsML
            /*
            var xdoc = XDocument.Load("C:\\Users\\mr_balya\\Documents\\GitHub\\AxiomaCheckGPS\\result_example_yandex.xml");

            var xdocCleanStr = IXMLUtils.RemoveAllNamespacesStr(xdoc.ToString());
            xdoc = XDocument.Parse(xdocCleanStr);
            //System.IO.File.WriteAllText("C:\\Users\\mr_balya\\Documents\\GitHub\\AxiomaCheckGPS\\clean.xml", xdocCleanStr);
            //var results = xdoc.Element("ymaps").Element("GeoObjectCollection").Element("metaDataProperty").Element("step3");
            var results = xdoc.Element("ymaps").Element("GeoObjectCollection").Element("metaDataProperty").Element("GeocoderResponseMetaData").Element("results").Value;
            */
            #endregion

            string table;
            table = "company";
            List<Company> toCheck = Init(700, table, "all"); //_map: google, yandex, yahoo, all

            foreach (Company _company in toCheck)
            {
                Company.getGoogleGPS(_company, table);
                Company.getYandexGPS(_company, table);
            }

            Console.WriteLine("All Done :-)");
            Console.ReadLine();
        }

        public static List<Company> Init(int _cnt, string _table, string _map)
        {
            string connetionString = null;
            List<Company> _toReturn = new List<Company>();
            SqlConnection connection;
            string query = null;
            connetionString = "Data Source=MR_BALYA-PC;Initial Catalog=AxiomaCheckGPS;User ID=checkers;Password=gonnacheck";
            if (_map == "all")
                query = "SELECT TOP " + _cnt +
                    " id, morionid, postcode, country, region, city_type, city, " +
                    "street_type, street, building, morion_lat, morion_lng, " +
                    "isnull(google_lat, 0), isnull(google_lng, 0), isnull(yandex_lat, 0), isnull(yandex_lng, 0), isnull(yahoo_lat, 0), isnull(yahoo_lng, 0), name " +
                    "FROM AxiomaCheckGPS.dbo." + _table +
                    " WHERE google_error is null " +
                    " OR yandex_error is null " +
                    //" OR yahoo_error is null " +
                    " ORDER BY country, city, street, building";
            else
                query = "SELECT TOP " + _cnt +
                    " id, morionid, postcode, country, region, city_type, city, " +
                    "street_type, street, building, morion_lat, morion_lng, " +
                    "isnull(google_lat, 0), isnull(google_lng, 0), isnull(yandex_lat, 0), isnull(yandex_lng, 0), isnull(yahoo_lat, 0), isnull(yahoo_lng, 0), name " +
                    "FROM AxiomaCheckGPS.dbo." + _table + 
                    " WHERE " + _map + "_error is null " +
                    " ORDER BY country, city, street, building";
            connection = new SqlConnection(connetionString);
            connection.Open();

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Company company = new Company
                                        (reader.GetInt32(0),    //id
                                        reader.GetInt32(1),     //morionid
                                        reader.GetInt32(2),     //postcode
                                        reader.GetString(3),    //country
                                        reader.GetString(4),    //region
                                        reader.GetString(5),    //city_type
                                        reader.GetString(6),    //city
                                        reader.GetString(7),    //street_type
                                        reader.GetString(8),    //street
                                        reader.GetString(9),    //building
                                        reader.GetDouble(10),   //morion_lat
                                        reader.GetDouble(11),   //morion_lng
                                        reader.GetDouble(12),   //google_lat
                                        reader.GetDouble(13),   //google_lng
                                        reader.GetDouble(14),   //yandex_lat
                                        reader.GetDouble(15),   //yandex_lng
                                        reader.GetDouble(16),   //yahoo_lat
                                        reader.GetDouble(17),   //yahoo_lng
                                        reader.GetString(18)    //name
                            );
                        _toReturn.Add(company);
                    }
                }
            }

            connection.Close();
            return _toReturn;
        }
       
    }
}
