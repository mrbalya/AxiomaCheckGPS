using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;

namespace AxiomaCheckGPS
{
    public class Company
    {
        public int id { get; set; }
        public int morion_id { get; set; }
        public int postcode { get; set; }
        public string country { get; set; }
        public string region { get; set; }
        public string city_type { get; set; }
        public string city { get; set; }
        public string street_type { get; set; }
        public string street { get; set; }
        public string building { get; set; }
        public double morion_lat { get; set; }
        public double morion_lng { get; set; }
        public double google_lat { get; set; }
        public double google_lng { get; set; }
        public double yandex_lat { get; set; }
        public double yandex_lng { get; set; }
        public double yahoo_lat { get; set; }
        public double yahoo_lng { get; set; }
        public string name { get; set; }

        public Company() { }
        public Company(int _id) 
        {
            id = _id;
        }
        /* test
        public Company(int _id, int _morion_id, int _postcode, string _country, string _region,
                       string _city_type, string _city, string _street_type, string _street,
                       string _building, double _morion_lat, double _morion_lng, string _name)
        {
            id = _id;
            morion_id = _morion_id;
            postcode = _postcode;
            country = _country;
            region = _region;
            city_type = _city_type;
            city = _city;
            street_type = _street_type;
            street = _street;
            building = _building;
            morion_lat = _morion_lat;
            morion_lng = _morion_lng;
            name = _name;
        }
         * */
        public Company(int _id, int _morion_id, int _postcode, string _country, string _region,
                       string _city_type, string _city, string _street_type, string _street,
                       string _building, double _morion_lat, double _morion_lng, double _google_lat,
                       double _google_lng, double _yandex_lat, double _yandex_lng, double _yahoo_lat,
                       double _yahoo_lng, string _name) 
        {
            id = _id;
            morion_id = _morion_id;
            postcode = _postcode;
            country = _country;
            region = _region;
            city_type = _city_type;
            city = _city;
            street_type = _street_type;
            street = _street;
            building = _building;
            morion_lat = _morion_lat;
            morion_lng = _morion_lng;
            google_lat = _google_lat;
            google_lng = _google_lng;
            yandex_lat = _yandex_lat;
            yandex_lng = _yandex_lng;
            yahoo_lat = _yahoo_lat;
            yahoo_lng = _yahoo_lng;
            name = _name;
        }

        public static void getYandexGPS (Company _company, string _table)
        {
            var address = _company.country + ", " + _company.region + " область, " + _company.city +
                        ", " + _company.street_type + " " + _company.street + ", " + _company.building;
            var requestUri = string.Format("https://geocode-maps.yandex.ru/1.x/?geocode={0}&sensor=false&results=1", Uri.EscapeDataString(address));

            var request = WebRequest.Create(requestUri);
            var response = request.GetResponse();
            var xdoc = XDocument.Load(response.GetResponseStream());
            var xdocCleanStr = IXMLUtils.RemoveAllNamespacesStr(xdoc.ToString());
            xdoc = XDocument.Parse(xdocCleanStr);
            
            var results = xdoc.Element("ymaps").Element("GeoObjectCollection").Element("metaDataProperty").Element("GeocoderResponseMetaData").Element("results").Value;

            if (results == "0")
            {
                Console.WriteLine("ZERO_RESULTS: " + address);
                Queries.UpdateGPS(_table, "yandex", null, null, "ZERO_RESULTS", _company.morion_id);
            }
            else
            {
                Console.WriteLine("Found " + results + " results");
                var coords = xdoc.Element("ymaps").Element("GeoObjectCollection").Element("featureMember").Element("GeoObject").Element("Point").Value;
                Regex rgx = new Regex("\\s[0-9\\.]*");
                var lng = rgx.Replace(coords, "");
                rgx = new Regex("[0-9\\.]*\\s");
                var lat = rgx.Replace(coords, "");
                Queries.UpdateGPS(_table, "yandex", lat, lng, results + "_RESULTS", _company.morion_id);

            }

        }

        public static void getGoogleGPS (Company _company, string _table) 
        {
            var address = _company.country + ", " + _company.region + " область, " + _company.city + 
                        ", " + _company.street_type + " " + _company.street + ", " + _company.building;
            var requestUri = string.Format("http://maps.googleapis.com/maps/api/geocode/xml?address={0}&sensor=false", Uri.EscapeDataString(address));
            string coords;

            var request = WebRequest.Create(requestUri);
            var response = request.GetResponse();
            var xdoc = XDocument.Load(response.GetResponseStream());

            var status = xdoc.Element("GeocodeResponse").Element("status");

            switch (status.Value.ToString())
            {
                case "OK":
                    Console.WriteLine(status.Value);
                    var result = xdoc.Element("GeocodeResponse").Element("result");
                    var locationElement = result.Element("geometry").Element("location");
                    var lat = locationElement.Element("lat").Value;
                    var lng = locationElement.Element("lng").Value;
                    coords = lat + " " + lng;
                    Queries.UpdateGPS(_table, "google", lat, lng, status.Value, _company.morion_id);
                    Console.WriteLine(address + ": " + coords);
                    break;
                default:
                    Console.WriteLine(status.Value + ": " + address);
                    Queries.UpdateGPS(_table, "google", null, null, status.Value, _company.morion_id);
                    break;
            }
        }

        public static SqlCommand command { get; set; }
    }
}
