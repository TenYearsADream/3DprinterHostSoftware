// -----------------------------------------------------------------------
// <copyright file="ThreedPrinterUpdateXMLversion.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace RepetierHost.view.utils
{
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Net.NetworkInformation;
using System.IO;
using RepetierHost.model;
using System.Xml;

    
    public class update
    {
        public update()
        {
        }

        public int buildnum = 0;
        public decimal version = 0;
        public int name = 0;
        public string language;
        public string updateTextExplanation;


    }    

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ThreedPrinterUpdateXMLversion
    {
    


     public   ThreedPrinterUpdateXMLversion()
        {
        }

        static List<update> listOfUpdates = new List<update>();

        public static int buildNumberForXMLUpdater = 1;
        public static int newestBuildAvailable;
        public static update newestUpdate = null;

        public static void CheckForUpdatesXML()
        {
            //try
            //{
                //string  url = "https://raw.github.com/garland3/3DprinterHostSoftware/master/Updates/versionNumber/version.txt";
                //string url = @"C:\Users\Anthony G\Documents\GitHub\Repetier-Host-mod\src\RepetierHost\Update\update.xml";
                string url=@"C:\Users\Anthony G\Documents\GitHub\Repetier-Host-mod\src\RepetierHost\Update\update.xml";
                XmlTextReader reader = new XmlTextReader(url);
                XmlDocument doc = new XmlDocument();
                doc.Load(reader);

                XmlNodeList rootlist = doc.GetElementsByTagName("updateDoc");
                //foreach (XmlNode n in rootlist)
                //{
                //    XmlAttribute att = n.Attributes["version"];
                //    if (att != null) language = att.InnerText;
                //}
                foreach (XmlNode n in doc.GetElementsByTagName("version"))
                {
                    update currentUpdateNode = new update();

                    XmlAttribute att = n.Attributes["number"];
                    if (att == null) continue; // missing id!
                    currentUpdateNode.version = Convert.ToDecimal(att.InnerText);

                    XmlAttribute att2 = n.Attributes["buildnumber"];
                    if (att2 == null) continue; // missing id!
                    currentUpdateNode.buildnum = Convert.ToInt32(att2.InnerText);

                    XmlAttribute att3 = n.Attributes["language"];
                    if (att3 == null) continue; // missing id!
                    currentUpdateNode.language = att3.InnerText;



                    currentUpdateNode.updateTextExplanation = n.InnerText.Trim();

                    listOfUpdates.Add(currentUpdateNode);

                }
            //}
            //catch (Exception e)
            //{
            //    MessageBox.Show("Error trying to check for updates. You may need to manually check for updates");
            //}

            foreach (update update in listOfUpdates)
            {
                if (update.buildnum > buildNumberForXMLUpdater)
                {
                    newestBuildAvailable = update.buildnum;
                    newestUpdate = update;
                }

            }

            if (newestBuildAvailable > buildNumberForXMLUpdater)
            {
                MessageUserToUpdate();
            }


        }

        private static void MessageUserToUpdate()
        {
            //RHUpdater updateFORM = new RHUpdater();
            //updateFORM.labelInstalledVersion.Text = buildNumberForXMLUpdater.ToString();
            //updateFORM.labelAvailableVersion.Text = newestUpdate.buildnum.ToString();
            //updateFORM.textUpdate.Text = newestUpdate.updateTextExplanation;
            //updateFORM.Visible = true;
            // throw new NotImplementedException();
            ThreedPrinterUpdateXMLversion updater = new ThreedPrinterUpdateXMLversion();

            //if (!Main.main.IsHandleCreated)
            //{
            //    Main.main.CreateHandle();
            //}

            Main.main.Invoke(Execute);

        }

        public static MethodInvoker Execute = delegate
        {
            if (RHUpdater.silent && RegMemory.GetInt("checkUpdateSkipBuild", 0) == ThreedPrinterUpdateXMLversion.newestUpdate.buildnum)
                return; // User didn't want to see this update.

            //if (form == null)
            //    form = new RHUpdater();

            RHUpdater.form.labelInstalledVersion.Text = ThreedPrinterUpdateXMLversion.buildNumberForXMLUpdater.ToString();
            RHUpdater.form.labelAvailableVersion.Text = ThreedPrinterUpdateXMLversion.newestUpdate.buildnum.ToString();
            RHUpdater.form.textUpdate.Text = ThreedPrinterUpdateXMLversion.newestUpdate.updateTextExplanation;
            RHUpdater.form.Show();

            //form.labelInstalledVersion.Text = currentVersion;
            //form.labelAvailableVersion.Text = newestVersion;
            //form.textUpdate.Text = updateText;
            //form.textUpdate.Select(0, 0);
            //form.Show();
        };


    }
}
