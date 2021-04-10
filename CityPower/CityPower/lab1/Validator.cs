using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using lab1;

namespace lab1
{
    public static class Validator
    {
        // tests if text box contains anything
        public static bool IsProvided(TextBox inputBox, string name)
        {
            bool result = true; // "innocent until proven guilty"

            if (inputBox.Text == "")// bad!
            {
                result = false;
                MessageBox.Show(name + " has to be provided");
                inputBox.Focus();
            }
            return result;
        }


        // tests if text box contains non-negative integer value
        public static bool IsNonNegativeInteger(TextBox inputBox, string name)
        {
            bool result = true;
            int val; // to capture value from TryParse
            if (!Int32.TryParse(inputBox.Text, out val))// bad
            {
                result = false;
                MessageBox.Show(name + " must be a whole number");
                inputBox.SelectAll();
                inputBox.Focus();
            }
            else // it is an integer
            {
                if (val < 0) // bad
                {
                    result = false;
                    MessageBox.Show(name + " must be zero or more");
                    inputBox.SelectAll();
                    inputBox.Focus();
                }
            }
            return result;
        }

        // tests if text box contains positive double value
        public static bool IsPositiveDouble(TextBox inputBox, string name)
        {
            bool result = true;
            double val; // to capture value from TryParse
            if (!Double.TryParse(inputBox.Text, out val))// bad
            {
                result = false;
                MessageBox.Show(name + " must be a decimal number");
                inputBox.SelectAll();
                inputBox.Focus();
            }
            else // it is a double
            {
                if (val <= 0) // bad
                {
                    result = false;
                    MessageBox.Show(name + " must be greater than zero");
                    inputBox.SelectAll();
                    inputBox.Focus();
                }
            }
            return result;
        }
    }
}
