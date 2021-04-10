using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clic_Works
{
    public class AgencyDB
    {
        public static Agency GetAllAgency(int agencyId)
        {
            
            Agency currentAgency = null;
            MySqlConnection con = ClicDBHelper.GetConnection();

            string selectStatement = "SELECT * "
                                   +"From agency " 
                                   + "WHERE AgencyId=@AgencyId";

            MySqlCommand cmd = new MySqlCommand(selectStatement, con);
            cmd.Parameters.AddWithValue("@AgencyId", agencyId);

            try
            {
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    currentAgency = new Agency();

                    currentAgency.AgencyName = reader["AgencyName"].ToString();
                    currentAgency.AgencyAddress = reader["AgencyAddress"].ToString();
                    currentAgency.AgencyCity = reader["AgencyCity"].ToString();
                    currentAgency.AgencyProv = reader["AgencyProv"].ToString();
                    currentAgency.AgencyPostal = reader["AgencyPostal"].ToString();
                    currentAgency.AgencyEmail = reader["AgencyEmail"].ToString();
                    currentAgency.AgencyPhone = reader["AgencyPhone"].ToString();
                    currentAgency.AgencyFax = reader["AgencyFax"].ToString();  
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex);
                throw ex;

            }
            finally
            {
                con.Close();
            }

            return currentAgency;
        }
    }
}
