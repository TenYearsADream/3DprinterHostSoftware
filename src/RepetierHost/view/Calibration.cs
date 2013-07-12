using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RepetierHost.model;
using RepetierHost.view;
using RepetierHost.view.utils;

namespace RepetierHost.view
{
    public partial class Calibration : Form
    {
        /// <summary>
        /// Translation object. 
        /// </summary>
        public Trans trans = null;


        /// <summary>
        /// Indicates the current step the user is on. Starts at 0
        /// </summary>
        int currentStep = 0;


        /// <summary>
        /// Initializes a new instance of the Calibration class. 
        /// </summary>
        public Calibration()
        {
            InitializeComponent();
            RepetierHost.Main.connection.eventPrinterAction += updateButtons; // connection.eventPrinterAction += OnPrinterAction;
            label1.Text = "0";

            if (Main.main != null)
            {
                Main.main.languageChanged += translate;
                translate();
            }
        }


        private void translate()
        {
            this.upButton.Text = Trans.T("B_UP");
            this.downButton.Text = Trans.T("B_DOWN");
            this.NextButton.Text = Trans.T("B_NEXT");
            this.downButton.Text = Trans.T("B_DOWN");
            this.Text = Trans.T("Z_Height_Calibration_Wizard");
            this.ExitButton.Text = Trans.T("B_EXIT");

        }

        /// <summary>
        /// Updates the Up and Down buttons, so that they are only enabled when on step 2,3 and when the injectionCommand count is 0;
        /// </summary>
        /// <param name="action"></param>
        private void updateButtons(string action)
        {
            if ((currentStep == 2) || (currentStep == 3))
            {
                if (Main.connection.injectCommands.Count == 0)
                {
                    upButton.Enabled = downButton.Enabled = true;
                }
                else
                {
                    upButton.Enabled = downButton.Enabled = false;

                }
            }
        }

        /// <summary>
        /// Action to take on clicking the next button. Go to the next step and update everything. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NextButton_Click(object sender, EventArgs e)
        {
            currentStep++;
            UpdateStep();
        }

        private float incrementSize = 0;

        /// <summary>
        /// Updates everything to the current step
        /// </summary>
        private void UpdateStep()
        {
            // TODO: Make sure that the printer is actually connected. 
            if (Main.connection.connected == false)
            {
                currentStep = 0;
                label1.Text = Trans.T("L_CONNECT_A_PRINTER"); // "Please connect to a printer";
                this.toolStripStatusLabel1.Text = label1.Text;
                return;
            }

            switch (currentStep)
            {
                case 0:
                    // Intro
                    // Tell to stop all other activities. 
                    zHeadMoveAmout = 0;

                    label1.Text = Trans.T("L_CALIBRATION_WIZARD_STEP1"); //"This Wizard will guide you through the process of calibrating the height of your 3D printer.\n\n"+
                        //"Please follow the step by step instructions. \n\n" +
                        // "If you choose to click exit before the Wizard is complete then your changes will not be saved";
                    this.upButton.Visible = this.downButton.Visible = false;
                    this.NextButton.Enabled = true;
                    //Main.main.manulControl.zHeadMoveAmout = 0;
                    break;
                case 1:
                    // Note saying we are going to move up.
                    label1.Text = Trans.T("L_CALIBRATION_WIZARD_STEP2"); 
                //    label1.Text = "The factory settings for your printer specifies that the Z distance is 135 mm; however each printer is slightly different. " +
                //"In order to calibrate the exact height, we will move the printing platform up 130 mm and then slowly move it closer and closer to the print head." +
                //"When \"Next\" is clicked, the platform will move upto 130 mm";
                    this.upButton.Visible = this.downButton.Visible = false;
                    break;
                case 2:
                    // Home all, then Move the print head to the center and go up 130 mm
                    // May need to wait till 0 commands. 
                    // Give new instructions about moving up 1 mm at a time slowly. when get close then switch to 0.1 mm increment
                    // show the move up and down buttons. 

                    // label1.Text = "Once the head ahs moved up 130, you should now use the \"Up\" button to move the platform within 1 mm or less of the print head, then click \"Next\"";
                    label1.Text = Trans.T("L_CALIBRATION_WIZARD_STEP3"); 
                    Main.main.manulControl.buttonHomeAll_Click(null, null);
                    Main.main.manulControl.moveHeadToCenterXY();

                    // Go quickly up 125
                    Main.connection.maxZFeedRate += 300;
                    this.MoveHeadInZDirection(-125.0f);

                    // slow back down to the normal feed rate for the last 5.
                    Main.connection.maxZFeedRate -= 300;
                    this.MoveHeadInZDirection(-5.0f);

                    incrementSize = 1.0f;
                    this.upButton.Visible = this.downButton.Visible = true;
                    break;
                case 3:
                    label1.Text = Trans.T("L_CALIBRATION_WIZARD_STEP4"); 
                    //label1.Text = "Now, carefully move the head up in 0.1 mm increments until a piece of paper cannot move betwen the print nozzle and the print platform\n" +
                    //    "Press Next when you are done";
                    this.upButton.Visible = this.downButton.Visible = true;
                    incrementSize = 0.1f;

                    break;
                case 4:
                    label1.Text = Trans.T("L_CALIBRATION_WIZARD_STEP5") + " " + Math.Abs(zHeadMoveAmout).ToString();
                    this.upButton.Visible = this.downButton.Visible = false;                                   
                    incrementSize = 0.0f;                   
                    break;

                case 5:
                    label1.Text = Trans.T("L_CALIBRATION_WIZARD_STEP6"); 
                    //label1.Text = "The printer will is going back to the home position. Press \"Exit\" to end and save the results of the configuration wizard";
                    Main.main.manulControl.buttonHomeAll_Click(null, null);
                    this.upButton.Visible = this.downButton.Visible = false;
                    this.NextButton.Enabled = false;
                    incrementSize = 0.0f;
                    break;
            }

            this.toolStripStatusLabel1.Text = Trans.T("L_STEP") + " " + currentStep.ToString() + " " + Trans.T("L_Z_AXIS") + " " + Math.Abs(zHeadMoveAmout).ToString();
        }

        /// <summary>
        /// Action to take on clicking the exit button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitButton_Click(object sender, EventArgs e)
        {
            // If we are on step for then we are done and want to exit, no need to warn the user. 
            if (currentStep == 5)
            {
                // Save the results
                Main.printerSettings.textPrintAreaHeight.Text = Math.Abs(zHeadMoveAmout).ToString();
                Main.printerSettings.buttonOK_Click(null, null);
                
                // Remove this dialog from view. 
                this.Visible = false;
                return;
            }

            string message = Trans.T("M_SURE_TO_EXIT");
            string caption = Trans.T("B_EXIT");
            var result = MessageBox.Show(message, caption,
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Question);

            // If the no button was pressed ... 
            if (result == DialogResult.Yes)
            {
                this.Visible = false;
            }
        }

        /// <summary>
        /// When visibility of the form changes (ie becomes visible) then reset everything. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Calibration_VisibleChanged(object sender, EventArgs e)
        {
            currentStep = 0;
            UpdateStep();
        }

        /// <summary>
        /// Action to take when clicking the up button. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void upButton_Click(object sender, EventArgs e)
        {
            this.MoveHeadInZDirection(-incrementSize);
        }

        /// <summary>
        /// Used only for the calibration dialog to record the totaly amount of z movement. It is reset when  starting step 0.
        /// </summary>
        private static float zHeadMoveAmout = 0;


        /// <summary>
        /// Moves the print platform and the Z
        /// </summary>
        /// <param name="_amount"></param>
        private void MoveHeadInZDirection(float _amount)
        {
            zHeadMoveAmout += _amount;
            Main.main.manulControl.moveHeadInZ(_amount);
            this.toolStripStatusLabel1.Text = Trans.T("L_STEP") + " " + currentStep.ToString() + " " + Trans.T("L_Z_AXIS") + " " + Math.Abs(zHeadMoveAmout).ToString();
        }

        /// <summary>
        /// Action to take when clicking the down button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void downButton_Click(object sender, EventArgs e)
        {
            this.MoveHeadInZDirection(incrementSize);
        }
    }
}
