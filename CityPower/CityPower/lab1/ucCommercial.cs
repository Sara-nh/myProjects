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
    public partial class ucCommercial : UserControl
    {
        //return commercial instance
        private static ucCommercial _instance;
        public static ucCommercial Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ucCommercial();

                return _instance;

            }

        }

        public List<Customer> mylist; // empty list

        public ucCommercial()
        {
            InitializeComponent();
        }

        private void ucCommercial_Load(object sender, EventArgs e)
        {
            //on load function
            DisplayCustomers();
            txtName.Focus();
        }

        //calculate total charge for all type of customers
        private double CalculateTotal()
        {
            double total = 0;
            foreach (Customer c in mylist)
                total += c.BillTotal();
            return total;
        }

        //calculate total charge for commercial customers
        private Double CommercialTotal()
        {
            double total = 0;
            foreach (Customer c in mylist)
            {
                if (c.Type == "C")

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
            lblCharCommercial.Text = CommercialTotal().ToString("c");
            lblCharCustomers.Text = CalculateTotal().ToString("c");
        }

        private void btnClear_Click(object sender, EventArgs e)
        {

            //clear the form
            txtName.Text = "";
            txtAccount.Text = "";
            txtUsage.Text = "";
            lblBill.Text = "";

            //go to Residential 
            // Deisplay the updated list
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

            if (Validator.IsProvided(txtUsage, "usage") &&
               Validator.IsNonNegativeInteger(txtUsage, "usage") &&
               Validator.IsProvided(txtName, "name") &&
               Validator.IsProvided(txtAccount, "account") &&
               Validator.IsNonNegativeInteger(txtAccount, "account"))
            {

                // get inputs, create product, and add to inventory
                Customer c = new Customer();
                c.Name = txtName.Text;
                c.Account = Convert.ToInt32(txtAccount.Text);
                c.Usage = Convert.ToInt32(txtUsage.Text);
                c.Type = "C";
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
