using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab1
{
    public partial class frmCustomerBill : Form
    {      
        public frmCustomerBill()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ucResidential.Instance.mylist = CustomerDB.ReadCustomers();
            panel.Controls.Add(ucResidential.Instance);
            ucResidential.Instance.Dock = DockStyle.Fill;
            ucResidential.Instance.BringToFront();               

         }

        //Add residential user control to panel
       
        private void btnResidential_Click(object sender, EventArgs e)
        {
            //Read and Display the customer file
            ucResidential.Instance.mylist = CustomerDB.ReadCustomers();
            ucResidential.Instance.DisplayCustomers();

            if (!panel.Controls.Contains(ucResidential.Instance))
            {
                panel.Controls.Add(ucResidential.Instance);
                ucResidential.Instance.Dock = DockStyle.Fill;
                ucResidential.Instance.BringToFront();
            }
            else
                ucResidential.Instance.BringToFront();
                
        }
        //Add commercial user control to panel
        private void btnCommercial_Click(object sender, EventArgs e)
        {
            //Read and Display the customer file
            ucCommercial.Instance.mylist = CustomerDB.ReadCustomers();
            ucCommercial.Instance.DisplayCustomers();

            if (!panel.Controls.Contains(ucCommercial.Instance))
            {
                panel.Controls.Add(ucCommercial.Instance);
                ucCommercial.Instance.Dock = DockStyle.Fill;
                ucCommercial.Instance.BringToFront();           
            }
            else
                ucCommercial.Instance.BringToFront();
        }
        //Add industrial user control to panel
        private void btnIndustrial_Click(object sender, EventArgs e)
        {
            //Read and Display the customer file
            ucIndustrial.Instance.mylist = CustomerDB.ReadCustomers();
            ucIndustrial.Instance.DisplayCustomers();

            if (!panel.Controls.Contains(ucIndustrial.Instance))
            {
                panel.Controls.Add(ucIndustrial.Instance);
                ucIndustrial.Instance.Dock = DockStyle.Fill;
                ucIndustrial.Instance.BringToFront();
            }
            else
                ucIndustrial.Instance.BringToFront();                
        }

        
    }
}
