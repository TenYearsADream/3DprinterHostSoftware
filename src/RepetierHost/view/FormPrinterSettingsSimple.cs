using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RepetierHost.view.utils;
using RepetierHost.model;

namespace RepetierHost.view
{
    public partial class FormPrinterSettingsSimple : Form
    {
        FormPrinterSettings printerSettings;
        public FormPrinterSettingsSimple(FormPrinterSettings _printerSettings)
        {
            
            InitializeComponent();
            printerSettings = _printerSettings;          
            SyncPortComboBox();
            Main.main.languageChanged += translate;
            translate();
            
        }

         /// <summary>
        /// Translates all the text
        /// </summary>
        public void translate()
        {
            labelPort.Text = Trans.T("L_PORT");
            buttonOK.Text = Trans.T("B_OK");
            buttonRefreshPorts.Text = Trans.T("B_REFRESH_PORTS");
            this.Text = Trans.T("W_PRINTER_SETTINGS");
        }

        private void SyncPortComboBox()
        {
            this.comboPort.Items.Clear();
            foreach (Object item in printerSettings.comboPort.Items)
            {
                this.comboPort.Items.Add(item);     
            }

            this.comboPort.Text = this.printerSettings.comboPort.Text;            

            //// Select the last one which is normally the newest and the one that is actually connected to the printer. 
            //int count =  this.comboPort.Items.Count;
            //if (count!=0)
            //{
            //    this.comboPort.SelectedIndex = (this.comboPort.Items.Count-1);
            //}
            
        }        

        private void button1_Click(object sender, EventArgs e)
        {
            this.printerSettings.comboPort.Text = this.comboPort.Text;
            this.printerSettings.buttonOK_Click( sender,  e);
            this.Visible = false;
        }

        private void buttonRefreshPorts_Click(object sender, EventArgs e)
        {
            this.printerSettings.UpdatePorts();
            this.SyncPortComboBox();
        }

        private void buttonRefreshPorts_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible == true)
            {
                this.printerSettings.UpdatePorts();
                this.SyncPortComboBox();
            }
        }


    }
}
