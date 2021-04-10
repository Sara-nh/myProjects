using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clic_Works
{
    public class CaseManagerDB
    {
        public static List<CaseManager> GetAllCaseManager()
        {
            List<CaseManager> casemanagers = new List<CaseManager>();
            CaseManager currentCaseManager = null;
            MySqlConnection con = ClicDBHelper.GetConnection();

            string selectStatement = "SELECT * "
                                     + "From casemanager";

            MySqlCommand cmd = new MySqlCommand(selectStatement, con);

            try
            {
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                while(reader.Read())
                {
                    currentCaseManager = new CaseManager();

                    currentCaseManager.CMFirstName = reader["CaseManagerFirstName"].ToString();
                    currentCaseManager.CMLastName = reader["CaseManagerLastName"].ToString();
                    currentCaseManager.AgencyId =Convert.ToInt32(reader["AgencyId"]);

                    casemanagers.Add(currentCaseManager);

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

            return casemanagers;
        }

        public static CaseManager getCaseManager(int casemanagerid)
        {
            CaseManager currentCaseManager = null;
            MySqlConnection con = ClicDBHelper.GetConnection();

            string selectStatement = "SELECT * "
                                   + "From casemanager "
                                   + "WHERE CaseManagerId=@CaseManagerId";

            MySqlCommand cmd = new MySqlCommand(selectStatement, con);
            cmd.Parameters.AddWithValue("@CaseManagerId", casemanagerid);


            try
            {
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    currentCaseManager = new CaseManager();

                    currentCaseManager.CMFirstName = reader["CaseManagerFirstName"].ToString();
                    currentCaseManager.CMLastName = reader["CaseManagerLastName"].ToString();
                    currentCaseManager.AgencyId = Convert.ToInt32(reader["AgencyId"]);

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

            return currentCaseManager;
        }



    }
}
