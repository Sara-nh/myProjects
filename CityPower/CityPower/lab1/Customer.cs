using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace lab1
{
    public class Customer
    {
           
              
        // private data and public accessors
        public string Name { get; set; }

        private double bill;
        public double Bill
        {
            get { return bill; }
            set { bill = (value < 0) ? 0 : value; }
        }

        private int usage;
        public int Usage
        {
            get { return usage; }
            set { usage = (value < 0) ? 0 : value; }
        }

        private int pusage;
        public int PUsage
        {
            get { return pusage; }
            set { pusage = (value < 0) ? 0 : value; }
        }

        private int opusage;
        public int OPUsage
        {
            get { return opusage; }
            set { opusage = (value < 0) ? 0 : value; }
        }

       
        private int account;
        public int Account
        {
            get { return account; }
            set { account = (value < 0) ? 0 : value; }
        }

        
        public string Type{ get; set; }

        // constructor
        public Customer(string n = "Unknown", int a = 0, string t = "Unknown", double b = 0, int u = 0, int pu = 0, int opu = 0 )
        {
            Name = n;
            Bill = b;
            Usage = u;
            PUsage = pu;
            OPUsage = opu;
            Type = t;
            Account = a;
                      
         }


        // Calculate the total charge for each customer(3 types)
        public double BillTotal()
        {
            switch (Type)
            {
                case "R":
                    //Rates
                    const double Flat_Rate = 6;
                    const Double Float_Rate = 0.052;
                                        
                    // calculate the Bill

                    Bill= Flat_Rate + Usage * Float_Rate;
                    break;
                        

                case "C":

                    //Rates
                    const int FlatRate = 60;
                    const double Base_Level = 1000;
                    const double FloatRate = 0.045;

                    // calculate the Bill
                    if (Usage <= Base_Level)
                    {
                        Bill= FlatRate;
                        break;
                    }
                    else
                    {
                        Bill= FlatRate + (Usage - Base_Level) * FloatRate;
                        break;
                    }

                case "I":

                    //Rates
                    const Double Peak_Flat_Rate = 76;
                    const Double BaseLevel= 1000;
                    const Double Peak_Float_Rate = 0.065;
                    const Double OffPeak_Flat_Rate = 40;
                    const Double OffPeak_Float_Rate = 0.028;
                    //partial totals
                    Double OP_B;
                    Double P_B;

                    // calculate the charge during peak hours
                    if (PUsage <= BaseLevel)
                    {
                        P_B = 76;
                    }
                    else
                    {
                        P_B = Peak_Flat_Rate + (PUsage - BaseLevel) * Peak_Float_Rate;
                    }
                    //Calculate the charge during off-peak hours
                    if (OPUsage <= BaseLevel)
                    {
                        OP_B = 40;
                    }
                    else
                    {
                        OP_B = OffPeak_Flat_Rate + (OPUsage - BaseLevel) * OffPeak_Float_Rate;
                    }
                    Bill= P_B + OP_B;
                    break;
            }
            return Bill;
        }
        
        
        //list's data
        public override string ToString()
        {

            return Name + ": " + Type + " - " + Account.ToString() + " - " +  Bill.ToString("c") ;
        }

        //file's data
        public string ToFileLine()
        {
            if (Type == "I")
            {
                return Name + "," + Type + "," + Account.ToString() + "," + PUsage.ToString() + "," + OPUsage.ToString() + "," + Bill.ToString();
            }
            else
            {

                return Name + "," + Type + "," + Account.ToString() + "," + Usage.ToString() + "," + Bill.ToString();
            }
        
        }
    }
}
