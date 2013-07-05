using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using RepetierHost.model;

namespace RepetierHost.view.utils
{
    /// <summary>
    /// Class related to showing a splash screen. If "dur" is 0 then no screen is displayed. The image to be displayed is
    /// contained with in the string "fileNameOfSplashScreen" and must be in the same folder as the application. 
    /// </summary>
    public partial class SplashScreen : Form
    {
        static string file;
        static string fileNameOfSplashScreen = "RandomImage.png";
        static int dur  = 0;
        static SplashScreen splash = null;

        /// <summary>
        /// Displays a Image in front of the Main application window during startup for "dur" seconds. 
        /// </summary>
        public static void run()
        {
           if (dur <= 0) return;
           file = Application.StartupPath + Path.DirectorySeparatorChar + fileNameOfSplashScreen;
            if (!File.Exists(file))
                return;
            splash = new SplashScreen(); // Make a new instance of the Splash Screen object
            splash.Show();
            splash.timer.Interval = 1000 * dur;
            splash.timer.Start();
        }

        /// <summary>
        /// Initialization of the splash screen object. 
        /// </summary>
        public SplashScreen()
        {
            InitializeComponent();
            Image img = Image.FromFile(file);
            BackgroundImage = img;
            Width = img.Width;
            Height = img.Height;
        }

        /// <summary>
        /// The timer will cause this event "Tick" to happen with the time runs out after is started. This function 
        /// tells the timer to stop, and to hide the Windows Form to get rid of the splash screen. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            Hide();
        }
    }
}
