using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace polowijo.gosari.dal
{
    public static class DBConnection
    {
        static OleDbConnection conn;
     
        public static OleDbConnection connect()
        {
            string SystemPath = System.AppDomain.CurrentDomain.BaseDirectory;
            //once you have the path you get the directory with:
            var directory = System.IO.Path.GetDirectoryName(SystemPath);

            var DBPath = directory + "\\Database\\database SHU.mdb";
            conn = new OleDbConnection("Provider=Microsoft.Jet.OleDb.4.0;"
                + "Data Source=" + DBPath);
            conn.Open();
            return conn;
        }

        public static void close()
        {
            if(conn != null)
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}
