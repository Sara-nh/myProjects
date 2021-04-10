using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Clic_Works
{
    /* This class creates a connection with the database "clicdata" */

    public class ClicDBHelper
    {

        public static MySqlConnection GetConnection()
        {

           /** MySqlConnectionStringBuilder connString = new MySqlConnectionStringBuilder();
            connString.Server = "192.168.0.1";
            connString.Port = 3306;
            connString.UserID = "root";
            connString.Password = "root";
            connString.SslMode = MySqlSslMode.None;
            connString.Database = "clicdata";
            MySqlConnection conn = new MySqlConnection(connString.ToString());

            return conn;
    **/

            string connString = "Data Source=localhost;Initial Catalog=clicdata;User ID=root;Password=;Convert Zero Datetime=True";

            MySqlConnection conn = new MySqlConnection(connString.ToString());
            return conn;
        }

        
    }
}
