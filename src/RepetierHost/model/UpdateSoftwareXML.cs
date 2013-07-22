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
        public string webaddressToDownload;
    }

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UpdateSoftwareXML
    {
        public UpdateSoftwareXML()
        {
            CheckForUpdatesXMLOnStartup();
        }

        private void CheckForUpdatesXMLOnStartup()
        {
            //beSilent = true;
            //CheckForUpdatesXML();
            //throw new NotImplementedException();
        }

        static List<update> listOfUpdates = new List<update>();


        /// <summary>
        /// The current build number. Used to determine if we need to update. 
        /// </summary>
        public static int currentBuildNumber = 1;

        /// <summary>
        /// The newest build number that is available for download. This must be stored as a seperate value from an instance of the update class becaue objects
        /// aren't necessarly thread safe and we are using a new thread. 
        /// </summary>
        public static int updateBuildNumber = currentBuildNumber;

        /// <summary>
        /// An instance of the update class which containes the information about the newest update. it is populated only after running checkfor udpatexsml.
        /// </summary>
        public static update newestUpdate = null;

        /// <summary>
        /// True if new updates avaiable, false if no updates are avaible
        /// </summary>
        public static bool newUpdatesToDown = false;


        public static string updateExplanationText = null;
      

        public static bool clickedUpdateButton = false;


        /// <summary>
        /// Tries to access an xmml file online that holds information about the newest updates
        /// </summary>
        public static void CheckForUpdatesXML()
        {
            // WARNING, be sure to upload new update.xml files and change the build number declared above before releasing the software. 

            try
            {

                // Local computer for debugging. 
              //  string url=@"C:\Users\Anthony G\Documents\GitHub\Repetier-Host-mod\src\RepetierHost\Update\update.xml";

                // GIt hub
               // string url = @"https://raw.github.com/garland3/3DprinterHostSoftware/master/src/RepetierHost/Update/update.xml";

                // Google doc storage and app engine    // Contact garland3@gmail.com for help.
                string url = @"http://commondatastorage.googleapis.com/software3dprinting-ant-garl%2Fupdate.xml";



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


                    XmlAttribute att4 = n.Attributes["downloadURL"];
                    if (att4 == null) continue; // missing id!
                    currentUpdateNode.webaddressToDownload = att4.InnerText;



                    currentUpdateNode.updateTextExplanation = n.InnerText.Trim();

                    listOfUpdates.Add(currentUpdateNode);

                }
            }

                // catch the exeption where we can't connect to the internet for some reason. If they clicked update while not connected, than this is a  problem
            catch (WebException e)
            {
                if (clickedUpdateButton == true)
                {
                    string message = "A error was happened while attempting to connect to the update site online. Please check that you are connected to the internet";
                    string caption = "Connnection error"; //Trans.T("B_EXIT");
                    var result = MessageBox.Show(message, caption,
                                                 MessageBoxButtons.OK
                                                 );
                 
                }

                return;
            }
            catch (Exception e)
            {               
                string caption = "Connnection error"; //Trans.T("B_EXIT");
                var result = MessageBox.Show(e.Message, caption,
                                             MessageBoxButtons.OK
                                             );
                return;
            }

            foreach (update update in listOfUpdates)
            {
                // Greater than or equal lets us get the curret information. 
                if (update.buildnum >= currentBuildNumber)
                {
                    // Need to store the newest build number as a seperate "int" so that we dont' need an object on another thread. 
                    updateBuildNumber = update.buildnum;
                    updateExplanationText = update.updateTextExplanation;
                    newestUpdate = update;
                   
                }

              
                if (update.buildnum > currentBuildNumber)
                {
                    newUpdatesToDown = true;
                    
                }
            }


            // IF a new update exists, or if the user clicked on the update button, then show the message box showing them an update. 
            if ((newUpdatesToDown == true)|| (clickedUpdateButton == true))            {
               
                MessageUserToUpdate();
            }
        }

        /// <summary>
        /// Invokes the method that will show the form which lets the user update. 
        /// We must invoke the message becasue the updater is running on a different thread
        /// </summary>
        private static void MessageUserToUpdate()
        {

          


                Main.main.Invoke(ShowUpdateFormToUser);
        }

        /// <summary>
        /// Updates the information in the updater form so that the user can see an update if it exists
        /// </summary>
        public static MethodInvoker ShowUpdateFormToUser = delegate
        {

            //if (RHUpdater.form == null)
            //{
            //    RHUpdater.form = new RHUpdater();
            //}

            RHUpdater.form.labelInstalledVersion.Text = UpdateSoftwareXML.currentBuildNumber.ToString();
            RHUpdater.form.labelAvailableVersion.Text = UpdateSoftwareXML.updateBuildNumber.ToString();
            RHUpdater.form.textUpdate.Text = UpdateSoftwareXML.updateExplanationText;

            if (UpdateSoftwareXML.newUpdatesToDown == false)
            {
                RHUpdater.form.buttonDownload.Enabled = false;
            }
            
            RHUpdater.form.Show();

            // This prevvents someone from trying to accessed a disposed form. Probably not the best way to do it. 
            Main.main.checkForUpdatesToolStripMenuItem.Enabled = false;
        };
    }
}
