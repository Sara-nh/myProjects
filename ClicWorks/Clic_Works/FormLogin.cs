using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Clic_Works
{
    public partial class FormLogin : Form
    {
       
        public FormLogin()
        {
            InitializeComponent();  
           
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            //Hide login form when user successfully logs in
            Form1 mainform = new Form1();
            FormLogin loginform = new FormLogin();

            

            string username = textBoxUserName.Text.Trim();
            string password = textBoxPassword.Text.Trim();

            //To check if entered username and password is exist in the database or not
            MySqlConnection con = ClicDBHelper.GetConnection();

            MySqlCommand cmd = new MySqlCommand("select * from loginadmin where userName like @username and password=@password;");
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password", password);

            cmd.Connection = con;
           
            try
            {
                con.Open();

                DataSet ds = new DataSet();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(ds);
                con.Close();

                bool loginSuccessful = ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0));

                if (loginSuccessful)
                {
                    //keep track if user is admin or regular user
                    LoginStatus.Users = "ADMIN";                
                    
                    //close login window and open main form if successful
                    mainform.Show();
                    this.Hide();
                }
                else
                {
                    cmd = new MySqlCommand("select * from loginusers where userName like @username and password=@password;");
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);

                    cmd.Connection = con;

                    con.Open();

                    ds = new DataSet();
                    da = new MySqlDataAdapter(cmd);
                    da.Fill(ds);
                    con.Close();

                    loginSuccessful = ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0));

                    if (loginSuccessful)
                    {
                        //keep track if user is admin or regular user
                        LoginStatus.Users = "REGULAR";

                        //close login window and open main form if successful
                        mainform.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Incorrect UserName or Password !!");
                        textBoxUserName.Text = "";
                        textBoxPassword.Text = "";
                    }
                    
                }
            }
            catch(MySqlException ex)
            {
               Console.WriteLine(ex.ToString());
                MessageBox.Show("Server Error");
            }

        }

        private void textBoxUserName_MouseHover(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Enter the username with minimum one character";
        }

        private void textBoxPassword_MouseHover(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Enter the password with minimum one character";
        }

        private void textBoxUserName_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }

        private void textBoxPassword_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {

        }
    }
}
