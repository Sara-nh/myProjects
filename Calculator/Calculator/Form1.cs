using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator
{
    public partial class Calculator : Form
    {
        double a, b, c;
        string op = null;
        bool flag=false;


        public Calculator()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtDisplay_TextChanged(null, null);
        }

        private void btnNumber_Click(object sender, MouseEventArgs e)
        {
            if (flag)
            {
                txtDisplay.Text = "";
                flag = false;
            }
            txtDisplay.Text += ((Button)sender).Text;
        }       

        private void equal(object sender, MouseEventArgs e)
        {
            b = Convert.ToDouble(txtDisplay.Text);

            switch (op)
            {
                case "+":
                    c = a + b;
                    break;
                case "-":
                    c = a - b;
                    break;
                case "*":
                    c = a * b;
                    break;
                case "/":
                    c = a / b;
                    break;
            }
            txtDisplay.Text = Convert.ToString(c);
            op = null;

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtDisplay.Text = "";
            a = b = c = 0;
        }

        private void txtDisplay_TextChanged(object sender, EventArgs e)
        {
            btnBackSpace.Enabled = Convert.ToBoolean(txtDisplay.Text.Length);
            btnClear.Enabled = Convert.ToBoolean(txtDisplay.Text.Length);
            btnPoint.Enabled = !txtDisplay.Text.Contains(".");

        }

        private void Calculator_KeyPress(object sender, KeyPressEventArgs e)
        {
            Button temp = new Button();
            temp.Text=e.KeyChar.ToString();
            if (e.KeyChar >= '0' && e.KeyChar <= '9')
                btnNumber_Click(temp, null);
            else if (e.KeyChar == '+' || e.KeyChar == '-' || e.KeyChar == '*' || e.KeyChar == '/')
                btnOperation_Click(temp, null);
            else if (e.KeyChar == '=')
                equal(null, null);
            else if (e.KeyChar == '.' && !txtDisplay.Text.Contains('.'))
                btnNumber_Click(temp, null);
            else if (e.KeyChar == '\b')
                btnBackSpace_Click(null, null);

            foreach (Button b in panel1.Controls)
            {

                if (e.KeyChar.ToString() == b.Text)
                    b.Focus();
                    } 
        }

        private void Calculator_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                equal(null, null);
                btnEqual.Focus();
            }
            else if (e.KeyCode == Keys.C)
            {
                btnClear_Click(null, null);
                btnClear.Focus();
            }
            else if (e.KeyCode == Keys.Back)
            {
                btnBackSpace_Click(null, null);
                btnBackSpace.Focus();
            }
        }

        private void btnOnOff_Click(object sender, MouseEventArgs e)
        {
            panel1.Enabled = !panel1.Enabled;
            if (btnOnOff.Text == "On")
            {
                this.KeyPreview = true;
                btnOnOff.Text = "Off";
            }
            else
            {
                btnClear_Click(null, null);
                btnOnOff.Text = "On";
            }

        }

        private void btnBackSpace_Click(object sender, EventArgs e)
        {
            txtDisplay_TextChanged(null, null);
            txtDisplay.Text = txtDisplay.Text.Remove(txtDisplay.Text.Length - 1, 1);
        }

        private void btnOperation_Click(object sender, MouseEventArgs e)
        {
            if(op!=null)
            {
                equal(null, null);

            }
            a = Convert.ToDouble(txtDisplay.Text);
            op = ((Button)sender).Text;
            flag = true;        
                     
        }
    }
}

