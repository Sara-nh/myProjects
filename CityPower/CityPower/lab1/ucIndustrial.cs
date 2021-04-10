using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab1
{
    public partial class ucIndustrial : UserControl
    {
        // return industrial instance
        private static ucIndustrial _instance;
        public static ucIndustrial Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ucIndustrial();

                return _instance;

            }

        }

        public List<Customer> mylist; // empty list
        public ucIndustrial()
        {
            InitializeComponent();
        }

        private void ucIndustrial_Load(object sender, EventArgs e)
        {
            //on load function
            DisplayCustomers();
            txtName.Focus();

        }

        // calculate total charge for all type of customers
        private double CalculateTotal()
        {
            double total = 0;
            foreach (Customer c in mylist)
                total += c.BillTotal();
            return total;
        }

        //calculate total charge for industrial customers
        private Double IndustrialTotal()
        {
            double total = 0;
            foreach (Customer c in mylist)
            {
                if (c.Type == "I")

                    total += c.BillTotal();

            }

            return total;
        }


        public void DisplayCustomers()
        {

            lstCustomers.Items.Clear(); // start with empty list box
            foreach (Customer c in mylist)
            {
                lstCustomers.Items.Add(c); // calls ToString()              
            }
            lblNrCustomers.Text = mylist.Count.ToString();
            lblCharIndustrial.Text = IndustrialTotal().ToString("c");
            lblCharCustomers.Text = CalculateTotal().ToString("c");
        }

        private void btnClear_Click(object sender, EventArgs e)
        {

            //clear the form           
            txtName.Text = "";
            txtAccount.Text = "";
            txtPUsage.Text = "";
            txtOPUsage.Text = "";
            lblBill.Text = "";

            //go to Residential 
            //Display the updated list
            ucResidential.Instance.mylist = CustomerDB.ReadCustomers();
            ucResidential.Instance.DisplayCustomers();
            ucResidential.Instance.BringToFront();
            txtName.Focus();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            //exit the form
            Application.Exit();
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {

            //Validation
            if (Validator.IsProvided(txtPUsage, "pusage") &&
                Validator.IsNonNegativeInteger(txtPUsage, "pusage") &&
                Validator.IsProvided(txtOPUsage, "opusage") &&
                Validator.IsNonNegativeInteger(txtOPUsage, "opusage") &&
               Validator.IsProvided(txtName, "name") &&
               Validator.IsProvided(txtAccount, "account") &&
               Validator.IsNonNegativeInteger(txtAccount, "account"))
            {

                // get inputs, create product, and add to inventory
                Customer c = new Customer();
                c.Name = txtName.Text;
                c.Account = Convert.ToInt32(txtAccount.Text);
                c.PUsage = Convert.ToInt32(txtPUsage.Text);
                c.OPUsage = Convert.ToInt32(txtOPUsage.Text);
                c.Type = "I";
                c.Bill = c.BillTotal();
                mylist.Add(c);
                // re-display customers

                DisplayCustomers();
                lblBill.Text = c.BillTotal().ToString("c");

                // save the new customer
                CustomerDB.SaveCustomers(mylist);
            }

        }

    }
}
