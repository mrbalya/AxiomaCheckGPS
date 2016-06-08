using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace AxiomaCheckGPS
{
    public class Queries
    {
        public static void UpdateGPS(string _table, string _map, string _gps_lat, string _gps_lng, string _error, int _morionid)
        {
            string connetionString = "Data Source=MR_BALYA-PC;Initial Catalog=AxiomaCheckGPS;User ID=checkers;Password=gonnacheck";
            SqlConnection connection;
            string query = null;
            _gps_lat = (_gps_lat == null) ? "null" : _gps_lat;
            _gps_lng = (_gps_lng == null) ? "null" : _gps_lng;
            query = "UPDATE AxiomaCheckGPS.dbo." + _table +
                    " SET " + _map + "_lat = " + _gps_lat + ", " + _map + "_lng = " + _gps_lng + ", " + _map + "_error = '" + _error + "' " +
                    " WHERE morionid = " + _morionid;
            connection = new SqlConnection(connetionString);
            connection.Open();
            SqlCommand updateGPS = new SqlCommand(query, connection);
            updateGPS.ExecuteNonQuery();
            connection.Close();
        }
    }    
}
