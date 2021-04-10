using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace CustomerMaintenance
{
    public static class CustomerDB
    {
        public static Customer GetCustomer(int customerID)
        {
            Customer cust = null;
            SqlConnection con = MMABooksDB.GetConnection();
            string selectStatement = "SELECT CustomerID, Name, Address, City, State, ZipCode " +
                                     "FROM Customers " +
                                     "WHERE CustomerID = @CustomerID";
            SqlCommand cmd = new SqlCommand(selectStatement, con);
            cmd.Parameters.AddWithValue("@CustomerID", customerID); // value comes from the method's argument
            try
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SingleRow);
                if(reader.Read()) // found a customer
                {
                    cust = new Customer();
                    cust.CustomerID = (int)reader["CustomerID"];
                    cust.Name = reader["Name"].ToString();
                    cust.Address = reader["Address"].ToString();
                    cust.City = reader["City"].ToString();
                    cust.State = reader["State"].ToString();
                    cust.ZipCode = reader["ZipCode"].ToString();
                }
            }
            catch(SqlException ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();                
            }
            return cust;
        }

        /// <summary>
        /// Adds a new customer to the Customers table in MMABooks database
        /// </summary>
        /// <param name="cust"> Customer object that cotaing data for the new record</param>
        /// <returns>generated CustomerID</returns>
        public static int AddCustomer(Customer cust)
        {
            SqlConnection con = MMABooksDB.GetConnection();
            string insertStatement = "INSERT INTO Customers (Name, Address, City, State, ZipCode) " +
                                     "VALUES(@Name, @Address, @City, @State, @ZipCode)";
            SqlCommand cmd = new SqlCommand(insertStatement, con);
            cmd.Parameters.AddWithValue("@Name", cust.Name);
            cmd.Parameters.AddWithValue("@Address", cust.Address);
            cmd.Parameters.AddWithValue("@City", cust.City);
            cmd.Parameters.AddWithValue("@State", cust.State);
            cmd.Parameters.AddWithValue("@ZipCode", cust.ZipCode);
            try
            {
                con.Open();
                cmd.ExecuteNonQuery(); // run the insert command
                // get the generated ID - current identity value for  Customers table
                string selectQuery = "SELECT IDENT_CURRENT('Customers') FROM Customers";
                SqlCommand selectCmd = new SqlCommand(selectQuery, con);
                int customerID = Convert.ToInt32(selectCmd.ExecuteScalar()); // single value
                                 // typecase (int) does NOT work!
                return customerID;
            }
            catch(SqlException ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }

        public static bool DeleteCustomer(Customer cust)
        {
            SqlConnection con = MMABooksDB.GetConnection();
            string deleteStatement = "DELETE FROM Customers " +
                                     "WHERE CustomerID = @CustomerID " + // to identify the customer to be  deleted
                                     "AND Name = @Name " + // remaining conditions - to ensure optimistic concurrency
                                     "AND Address = @Address " +
                                     "AND City = @City " +
                                     "AND State = @State " +
                                     "AND ZipCode = @ZipCode";
            SqlCommand cmd = new SqlCommand(deleteStatement, con);
            cmd.Parameters.AddWithValue("@CustomerID", cust.CustomerID);
            cmd.Parameters.AddWithValue("@Name", cust.Name);
            cmd.Parameters.AddWithValue("@Address", cust.Address);
            cmd.Parameters.AddWithValue("@City", cust.City);
            cmd.Parameters.AddWithValue("@State", cust.State);
            cmd.Parameters.AddWithValue("@ZipCode", cust.ZipCode);
            try
            {
                con.Open();
                int count = cmd.ExecuteNonQuery();
                if (count > 0) return true;
                else return false;
            }
            catch(SqlException ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }

        /// <summary>
        /// Updates existing customer record
        /// </summary>
        /// <param name="oldCust">data before update</param>
        /// <param name="newCust">new data for the update</param>
        /// <returns>indicator of success</returns>
        public static bool UpdateCustomer(Customer oldCust, Customer newCust)
        {
            SqlConnection con = MMABooksDB.GetConnection();
            string updateStatement = "UPDATE Customers " +
                                     "SET Name = @NewName, " +
                                     "    Address = @NewAddress, " +
                                     "    City = @NewCity, " +
                                     "    State = @NewState, " +
                                     "    ZipCode = @NewZipCode " +
                                     "WHERE CustomerID = @OldCustomerID " +
                                     "AND Name = @OldName " +
                                     "AND Address = @OldAddress " +
                                     "AND City = @OldCity " +
                                     "AND State = @OldState " +
                                     "AND ZipCode = @OldZipCode";
            SqlCommand cmd = new SqlCommand(updateStatement, con);
            cmd.Parameters.AddWithValue("@NewName", newCust.Name);
            cmd.Parameters.AddWithValue("@NewAddress", newCust.Address);
            cmd.Parameters.AddWithValue("@NewCity", newCust.City);
            cmd.Parameters.AddWithValue("@NewState", newCust.State);
            cmd.Parameters.AddWithValue("@NewZipCode", newCust.ZipCode);
            cmd.Parameters.AddWithValue("@OldCustomerID", oldCust.CustomerID);
            cmd.Parameters.AddWithValue("@OldName", oldCust.Name);
            cmd.Parameters.AddWithValue("@OldAddress", oldCust.Address);
            cmd.Parameters.AddWithValue("@OldCity", oldCust.City);
            cmd.Parameters.AddWithValue("@OldState", oldCust.State);
            cmd.Parameters.AddWithValue("@OldZipCode", oldCust.ZipCode);
            try
            {
                con.Open();
                int count = cmd.ExecuteNonQuery();
                if (count > 0) return true;
                else return false;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }
    }
}
