//I, Xuan Huy Pham, 000899551 certify that this material is my original work.  No other person's work has been used without due acknowledgement.
using System;
using System.Linq;
using System.Windows.Forms;

namespace Lab2B
{
    /// <summary>
    /// Xuan Huy Pham - ID: 000899551
    /// Assignment 2B 
    /// Modified on Oct 8
    /// This assignment is to make use of GUI interface that determines pricing for a hair solon
    /// </summary>
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //set the name of the application
            this.Text = "Huy's Salon";

            //attach event handlers to 3 buttons
            calculateButton.Click += CalculateButton_Click;
            clearButton.Click += ClearButton_Click;
            exitButton.Click += ExitButton_Click;

            //set janeButton as the focused button
            janeButton.Focus();

            //set "Standard Adult" as the default option in client type field
            adultButton.Checked = true;
        }

        //CONFIGURE THE EVENT HANDLER FOR 3 MAIN BUTTONS
        /// <summary>
        /// configure the event handler for the Calculate button
        /// </summary>
        /// <param name="sender">the sender object that triggered the event</param>
        /// <param name="e">the event arguments</param>
        private void CalculateButton_Click(object sender, EventArgs e)
        {
            //this is to check if at least one service is selected
            bool atLeastOneServiceSelected = servicesGroupBox.Controls.OfType<CheckBox>().Any(cb => cb.Checked);

            //this is to check if input for the number of visits is a valid positive integer
            if (!int.TryParse(visitBox.Text, out int numVisits) || numVisits <= 0)
            {
                //if input for number of visits is invalid, show the error message and set the focus to the text field
                MessageBox.Show("Please enter a valid positive integer.", "Invalid number of visits", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //set the focus to visitBox
                visitBox.Focus();
                return; // the calculation won't be proceeded
            }

            //this is to check if there's at least one service is selected
            //if there's no one selected, display the error message
            if (!atLeastOneServiceSelected)
            {
                MessageBox.Show("Please select at least one service.", "No service selected", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; //the calculation won't be proceeded
            }

            //calculate the total price
            double baseRate = GetSelectedBaseRate();//the base rate of each hair dresser
            double serviceRate = GetSelectedServiceRate();//the additional fee based on selected services
            double discountRate = GetSelectedDiscountRate();//the amount of discount
            double totalPrice = (baseRate + serviceRate) * (1 - discountRate / 100.0);

            //apply discount based on the number of visits
            //no discount for 1 to 3 visits
            //case 4 to 8 visits:
            if (numVisits >= 4 && numVisits <= 8)
            {
                totalPrice *= 0.95; //the totalPrice is reduced by multiplying it by 0.95, or 5% discount
            }
            //case 9 to 13 visits
            else if (numVisits >= 9 && numVisits <= 13)
            {
                totalPrice *= 0.90; //10% discount
            }
            //case more than 14 visits
            else if (numVisits >= 14)
            {
                totalPrice *= 0.85; //15% discount
            }
            //display the total price in the totalBox
            //use "C" as the format specifier to formats the number to the currency $
            totalBox.Text = totalPrice.ToString("C");
        }

        /// <summary>
        /// configure the event handler for the Clear button
        /// </summary>
        /// <param name="sender">the object that triggered the event</param>
        /// <param name="e">the event arguments</param>
        private void ClearButton_Click(object sender, EventArgs e)
        {
            //clear all controls and reset the selections
            //reset the selection of hairdresser and set the first radio button as checked
            hairdresserGroupBox.Controls.OfType<RadioButton>().First().Checked = true;
            //unchecked all the checkboxes
            servicesGroupBox.Controls.OfType<CheckBox>().ToList().ForEach(cb => cb.Checked = false);
            //reset the selection of client type and set adultButton as checked
            typeGroupBox.Controls.OfType<RadioButton>().First().Checked = true;
            adultButton.Checked = true;
            //clear the input in the visitBox(number of visits) and the totalBox
            visitBox.Text = string.Empty;
            totalBox.Text = string.Empty;

            //set the focused radio button as janeButton in Hairdresser
            janeButton.Focus();
        }

        /// <summary>
        /// configure the event handler for the Exit button
        /// </summary>
        /// <param name="sender">the object that triggered the event</param>
        /// <param name="e">the event argument</param>
        private void ExitButton_Click(object sender, EventArgs e)
        {
            //close the app
            Close();
        }


        /// <summary>
        /// retrieve the base rate based on the selected hairdresser
        /// </summary>
        /// <returns>the base rate</returns>
        private double GetSelectedBaseRate()
        {
            //return corresponding base rate based on the selected hair dresser
            if (janeButton.Checked) return 30.0;
            if (patButton.Checked) return 45.0;
            if (ronButton.Checked) return 40.0;
            if (sueButton.Checked) return 50.0;
            if (lauraButton.Checked) return 55.0;
            //if there's no hairdresser selected, return the base rate as 0
            return 0.0;
        }


        //CALCULATION
        /// <summary>
        /// calculate the total rate based on the selected services
        /// </summary>
        /// <returns>the total rate</returns>
        private double GetSelectedServiceRate()
        {
            //initialize the total rate to 0
            double totalServiceRate = 0.0;
            //check for selected services
            foreach (CheckBox serviceCheckBox in servicesGroupBox.Controls.OfType<CheckBox>())
            {
                //if the service box is checked
                if (serviceCheckBox.Checked)
                {
                    //based on the selected services, add its rate to the total
                    switch (serviceCheckBox.Text)
                    {
                        case "Cut":
                            totalServiceRate += 30.0;
                            break;
                        case "Colour":
                            totalServiceRate += 40.0;
                            break;
                        case "Highlights":
                            totalServiceRate += 50.0;
                            break;
                        case "Extensions":
                            totalServiceRate += 200.0;
                            break;
                    }
                }
            }
            return totalServiceRate;//return the total rate
        }

        /// <summary>
        /// calculate the discount rate based on the selected client type
        /// </summary>
        /// <returns>the discount rate</returns>
        private double GetSelectedDiscountRate()
        {
            //return the corresponding discount rate based on the selected type of client
            if (adultButton.Checked) return 0.0;//no discount
            if (childButton.Checked) return 10.0;//10%
            if (studentButton.Checked) return 5.0;//5%
            if (seniorButton.Checked) return 15.0;//15%
            return 0.0; //if no option is selected
        }
    }
}
