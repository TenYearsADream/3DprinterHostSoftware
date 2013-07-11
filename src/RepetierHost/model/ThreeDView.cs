using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform.Windows;
using OpenTK;
using RepetierHost.model;

namespace RepetierHost.view
{
    public delegate void onObjectMoved(float dx, float dy);
    public delegate void onObjectSelected(ThreeDModel selModel);

    /// <summary>
    /// ThreeDView uses 3D vectors to represent the position of the camera, the position to look at, the front and rear clip planes, the 
    /// rotational orientation of the view. 
    /// </summary>
    public class ThreeDView
    {
        FormPrinterSettings ps = Main.printerSettings;
        public onObjectMoved eventObjectMoved;
        public onObjectSelected eventObjectSelected;
        public static float zoom = 1.0f;
        public static Vector3 viewCenter;
        public static Vector3 userPosition;
        public static Matrix4 lookAt, persp, modelView;
        public static double normX = 0, normY = 0;
        public static float nearDist, farDist, aspectRatio, nearHeight;
        public static float rotZ = 0, rotX = 0;
        public static int mode = 0;
        public bool editor = false;
        public bool autoupdateable = false;
        public int slowCounter = 0; // Indicates slow framerates
        public uint timeCall = 0;
        public bool objectsSelected = false;
        public LinkedList<ThreeDModel> models;

        public ThreeDView()
        {
            viewCenter = new Vector3(0, 0, 0);
            rotX = 25;
            userPosition = new Vector3(0f * ps.PrintAreaWidth, -1.6f * (float)Math.Sqrt(ps.PrintAreaDepth * ps.PrintAreaDepth + ps.PrintAreaWidth * ps.PrintAreaWidth + ps.PrintAreaHeight * ps.PrintAreaHeight), 0.0f * ps.PrintAreaHeight);
            //userPosition = new Vector3(0, -1.7f * (float)Math.Sqrt(ps.PrintAreaDepth * ps.PrintAreaDepth + ps.PrintAreaWidth * ps.PrintAreaWidth), 0.0f * ps.PrintAreaHeight);
            models = new LinkedList<ThreeDModel>();
        }
        public void SetEditor(bool ed)
        {
            editor = ed;
        }

    }
}
