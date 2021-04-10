using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace lab1
{
    public static class CustomerDB
    {
        const string path = "customers.txt";

        /// <summary>
        /// Reads data from the file if exists; or creates an empty file
        /// </summary>
        /// <returns>
        /// List of products if file existed, or empty list otherwise
        /// </returns>
        public static List<Customer> ReadCustomers()
        {
            List<Customer> customers = new List<Customer>(); // empty list
            FileStream fs = null;
            StreamReader sr = null;

            // for reading
            string line;
            string[] parts;
            Customer cust;
            try
            {
                // open the file for reading (the very first time, the file does not exist)
                fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read);
                sr = new StreamReader(fs);
                // read data
                while (!sr.EndOfStream) // while there is still data
                {
                    line = sr.ReadLine();
                    parts = line.Split(','); // split where commas are
                    // create another product and add to the list
                    cust = new Customer();
                    cust.Name = parts[0];
                    cust.Type = parts[1];
                    cust.Account = Convert.ToInt32(parts[2]);
                    if (cust.Type == "I")
                    {
                        cust.PUsage = Convert.ToInt32(parts[3]);
                        cust.OPUsage = Convert.ToInt32(parts[4]);
                        cust.Bill = Convert.ToDouble(parts[5]);
                        customers.Add(cust);
                    }
                    else
                    {
                        cust.Usage = Convert.ToInt32(parts[3]);
                        cust.Bill = Convert.ToDouble(parts[4]);
                        customers.Add(cust);
                    }
                    
                }

            }
            catch (Exception ex)
            {
                throw ex; // pass it to the calling code
            }
            finally
            {
                if (sr != null) sr.Close();
            }
            return customers;
        }

        public static void SaveCustomers(List<Customer> customers)
        {
            FileStream fs = null;
            StreamWriter sw = null;
            try
            {
                // open the file for writing; overwrite old content
                fs = new FileStream(path, FileMode.Create, FileAccess.Write);
                sw = new StreamWriter(fs);

                // save data
                //for (int i = 0; i < customers.Count; i++) // for each customer
                //{
                //    sw.WriteLine(customers[i].ToFileLine());
                //}
                foreach (Customer c in customers)
                    sw.WriteLine(c.ToFileLine());

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sw != null) sw.Close();
            }
        }
    }
}
