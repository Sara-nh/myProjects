using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using micautLib;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        double x, y;
        string op;
        bool minus, dot;

        public Form1()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {

            textBox1.Text += button1.Text;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text += button2.Text;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text += button3.Text;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Text += button4.Text;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox1.Text += button5.Text;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox1.Text += button6.Text;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox1.Text += button7.Text;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            textBox1.Text += button8.Text;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            textBox1.Text += button9.Text;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
                return;
            else
                textBox1.Text += button10.Text;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = "0.";
                dot = true;
            }
            else
                if (!dot)
                {
                    textBox1.Text += button11.Text;
                    dot = true;
                }
        }



        private void button13_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                x = double.Parse(textBox1.Text);
                lblop.Text = Convert.ToString(x) + " - ";
                op = "-";
                textBox1.Clear();
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            x = double.Parse(textBox1.Text);
            lblop.Text = Convert.ToString(x) + " × ";
            op = "*";
            textBox1.Clear();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            x = double.Parse(textBox1.Text);
            lblop.Text = Convert.ToString(x) + " / ";
            op = "/";
            textBox1.Clear();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            if (!minus)
            {
                textBox1.Text = "-" + textBox1.Text;
                minus = true;
            }
            else
                return;

        }

        private void button17_Click(object sender, EventArgs e)
        {
            x = 0; y = 0;
            textBox1.Clear();
            lblop.Text = "";
            minus = false;
            dot = false;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            x = double.Parse(textBox1.Text);
            lblop.Text = Convert.ToString(x) + " + ";
            op = "+";
            textBox1.Clear();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            rdodegree.Checked = true;
            rdolbl.Checked = true;

        }

        private void button23_Click(object sender, EventArgs e)
        {
            minus = false;
            y = double.Parse(textBox1.Text);
            lblop.Text += Convert.ToString(y) + " = ";
            switch (op)
            {
                case "+":
                    textBox1.Text = Convert.ToString(x + y);
                    break;
                case "-":
                    textBox1.Text = Convert.ToString(x - y);
                    break;
                case "*":
                    textBox1.Text = Convert.ToString(x * y);
                    break;
                case "/":
                    textBox1.Text = Convert.ToString(x / y);
                    break;
                case "^":
                    textBox1.Text = Convert.ToString(Math.Pow(x, y));
                    break;
                case "mod":
                    textBox1.Text = Convert.ToString(x % y);
                    break;
                case "log":
                    textBox1.Text = Convert.ToString(Math.Log(x, y));
                    break;
                default:
                    break;
            }
        }

        private void button37_Click(object sender, EventArgs e)
        {
            lblop.Text = textBox1.Text + " ³ =";
            textBox1.Text = Convert.ToString(Math.Pow(double.Parse(textBox1.Text), 3));


        }

        private void button31_Click(object sender, EventArgs e)
        {
            lblop.Text = textBox1.Text + " ² =";
            textBox1.Text = Convert.ToString(Math.Pow(double.Parse(textBox1.Text), 2));
        }

        private void button22_Click(object sender, EventArgs e)
        {
            lblop.Text = textBox1.Text + " % = ";
            textBox1.Text = Convert.ToString((double.Parse(textBox1.Text)) / 100 * (double.Parse(textBox1.Text)));

        }

        private void button26_Click(object sender, EventArgs e)
        {
            x = double.Parse(textBox1.Text);
            lblop.Text = Convert.ToString(x) + " Mod ";
            op = "mod";
            textBox1.Clear();
        }

        private void button35_Click(object sender, EventArgs e)
        {

            double fac, z = double.Parse(textBox1.Text);
            lblop.Text = "! " + textBox1.Text + "=";
            fac = 1;
            for (int i = 1; i <= z; i++)
            {
                fac = fac * i;
            }
            textBox1.Text = Convert.ToString(fac);

        }

        private void button25_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                lblop.Text = " 10 ^ " + textBox1.Text + "=";
                textBox1.Text = Convert.ToString(Math.Pow(10, double.Parse(textBox1.Text)));
            }
        }

        private void button27_Click(object sender, EventArgs e)
        {
            x = double.Parse(textBox1.Text);
            lblop.Text = Convert.ToString(x) + " ^ ";
            op = "^";
            textBox1.Clear();
        }

        private void button32_Click(object sender, EventArgs e)
        {
            lblop.Text = "نماد علمی عدد " + textBox1.Text + "میشود";
            textBox1.Text = textBox1.Text + ".e+0";
        }

        private void button24_Click(object sender, EventArgs e)
        {
            double y, z = double.Parse(textBox1.Text);
            int w = 0;
            y = z;
            while (y % 10 == 0)
            {
                y = (y / 10);
                w++;
            }



            lblop.Text = " نماد علمی عدد " + textBox1.Text + " میشود ";
            textBox1.Text = y.ToString() + ".e+ " + w.ToString();
        }

        private void button33_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                dot = false;
                textBox1.Text = Convert.ToString(Math.Round(Convert.ToDouble(textBox1.Text)));

            }
        }

        private void button29_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                lblop.Text = "Cos " + textBox1.Text;
                textBox1.Text = Convert.ToString(Math.Cos(double.Parse(textBox1.Text)));//radian
                if (rdodegree.Checked)
                    
                    textBox1.Text = (double.Parse(textBox1.Text) * 57.2957795 ).ToString();
            }

        }

        private void button30_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                lblop.Text = "Sin " + textBox1.Text;

                textBox1.Text = Convert.ToString(Math.Sin(double.Parse(textBox1.Text)));
                if (rdodegree.Checked)
                    textBox1.Text = (double.Parse(textBox1.Text) * 57.2957795 ).ToString();

            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                lblop.Text = "Cosh " + textBox1.Text;
                textBox1.Text = Convert.ToString(Math.Cosh(double.Parse(textBox1.Text)));
                if (rdodegree.Checked)
                    textBox1.Text = (double.Parse(textBox1.Text) * 57.2957795 ).ToString();
            }
        }

        private void button36_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                lblop.Text = "Sinh " + textBox1.Text;
                textBox1.Text = Convert.ToString(Math.Sinh(double.Parse(textBox1.Text)));
                if (rdodegree.Checked)
                    textBox1.Text = (double.Parse(textBox1.Text) * 57.2957795 ).ToString();
            }
        }

        private void button34_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                lblop.Text = "Tan " + textBox1.Text;
                textBox1.Text = Convert.ToString(Math.Tan(double.Parse(textBox1.Text)));
                if (rdodegree.Checked)
                    textBox1.Text = (double.Parse(textBox1.Text) * 57.2957795 ).ToString();
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                lblop.Text = "Tanh " + textBox1.Text;
                textBox1.Text = Convert.ToString(Math.Tanh(double.Parse(textBox1.Text)));
            }
        }

        private void button38_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                lblop.Text = " " + textBox1.Text;
                textBox1.Text = Convert.ToString(Math.Sqrt(double.Parse(textBox1.Text)));
            }
        }

        private void button39_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                lblop.Text = "Sign " + textBox1.Text;
                textBox1.Text = Convert.ToString(Math.Sign(double.Parse(textBox1.Text)));
            }
        }

        private void button40_Click(object sender, EventArgs e)
        {
            x = double.Parse(textBox1.Text);
            lblop.Text = Convert.ToString(x) + " log  بر پایه ی";
            op = "log";
            textBox1.Clear();
        }

        private void button41_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                lblop.Text = "log10 " + textBox1.Text;
                textBox1.Text = Convert.ToString(Math.Log10(double.Parse(textBox1.Text)));
            }
        }

        private void button42_Click(object sender, EventArgs e)
        {

            if (textBox1.Text != "")
            {
                lblop.Text = "Abs " + textBox1.Text;
                textBox1.Text = Convert.ToString(Math.Abs(double.Parse(textBox1.Text)));
            }
        }



        private void button28_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                lblop.Text = "Tanh " + textBox1.Text;
                textBox1.Text = Convert.ToString(Math.Tanh(double.Parse(textBox1.Text)));
                if (rdodegree.Checked)
                    textBox1.Text = (double.Parse(textBox1.Text) * 57.2957795 ).ToString();
            }
        }

        private void button43_Click(object sender, EventArgs e)
        {
            if (rdotxt.Checked)
            {
                SpeechLib.SpVoiceClass voice = new SpeechLib.SpVoiceClass();
                voice.Speak(textBox1.Text, SpeechLib.SpeechVoiceSpeakFlags.SVSFDefault);
            }
            else if (rdoltext.Checked)
            {
                SpeechLib.SpVoiceClass voice = new SpeechLib.SpVoiceClass();
                voice.Speak(lblop.Text, SpeechLib.SpeechVoiceSpeakFlags.SVSFDefault);
            }
            else
            {
                SpeechLib.SpVoiceClass voice = new SpeechLib.SpVoiceClass();
                voice.Speak(lblop.Text + textBox1.Text, SpeechLib.SpeechVoiceSpeakFlags.SVSFDefault);
            }



        }

        private void button1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void button18_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length != 0)
                textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 1);

        }

        private void button44_Click(object sender, EventArgs e)
        {
            MathInputControl ctrl = new MathInputControlClass();
            ctrl.EnableExtendedButtons(true);
            ctrl.Show();
            ctrl.Close += () => Application.ExitThread();
        }



       





    








    }
}
