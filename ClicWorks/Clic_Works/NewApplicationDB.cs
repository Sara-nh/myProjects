using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Clic_Works
{
    public static class NewApplicationDB
    {

        public static int AddNewApplicatin(NewApplication newApp)
        {
            MySqlConnection con = ClicDBHelper.GetConnection();
            string sqlStatement = "INSERT INTO application (FirstName,LastName,DOB,PrimaryPhone,SecondaryPhone,ClientEmail,ClientAddress,ClientCity,ClientProv,ClientPostal,FormerClient,Children,CaseManagerId,ApplicationStatus,RequestDate)  VALUES(@FirstName, @LastName, @DOB, @PrimaryPhone, @SecondaryPhone, @ClientEmail, @ClientAddress, @ClientCity, @ClientProv, @ClientPostal, @FormerClient, @Children, @CaseManagerId, @ApplicationStatus, @RequestDate)";

            MySqlCommand cmd = new MySqlCommand(sqlStatement,con);

            cmd.Parameters.AddWithValue("@FirstName", newApp.newFirstName);        
            cmd.Parameters.AddWithValue("@LastName", newApp.newLastName);
            cmd.Parameters.AddWithValue("@DOB", newApp.newDOB);
            cmd.Parameters.AddWithValue("@PrimaryPhone", newApp.newPrimPhone);
            cmd.Parameters.AddWithValue("@SecondaryPhone", newApp.newSecPhone);
            cmd.Parameters.AddWithValue("@ClientEmail", newApp.newClientEmail);
            cmd.Parameters.AddWithValue("@ClientAddress", newApp.newClientAddress);
            cmd.Parameters.AddWithValue("@ClientCity", newApp.newClientCity);
            cmd.Parameters.AddWithValue("@ClientProv", newApp.newClientProv);
            cmd.Parameters.AddWithValue("@ClientPostal", newApp.newClientPostal);
            cmd.Parameters.AddWithValue("@FormerClient", newApp.newFormerClient);
            cmd.Parameters.AddWithValue("@Children", newApp.newChildren);
            cmd.Parameters.AddWithValue("@CaseManagerId", newApp.newCaseManager);
            cmd.Parameters.AddWithValue("@ApplicationStatus", "In Process");
            cmd.Parameters.AddWithValue("@RequestDate", newApp.newReqDate);

            try
            {
                con.Open();
                cmd.ExecuteNonQuery(); // run insert command   

                return 0;
               
              
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.ToString());
                throw ex;
                
            }
            finally
            {
                con.Close();
            }
        }
    }
}
