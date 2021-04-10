using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace Clic_Works
{
    public partial class Form1 : Form
    {
        List<CaseManager> casemanager;
        Agency currentAgency;
        CaseManager currentcasemanager;

        public Form1()
        {
            InitializeComponent();        

            //default option selected in combo box
            comboBoxAddAdmin_User.SelectedIndex = 0;         
            comboBoxShowProd.SelectedIndex = 0;
            comboBoxFormerClient.SelectedIndex = 0;
            comboBoxCaseM.SelectedIndex = 0;
            comboBoxChildren.SelectedIndex = 0;

            //read xml file and display item type and brand list
            showTypeAndBrand();

            //Call function to display default date for former client and date requested
            defaultDateFormerClient_DateRequested();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string userType = LoginStatus.Users;

            //remove add person tab if regular user logs in
            if (userType.Equals("REGULAR")) 
            {
                tabData.TabPages.Remove(tabPage6);
            }

            //load casemanager in new application tab
            loadCaseManager();
            
        }

        //loadCaseManager function implementation
        public void loadCaseManager ()
        {
            if(comboBoxCaseMList.Text != "")
            {
                comboBoxCaseMList.Items.Clear();
            }
            try
            {
                casemanager = CaseManagerDB.GetAllCaseManager();
                foreach (var name in casemanager )
                {
                    comboBoxCaseMList.Items.Add(name.CMFirstName + " " +name.CMLastName);
                }

                comboBoxCaseMList.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().ToString());
            }
        }

        //Display default today's for former client and 

        public void defaultDateFormerClient_DateRequested()
        {
            //display date from datepicker control
            datePickerFormerClient.Format = DateTimePickerFormat.Custom;
            datePickerFormerClient.CustomFormat = "dd-MM-yy";
            datePickerFormerClient.Value = DateTime.Today;

            datePickerDateReq.Format = DateTimePickerFormat.Custom;
            datePickerDateReq.CustomFormat = "dd-MM-yy";
            datePickerDateReq.Value = DateTime.Today;

            textBoxFormerClient.Text = datePickerFormerClient.Value.ToString("dd-MM-yy");
            textBoxDateReq.Text = datePickerDateReq.Value.ToString("dd-MM-yy");
            
        }


        //Triggers when selection made in device type combo box
        private void comboBoxDeviceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("Brand.xml");
            XmlElement root = doc.DocumentElement;

            comboBoxBrand.Items.Clear();
            if (comboBoxDeviceType.SelectedIndex == 0)
            {
                XmlNodeList nodes = root.GetElementsByTagName("iname");

                for(int i=0;i<nodes.Count;i++)
                {
                    comboBoxBrand.Items.Add(nodes[i].InnerXml);
                }
            }
            else if (comboBoxDeviceType.SelectedIndex == 1)
            {
                XmlNodeList nodes = root.GetElementsByTagName("mname");

                for (int i = 0; i < nodes.Count; i++) 
                {
                    comboBoxBrand.Items.Add(nodes[i].InnerXml);
                }
            }
            else if (comboBoxDeviceType.SelectedIndex == 2)
            {
                XmlNodeList nodes = root.GetElementsByTagName("nname");

                for (int i = 0; i < nodes.Count; i++)
                {
                    comboBoxBrand.Items.Add(nodes[i].InnerXml);
                }
            }
            else if (comboBoxDeviceType.SelectedIndex == 3)
            {
                XmlNodeList nodes = root.GetElementsByTagName("pname");

                for (int i = 0; i < nodes.Count; i++)
                {
                    comboBoxBrand.Items.Add(nodes[i].InnerXml);
                }
            }
            comboBoxBrand.SelectedIndex = 0;
        }

        //function to show item types and brand names
        public void showTypeAndBrand()
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load("Brand.xml");

                foreach (XmlNode node in doc.DocumentElement)
                {
                    string name = node.Attributes[0].InnerText;
                    comboBoxDeviceType.Items.Add(name);
                }

                comboBoxDeviceType.SelectedIndex = 0; // first device type is by default selected

                XmlElement root = doc.DocumentElement;

                comboBoxBrand.Items.Clear();

                XmlNodeList nodes = root.GetElementsByTagName("iname");

                for (int i = 0; i < nodes.Count; i++)
                {
                    comboBoxBrand.Items.Add(nodes[i].InnerXml);
                }
                comboBoxBrand.SelectedIndex = 0; // first brand name by default selected
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure ?", "LOGOUT", MessageBoxButtons.YesNo);
            
            if (result == DialogResult.Yes)
            {
                this.Hide();
                FormLogin loginform = new FormLogin();
                loginform.Show();
            }
        }

        

        private void btnClearForm_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Sure", "Do you want to clear all data ?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                   //calling class function to clear all textfield 
                   ClearData.ClearTextFieldAndCombo(tabData.SelectedTab.Controls);
                   comboBoxFormerClient.SelectedIndex = 0;
                   comboBoxChildren.SelectedIndex = 0;
                   comboBoxCaseM.SelectedIndex = 0;
            }

            defaultDateFormerClient_DateRequested();
        }

        
        private void comboBoxFormerClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBoxFormerClient.SelectedIndex==0)
            {
                textBoxFormerClient.Visible = false;
                datePickerFormerClient.Visible = false;
                defaultDateFormerClient_DateRequested();
            }
            else if(comboBoxFormerClient.SelectedIndex == 1)
            {
                textBoxFormerClient.Visible = true;
                datePickerFormerClient.Visible = true;
            }
        }

        private void comboBoxCaseM_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxCaseM.SelectedIndex == 0)
            {
                comboBoxCaseMList.Visible = false;
                textBoxCMAgncyName.Visible = false;
                labelCMAgncyName.Visible = false;
                loadCaseManager();
                
            }
            else if (comboBoxCaseM.SelectedIndex == 1)
            {
                comboBoxCaseMList.Visible = true;
                

                if (comboBoxCaseMList.SelectedIndex !=-1)
                {
                    textBoxCMAgncyName.Visible = true;
                    labelCMAgncyName.Visible = true;
                }
            }
        }

        private void datePickerFormerClient_ValueChanged(object sender, EventArgs e)
        {
            textBoxFormerClient.Text = datePickerFormerClient.Text;
        }


        private void btnSubmitForm_Click(object sender, EventArgs e)
        {
            string firstName = textBoxFirstName.Text.Trim();
            string lastName =  textBoxLastName.Text.Trim();
            string dob =        textBoxDOB.Text.Trim();
            string primaryPhone = textBoxPriPhone.Text.Trim();
            string secondaryPhone = textBoxSecPhone.Text.Trim();
            string email = textBoxEmail.Text.Trim();
            string address = textBoxAddress.Text.Trim();
            string city = textBoxCity.Text.Trim();
            string province = textBoxProvince.Text.Trim();
            string postal = textBoxPostalCode.Text.Trim();
            string formerClient = textBoxFormerClient.Text.Trim();
            string children = comboBoxChildren.Text;
            int caseManagerid=0;
            if (comboBoxCaseMList.SelectedIndex > 0)
            {
                caseManagerid =comboBoxCaseMList.SelectedIndex;
            }           
            string dateRequested = textBoxDateReq.Text.Trim();

            if(firstName !="" && lastName !="" && dob !="" && primaryPhone != "" && secondaryPhone != "" && address != "" && city != "" 
                && province != "" && postal != "" && formerClient != "" && children != "" && dateRequested != "")
            {
                DateTime dateValue ;
                
               if(DateTime.TryParseExact(dob,"dd'-'MM'-'yy" ,CultureInfo.InvariantCulture,DateTimeStyles.None , out dateValue))
               {
                    NewApplication newApp = new NewApplication();
                    newApp.newFirstName = firstName;
                    newApp.newLastName = lastName;
                    DateTime dateDob = DateTime.ParseExact(dob, "dd-MM-yy", null);
                    newApp.newDOB = dateDob;
                    newApp.newPrimPhone = primaryPhone;
                    newApp.newSecPhone = secondaryPhone;
                    newApp.newClientEmail = email;
                    newApp.newClientAddress = address;
                    newApp.newClientCity = city;
                    newApp.newClientProv = province;
                    newApp.newClientPostal = postal;
                    newApp.newFormerClient = formerClient;
                    newApp.newChildren = Int32.Parse(children);
                    newApp.newCaseManager = caseManagerid ;
                    DateTime dateReq = DateTime.ParseExact(dob, "dd-MM-yy", null);
                    newApp.newReqDate = dateReq;

                    try
                    {
                        //add products by calling function "AddProducts"
                        int result = NewApplicationDB.AddNewApplicatin(newApp);

                        if (result == 0)
                        {
                            MessageBox.Show("Record Added Successfully");
                            ClearData.ClearTextFieldAndCombo(tabData.SelectedTab.Controls);
                            comboBoxFormerClient.SelectedIndex = 0;
                            comboBoxChildren.SelectedIndex = 0;
                            comboBoxCaseM.SelectedIndex = 0;
                            defaultDateFormerClient_DateRequested();
                            comboBoxFormerClient.SelectedIndex = 0;
                            comboBoxChildren.SelectedIndex = 0;
                            comboBoxCaseM.SelectedIndex = 0;
                        }
                        else
                        {
                            MessageBox.Show("ERROR !!");
                        }
               
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, ex.GetType().ToString());
                    }
                }
               else
               {
                    MessageBox.Show("Enter Valid Date of Birth");
                    textBoxDOB.Text = "";
               }
              
            }
            else
            {
                MessageBox.Show("Enter Required Data");
            }

        }

        private void datePickerDateReq_ValueChanged(object sender, EventArgs e)
        {
            textBoxDateReq.Text = datePickerDateReq.Value.ToString("dd-MM-yy");
        }

        private void textBoxPriPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBoxSecPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void comboBoxCaseMList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = comboBoxCaseMList.SelectedIndex;
            int agencyid = 0;
            if(index > 0)
            {
                currentcasemanager = CaseManagerDB.getCaseManager(index);
                agencyid = currentcasemanager.AgencyId;

                if(agencyid >0)
                {
                    currentAgency = AgencyDB.GetAllAgency(agencyid);
                    textBoxCMAgncyName.Text = currentAgency.AgencyName;
                }
             
               
            }
        }
    }
}
