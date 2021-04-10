using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clic_Works
{
    public class ClearData
    {
        public static string ClearTextFieldAndCombo(Control.ControlCollection frm)
        {
            foreach(Control ctl in frm)
            {
                if(ctl is TextBox)
                {
                    if (ctl.Name != "textBoxCity" && ctl.Name != "textBoxProvince")
                    {
                        ctl.Text = "";
                    }

                   
                }  
              
            }
            return null;
        }

        //public static bool FormatValid(Control.ControlCollection frm)
        //{
        //    foreach (Control ctl in frm)
        //    {
        //        if (ctl is TextBox)
        //        {
        //            if (ctl.Name == "textBoxDOB")
        //            {
        //                String txtDob = ctl.Text;
        //                DateTime dateVal = default(DateTime);
        //                if(DateTime.TryParse(txtDob,out dateVal))
        //                {
                           
        //                }
        //                else
        //                {
                            
        //                }
        //            }
        //        }
        //        else
        //        {
                   
        //        }

        //    }

        //    return true;
        //}
    }
}
