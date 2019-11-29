using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        //Event handler for any button pressed that contributes to the problem string ie all operators, numbers, decimal points
        private void buttonClick(object sender, EventArgs e)
        {
            Button num = (Button)sender;
            if (result.Text == "0" && (num.Text != "." && num.Text != "-"))
                result.Clear();

            result.Text = result.Text + num.Text;

        }


        //Sends equation to calculator class to be solved, catches any exceptions that are thrown and displays it on screen as well
        private void equals(object sender, EventArgs e)
        {
            Calculator calc = new Calculator(result.Text);
            if (calc.isValid(result.Text))
            {
                try
                {
                    result.Text = Convert.ToString(calc.calculate());
                }
                catch (Exception ex)
                {
                    result.Text = ex.Message;
                }
            }
            else result.Text = "Invalid Input";
        }


        //clears the last inputted character
        private void clearLast(object sender, EventArgs e)
        {
            if (result.Text.Length > 1)
                result.Text = result.Text.Remove(result.Text.Length - 1);
            else result.Text = "0";
        }


        //Resets the entire equation to 0
        private void clearAll(object sender, EventArgs e)
        {
            result.Text = "0";
        }
    }
}
