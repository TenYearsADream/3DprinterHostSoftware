﻿//-----------------------------------------------------------------------
// <copyright file="ThreeDControl.cs" company="Baoyan">
//   Some parts of this file were derived from Repetier Host which can be found at
// https://github.com/repetier/Repetier-Host Which is licensed using the Apache 2.0 license. 
// 
// Other parts of the file are property of Baoyan Automation LTC, Nanjing Jiangsu China.  
// 
//  Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
//-----------------------------------------------------------------------

namespace RepetierHost.view
{
    using System;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;
    using OpenTK.Graphics.OpenGL;
    using OpenTK;
    using System.Diagnostics;
    using System.Globalization;
    using RepetierHost.model;

    /// <summary>
    /// The user control ThreeDControl is the .stl OpenGL frame and Controller. Most of the rendering works happens here. 
    /// Basically a OpenGL wrapper as a userControl. 
    /// http://www.opentk.com/doc/chapter/2/glcontrol is very helpful. 
    /// </summary>
    public partial class ThreeDControl : UserControl
    {
        /// <summary>
        /// Printer settings. Important so that we know the size of the printer platform. 
        /// </summary>
        private FormPrinterSettings ps = Main.printerSettings;

        /// <summary>
        /// Indicates if the OpenTK control is loaded. Used to prevent painting and manipulation calls before the control is actually loaded. 
        /// </summary>
        private bool loaded = false;

        /// <summary>
        /// The X position of the Mouse click down event. Used for dragging the mouse will holding down the right click. 
        /// </summary>
        private float xDown;
        
        /// <summary>
        /// The Y position of the Mouse click down event. Used for dragging the mouse will holding down the right click. 
        /// </summary>
        private float yDown;

        /// <summary>
        /// Last recorded X position of the mouse. 
        /// </summary>
        private float xPos;

        /// <summary>
        /// Last recorded Y position of the mouse. 
        /// </summary>
        private float yPos;

        /// <summary>
        /// Speed in X direction of the mouse movement
        /// </summary>
        private float speedX;

        /// <summary>
        /// Speed in Y direction of the mouse movement
        /// </summary>
        private float speedY;

        /// <summary>
        /// The vector location of where the center of view (What we are looking at) is at the beginning of a mouse movement event. ViewCenter when we begin to move the mouse. 
        /// </summary>
        private Vector3 startViewCenter;

        /// <summary>
        /// The vector location of where the User Position (Camera position) is at the beginning of a mouse movement event.  
        /// </summary>
        private Vector3 startUserPosition;
       
        //// double normX = 0, normY = 0;

        /// <summary>
        /// The Z rotation when the mouse was first pressed down (and it is reset on the new mouse press down)
        /// </summary>
        private float startRotZ = 0;

        /// <summary>
        /// The X rotation when the mouse was first pressed down (and it is reset on the new mouse press down)
        /// </summary>
        private float startRotX = 0;

        /// <summary>
        /// The X location of the mouse on the last press down (reset on the next mouse press down);
        /// </summary>
        private float lastX;
        
        /// <summary>
        /// The Y location of the mouse on the last press down (reset on the next mouse press down);
        /// </summary>
        private float lastY;

        /// <summary>
        /// Options for what to do when dragging the mouse in the ThreeDController.       
        /// </summary>
        public enum modeOptions
        {
            /// <summary>
            /// Rotates the view when dragging the mouse. Old 0
            /// </summary>
            Rotation,

            /// <summary>
            /// Moves the Center of view when dragging the mouse. Old 1
            /// </summary>
            Move,

            /// <summary>
            /// Moves the Center of view when dragging the mouse. Old 2
            /// </summary>
            MoveViewpoint,

            /// <summary>
            /// Zooms of view when dragging the mouse. Old 3
            /// </summary>
            Zoom,

            /// <summary>
            /// Moves the object. Old 4
            /// </summary>
            MoveObject
        }

        /// <summary>
        /// Stop watch
        /// </summary>
        private Stopwatch sw = new Stopwatch();

        /// <summary>
        /// Frames per Second timer
        /// </summary>
        private Stopwatch fpsTimer = new Stopwatch();

        /// <summary>
        /// Current mode on how to manipulate the model when clicking and dragging with the mouse. 
        /// </summary>
        private static modeOptions currentMode = modeOptions.Rotation;

        /// <summary>
        /// Gets or sets the currentMode
        /// </summary>
        public static modeOptions CurrentMode
        {
            get { return ThreeDControl.currentMode; }
            set { ThreeDControl.currentMode = value; }
        }

        /// <summary>
        /// Indicates slow frame rates. If going slow then reduce quality of drawings. 
        /// </summary>
        private int slowCounter = 0; 

        /// <summary>
        /// Used with the ticker to help determine when to redraw. Uses modulus == something to determine redraw. 
        /// </summary>
        private uint timeCall = 0;

        /// <summary>
        /// Used to indicate if we should use parallel projection instead of perspective
        /// </summary>
        private static bool perspectiveModeisParrallel = false;

        /// <summary>
        /// Current 3DView to use. Important because it contains the model to draw. Each view mode has different models. 
        /// </summary>
        private ThreeDView view = null;

        /// <summary>
        /// Textwriter used to show if wer are in developer mode. 
        /// </summary>
        private  MyTextWriter modeText;

        /// <summary>
        /// Initializes a new instance of the ThreeDControl class which is the top level wrapper or controller for the openTK or openGL code
        /// </summary>
        public ThreeDControl()
        {
            InitializeComponent();


            gl.MouseWheel += gl_MouseWheel;
            timer.Start();
            translate();
            Main.main.languageChanged += translate;
        }

        /// <summary>
        /// Translate. Nothing to translate
        /// </summary>
        private void translate()
        {
        }

        /// <summary>
        /// Sets the current 3D view object. This should probably be done as a SET and GET property. TODO!
        /// </summary>
        /// <param name="view"></param>
        public void SetView(ThreeDView view)
        {
            this.view = view;
            UpdateChanges();
        }

        /// <summary>
        /// Sets if an object is selected. Not sure this function is really needed. TODO??
        /// </summary>
        /// <param name="sel">True if an object is selected</param>
        public void SetObjectSelected(bool sel)
        {
            view.objectsSelected = sel;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to updates the 3D view automatically. 
        /// </summary>
        public bool AutoUpdateable
        {
            get 
            { 
                return view.autoupdateable;
            }

            set
            {
                view.autoupdateable = value;
                if (view.autoupdateable)
                {
                    timer.Start();
                }
                else
                {
                    timer.Stop();
                }               
            }
        }

        /// <summary>
        /// Causes the Form to redraw by calling Invalidate
        /// </summary>
        public void UpdateChanges()
        {
            gl.Invalidate();
        }

        /// <summary>
        /// Paints the Form again. Not sure this is needed. No custom code is here. 
        /// </summary>
        /// <param name="pe"></param>
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        /// <summary>
        /// Sets up the openTL viewport based on the view Center and current Zoom. 
        /// </summary>
        private void SetupViewport()
        {
            try
            {
                int w = gl.Width;
                int h = gl.Height;
                GL.Viewport(0, 0, w, h); // Use all of the glControl painting area
            }
            catch 
            { 
            }
        }

        /// <summary>
        /// The main paint call for the controller. This is where the beginning of the openTK stuff begins. When ever you need to update the 3d view, call 
        /// Invalidate on the controller which will cause the windows form to cue the paint event. This is the handler for the paint event. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gl_Paint(object sender, PaintEventArgs e)
        {
            if (view == null)
            {
                return;
            }

            try
            {
                // Protect against tryint to run OpenTK code before it is loaded. 
                if (!loaded)
                {
                    return;
                }

                ThreeDSettings.drawMethod tempMethod = ThreeDSettings.currentDrawMethod; // hreeDSettings.currentDrawMethod;
                switch (Main.threeDSettings.comboDrawMethod.SelectedIndex)
                {
                    case 0: // Autodetect;
                        if (Main.threeDSettings.useVBOs && Main.threeDSettings.openGLVersion >= 1.499)
                        {
                            ThreeDSettings.currentDrawMethod = ThreeDSettings.drawMethod.VBO;
                            //// Main.threeDSettings.drawMethod = 2;
                        }
                        else if (Main.threeDSettings.openGLVersion >= 1.099)
                        {
                            ThreeDSettings.currentDrawMethod = ThreeDSettings.drawMethod.Elements;
                            //// Main.threeDSettings.drawMethod = 1;
                        }
                        else
                        {
                            ThreeDSettings.currentDrawMethod = ThreeDSettings.drawMethod.DrawElements;
                            ////Main.threeDSettings.drawMethod = 0;
                        }

                        break;

                    case 1: // VBOs
                        ThreeDSettings.currentDrawMethod = ThreeDSettings.drawMethod.VBO;
                        ////Main.threeDSettings.drawMethod = 2;
                        break;
                    case 2: // drawElements
                        ThreeDSettings.currentDrawMethod = ThreeDSettings.drawMethod.Elements;
                        ////Main.threeDSettings.drawMethod = 1;
                        break;
                    case 3: // elements
                        ThreeDSettings.currentDrawMethod = ThreeDSettings.drawMethod.DrawElements;
                        //// Main.threeDSettings.drawMethod = 0;
                        break;
                }

                // Not sure why this is here??
                if (tempMethod != ThreeDSettings.currentDrawMethod)
                {
                    Main.main.updateTravelMoves();
                }

                fpsTimer.Reset(); // Reset the stop watch
                fpsTimer.Start(); // Start the stop watch
                gl.MakeCurrent(); // Set all GL commands to the current rendering.                

                GL.ClearColor(Main.threeDSettings.background.BackColor); // Set the color that clears the screen. 
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                SetupViewport(); // Setup the opengl viewport

                // Draw the background gradient before we draw the 3D view. 
                DrawBackGroundGradient();

                if (Main.main.DeveloperMode == true)
                {
                    modeText.Draw(); //
                }

                // Switch to projection mode and setup our projection matrix. 
                GL.Enable(EnableCap.DepthTest); // Renable 3D
                SetupProjectionMatrix(); // Setup our projection matrix. 

                RepetierHost.view.ThreeDView.lookAt = Matrix4.LookAt(
                    RepetierHost.view.ThreeDView.userPosition.X, RepetierHost.view.ThreeDView.userPosition.Y, RepetierHost.view.ThreeDView.userPosition.Z,
                    RepetierHost.view.ThreeDView.viewCenter.X, RepetierHost.view.ThreeDView.viewCenter.Y, RepetierHost.view.ThreeDView.viewCenter.Z, 0, 0, 1.0f);

                // Change to the Model View Mode. 
                GL.MatrixMode(MatrixMode.Modelview); 
                GL.LoadMatrix(ref RepetierHost.view.ThreeDView.lookAt);

                // Begin customizing the specifics about the view. 
                GL.ShadeModel(ShadingModel.Smooth);
                GL.Light(LightName.Light0, LightParameter.Ambient, new float[] { 0.2f, 0.2f, 0.2f, 1f });
                GL.Light(LightName.Light0, LightParameter.Diffuse, new float[] { 0, 0, 0, 0 });
                GL.Light(LightName.Light0, LightParameter.Specular, new float[] { 0, 0, 0, 0 });
                GL.Enable(EnableCap.Light0);
                if (Main.threeDSettings.enableLight1.Checked)
                {
                    GL.Light(LightName.Light1, LightParameter.Ambient, Main.threeDSettings.Ambient1());
                    GL.Light(LightName.Light1, LightParameter.Diffuse, Main.threeDSettings.Diffuse1());
                    GL.Light(LightName.Light1, LightParameter.Specular, Main.threeDSettings.Specular1());
                    GL.Light(LightName.Light1, LightParameter.Position, Main.threeDSettings.Dir1());
                    GL.Enable(EnableCap.Light1);
                }
                else
                {
                    GL.Disable(EnableCap.Light1);
                }

                if (Main.threeDSettings.enableLight2.Checked)
                {
                    GL.Light(LightName.Light2, LightParameter.Ambient, Main.threeDSettings.Ambient2());
                    GL.Light(LightName.Light2, LightParameter.Diffuse, Main.threeDSettings.Diffuse2());
                    GL.Light(LightName.Light2, LightParameter.Specular, Main.threeDSettings.Specular2());
                    GL.Light(LightName.Light2, LightParameter.Position, Main.threeDSettings.Dir2());
                    GL.Light(LightName.Light2, LightParameter.SpotExponent, new float[] { 1.0f, 1.0f, 1.0f, 1.0f });
                    GL.Enable(EnableCap.Light2);
                }
                else
                {
                    GL.Disable(EnableCap.Light2);
                }

                if (Main.threeDSettings.enableLight3.Checked)
                {
                    GL.Light(LightName.Light3, LightParameter.Ambient, Main.threeDSettings.Ambient3());
                    GL.Light(LightName.Light3, LightParameter.Diffuse, Main.threeDSettings.Diffuse3());
                    GL.Light(LightName.Light3, LightParameter.Specular, Main.threeDSettings.Specular3());
                    GL.Light(LightName.Light3, LightParameter.Position, Main.threeDSettings.Dir3());
                    GL.Light(LightName.Light3, LightParameter.SpotExponent, new float[] { 1.0f, 1.0f, 1.0f, 1.0f });
                    GL.Enable(EnableCap.Light3);
                }
                else
                {
                    GL.Disable(EnableCap.Light3);
                }

                if (Main.threeDSettings.enableLight4.Checked)
                {
                    GL.Light(LightName.Light4, LightParameter.Ambient, Main.threeDSettings.Ambient4());
                    GL.Light(LightName.Light4, LightParameter.Diffuse, Main.threeDSettings.Diffuse4());
                    GL.Light(LightName.Light4, LightParameter.Specular, Main.threeDSettings.Specular4());
                    GL.Light(LightName.Light4, LightParameter.Position, Main.threeDSettings.Dir4());
                    GL.Light(LightName.Light4, LightParameter.SpotExponent, new float[] { 1.0f, 1.0f, 1.0f, 1.0f });
                    GL.Enable(EnableCap.Light4);
                }
                else
                {
                    GL.Disable(EnableCap.Light4);
                }

                GL.Enable(EnableCap.Lighting);

                //Enable Backfaceculling
                GL.Enable(EnableCap.CullFace);
                GL.Enable(EnableCap.LineSmooth);
               
                GL.Enable(EnableCap.Blend);
                GL.LineWidth(2f);
                GL.Hint(HintTarget.LineSmoothHint, HintMode.Nicest);
                GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

                // Get the printer base color
                Color col = Main.threeDSettings.printerBase.BackColor;             

                // Rotate, and translate the model view based on what the user has done with the mouse and other inputs. 
                GL.Rotate(RepetierHost.view.ThreeDView.rotX, 1, 0, 0);
                GL.Rotate(RepetierHost.view.ThreeDView.rotZ, 0, 0, 1);
                GL.Translate(-ps.BedLeft - (ps.PrintAreaWidth * 0.5f),
                    -ps.BedFront - (ps.PrintAreaDepth * 0.5f),
                    -0.5f * ps.PrintAreaHeight);

                GL.GetFloat(GetPName.ModelviewMatrix, out RepetierHost.view.ThreeDView.modelView);
                GL.Material(
                    MaterialFace.Front,
                    MaterialParameter.Specular,
                    new OpenTK.Graphics.Color4(255, 255, 255, 255));

                // Drawing background skybox would probably be ok here. 

                float dx1 = ps.DumpAreaLeft;
                float dx2 = dx1 + ps.DumpAreaWidth;
                float dy1 = ps.DumpAreaFront;
                float dy2 = dy1 + ps.DumpAreaDepth;
                if (Main.threeDSettings.showPrintbed.Checked)
                {
                    col = Main.threeDSettings.printerBase.BackColor;
                    GL.Material(MaterialFace.FrontAndBack, MaterialParameter.AmbientAndDiffuse, new OpenTK.Graphics.Color4(0, 0, 0, 255));
                    GL.Material(MaterialFace.Front, MaterialParameter.Emission, new OpenTK.Graphics.Color4(0, 0, 0, 0));
                    GL.Material(MaterialFace.Front, MaterialParameter.Specular, new float[] { 0.0f, 0.0f, 0.0f, 1.0f });
                    GL.Material(
                        MaterialFace.Front,
                        MaterialParameter.Emission,
                        new OpenTK.Graphics.Color4(col.R, col.G, col.B, col.A));
                    int i;

                    // Draw origin
                    GL.Disable(EnableCap.CullFace);
                    GL.Begin(BeginMode.Triangles);
                    GL.Normal3(0, 0, 1);
                    double delta = Math.PI / 8;
                    double rad = 2.5;
                    for (i = 0; i < 16; i++)
                    {
                        GL.Vertex3(0, 0, 0);
                        GL.Vertex3(rad * Math.Sin(i * delta), rad * Math.Cos(i * delta), 0);
                        GL.Vertex3(rad * Math.Sin((i + 1) * delta), rad * Math.Cos((i + 1) * delta), 0);
                    }

                    GL.End();
                    GL.Begin(BeginMode.Lines);
                    if (ps.printerType < 2)
                    {
                        // Print cube
                        GL.Vertex3(ps.BedLeft, ps.BedFront, 0);
                        GL.Vertex3(ps.BedLeft, ps.BedFront, ps.PrintAreaHeight);
                        GL.Vertex3(ps.BedLeft + ps.PrintAreaWidth, ps.BedFront, 0);
                        GL.Vertex3(ps.BedLeft + ps.PrintAreaWidth, ps.BedFront, ps.PrintAreaHeight);
                        GL.Vertex3(ps.BedLeft, ps.BedFront + ps.PrintAreaDepth, 0);
                        GL.Vertex3(ps.BedLeft, ps.BedFront + ps.PrintAreaDepth, ps.PrintAreaHeight);
                        GL.Vertex3(ps.BedLeft + ps.PrintAreaWidth, ps.BedFront + ps.PrintAreaDepth, 0);
                        GL.Vertex3(ps.BedLeft + ps.PrintAreaWidth, ps.BedFront + ps.PrintAreaDepth, ps.PrintAreaHeight);
                        GL.Vertex3(ps.BedLeft, ps.BedFront, ps.PrintAreaHeight);
                        GL.Vertex3(ps.BedLeft + ps.PrintAreaWidth, ps.BedFront, ps.PrintAreaHeight);
                        GL.Vertex3(ps.BedLeft + ps.PrintAreaWidth, ps.BedFront, ps.PrintAreaHeight);
                        GL.Vertex3(ps.BedLeft + ps.PrintAreaWidth, ps.BedFront + ps.PrintAreaDepth, ps.PrintAreaHeight);
                        GL.Vertex3(ps.BedLeft + ps.PrintAreaWidth, ps.BedFront + ps.PrintAreaDepth, ps.PrintAreaHeight);
                        GL.Vertex3(ps.BedLeft, ps.BedFront + ps.PrintAreaDepth, ps.PrintAreaHeight);
                        GL.Vertex3(ps.BedLeft, ps.BedFront + ps.PrintAreaDepth, ps.PrintAreaHeight);
                        GL.Vertex3(ps.BedLeft, ps.BedFront, ps.PrintAreaHeight);
                        if (ps.printerType == 1)
                        {
                            if (dy1 != 0)
                            {
                                GL.Vertex3(ps.BedLeft + dx1, ps.BedFront + dy1, 0);
                                GL.Vertex3(ps.BedLeft + dx2, ps.BedFront + dy1, 0);
                            }

                            GL.Vertex3(ps.BedLeft + dx2, ps.BedFront + dy1, 0);
                            GL.Vertex3(ps.BedLeft + dx2, ps.BedFront + dy2, 0);
                            GL.Vertex3(ps.BedLeft + dx2, ps.BedFront + dy2, 0);
                            GL.Vertex3(ps.BedLeft + dx1, ps.BedFront + dy2, 0);
                            GL.Vertex3(ps.BedLeft + dx1, ps.BedFront + dy2, 0);
                            GL.Vertex3(ps.BedLeft + dx1, ps.BedFront + dy1, 0);
                        }

                        float dx = 10; // ps.PrintAreaWidth / 20f;
                        float dy = 10; // ps.PrintAreaDepth / 20f;
                        float x, y;
                        for (i = 0; i < 200; i++)
                        {
                            x = (float)i * dx;
                            if (x >= ps.PrintAreaWidth)
                            {
                                x = ps.PrintAreaWidth;
                            }

                            if (ps.printerType == 1 && x >= dx1 && x <= dx2)
                            {
                                GL.Vertex3(ps.BedLeft + x, ps.BedFront, 0);
                                GL.Vertex3(ps.BedLeft + x, ps.BedFront + dy1, 0);
                                GL.Vertex3(ps.BedLeft + x, ps.BedFront + dy2, 0);
                                GL.Vertex3(ps.BedLeft + x, ps.BedFront + ps.PrintAreaDepth, 0);
                            }
                            else
                            {
                                GL.Vertex3(ps.BedLeft + x, ps.BedFront, 0);
                                GL.Vertex3(ps.BedLeft + x, ps.BedFront + ps.PrintAreaDepth, 0);
                            }

                            if (x >= ps.PrintAreaWidth)
                            {
                                break;
                            }
                        }

                        for (i = 0; i < 200; i++)
                        {
                            y = (float)i * dy;
                            if (y > ps.PrintAreaDepth)
                            {
                                y = ps.PrintAreaDepth;
                            }

                            if (ps.printerType == 1 && y >= dy1 && y <= dy2)
                            {
                                GL.Vertex3(ps.BedLeft, ps.BedFront + y, 0);
                                GL.Vertex3(ps.BedLeft + dx1, ps.BedFront + y, 0);
                                GL.Vertex3(ps.BedLeft + dx2, ps.BedFront + y, 0);
                                GL.Vertex3(ps.BedLeft + ps.PrintAreaWidth, ps.BedFront + y, 0);
                            }
                            else
                            {
                                GL.Vertex3(ps.BedLeft, ps.BedFront + y, 0);
                                GL.Vertex3(ps.BedLeft + ps.PrintAreaWidth, ps.BedFront + y, 0);
                            }

                            if (y >= ps.PrintAreaDepth)
                            {
                                break;
                            }
                        }
                    }
                    else if (ps.printerType == 2) // Cylinder shape
                    {
                        int ncirc = 32;
                        int vertexevery = 4;
                        delta = (float)(Math.PI * 2 / ncirc);
                        float alpha = 0;
                        for (i = 0; i < ncirc; i++)
                        {
                            float alpha2 = (float)(alpha + delta);
                            float x1 = (float)(ps.rostockRadius * Math.Sin(alpha));
                            float y1 = (float)(ps.rostockRadius * Math.Cos(alpha));
                            float x2 = (float)(ps.rostockRadius * Math.Sin(alpha2));
                            float y2 = (float)(ps.rostockRadius * Math.Cos(alpha2));
                            GL.Vertex3(x1, y1, 0);
                            GL.Vertex3(x2, y2, 0);
                            GL.Vertex3(x1, y1, ps.rostockHeight);
                            GL.Vertex3(x2, y2, ps.rostockHeight);
                            if ((i % vertexevery) == 0)
                            {
                                GL.Vertex3(x1, y1, 0);
                                GL.Vertex3(x1, y1, ps.rostockHeight);
                            }

                            alpha = alpha2;
                        }

                        delta = 10;
                        float x = (float)(Math.Floor(ps.rostockRadius / delta) * delta);
                        while (x > -ps.rostockRadius)
                        {
                            alpha = (float)Math.Acos(x / ps.rostockRadius);
                            float y = (float)(ps.rostockRadius * Math.Sin(alpha));
                            GL.Vertex3(x, -y, 0);
                            GL.Vertex3(x, y, 0);
                            GL.Vertex3(y, x, 0);
                            GL.Vertex3(-y, x, 0);
                            x -= (float)delta;
                        }
                    }
                    else if (ps.printerType == 3)
                    {
                        float dx = 10; // ps.PrintAreaWidth / 20f;
                        float dy = 10; // ps.PrintAreaDepth / 20f;
                        float x, y;
                        for (i = 0; i < 200; i++)
                        {
                            x = (float)i * dx;
                            if (x >= ps.PrintAreaWidth)
                            {
                                x = ps.PrintAreaWidth;
                            }

                            if (ps.printerType == 1 && x >= dx1 && x <= dx2)
                            {
                                GL.Vertex3(ps.BedLeft + x, ps.BedFront, 0);
                                GL.Vertex3(ps.BedLeft + x, ps.BedFront + dy1, 0);
                                GL.Vertex3(ps.BedLeft + x, ps.BedFront + dy2, 0);
                                GL.Vertex3(ps.BedLeft + x, ps.BedFront + ps.PrintAreaDepth, 0);
                            }
                            else
                            {
                                GL.Vertex3(ps.BedLeft + x, ps.BedFront, 0);
                                GL.Vertex3(ps.BedLeft + x, ps.BedFront + ps.PrintAreaDepth, 0);
                            }

                            if (x >= ps.PrintAreaWidth)
                            {
                                break;
                            }
                        }

                        for (i = 0; i < 200; i++)
                        {
                            y = (float)i * dy;
                            if (y > ps.PrintAreaDepth)
                            {
                                y = ps.PrintAreaDepth;
                            }

                            if (ps.printerType == 1 && y >= dy1 && y <= dy2)
                            {
                                GL.Vertex3(ps.BedLeft, ps.BedFront + y, 0);
                                GL.Vertex3(ps.BedLeft + dx1, ps.BedFront + y, 0);
                                GL.Vertex3(ps.BedLeft + dx2, ps.BedFront + y, 0);
                                GL.Vertex3(ps.BedLeft + ps.PrintAreaWidth, ps.BedFront + y, 0);
                            }
                            else
                            {
                                GL.Vertex3(ps.BedLeft, ps.BedFront + y, 0);
                                GL.Vertex3(ps.BedLeft + ps.PrintAreaWidth, ps.BedFront + y, 0);
                            }

                            if (y >= ps.PrintAreaDepth)
                            {
                                break;
                            }
                        }
                    }

                    GL.End();
                }

                ////if (Main.main.tab.SelectedIndex > 1)
                if ((Main.main.current3Dview == Main.ThreeDViewOptions.gcode) || (Main.main.current3Dview == Main.ThreeDViewOptions.livePrinting))
                {
                    GL.Enable(EnableCap.CullFace);
                }
                else
                {
                    GL.Disable(EnableCap.CullFace);
                }

                GL.Disable(EnableCap.LineSmooth);

                // For all the models in the current view. Draw them. 
                foreach (ThreeDModel model in view.models)
                {
                    GL.PushMatrix();
                    model.AnimationBefore();
                    GL.Translate(model.Position.x, model.Position.y, model.Position.z);
                    GL.Rotate(model.Rotation.z, Vector3.UnitZ);
                    GL.Rotate(model.Rotation.y, Vector3.UnitY);
                    GL.Rotate(model.Rotation.x, Vector3.UnitX);
                    GL.Scale(model.Scale.x, model.Scale.y, model.Scale.z);

                    // Paint the model using Open
                    model.Paint();
                    model.AnimationAfter();
                    GL.PopMatrix();

                    // If the model is selected, then draw a box around it. 
                    if (model.Selected)
                    {
                        GL.PushMatrix();
                        model.AnimationBefore();
                        col = Main.threeDSettings.selectionBox.BackColor;
                        GL.Material(MaterialFace.FrontAndBack, MaterialParameter.AmbientAndDiffuse, new OpenTK.Graphics.Color4(0, 0, 0, 255));
                        GL.Material(MaterialFace.Front, MaterialParameter.Emission, new OpenTK.Graphics.Color4(0, 0, 0, 0));
                        GL.Material(MaterialFace.Front, MaterialParameter.Specular, new float[] { 0.0f, 0.0f, 0.0f, 1.0f });
                        GL.Material(
                            MaterialFace.Front,
                            MaterialParameter.Emission,
                            new OpenTK.Graphics.Color4(col.R, col.G, col.B, col.A));
                        GL.Begin(BeginMode.Lines);
                        GL.Vertex3(model.xMin, model.yMin, model.zMin);
                        GL.Vertex3(model.xMax, model.yMin, model.zMin);

                        GL.Vertex3(model.xMin, model.yMin, model.zMin);
                        GL.Vertex3(model.xMin, model.yMax, model.zMin);

                        GL.Vertex3(model.xMin, model.yMin, model.zMin);
                        GL.Vertex3(model.xMin, model.yMin, model.zMax);

                        GL.Vertex3(model.xMax, model.yMax, model.zMax);
                        GL.Vertex3(model.xMin, model.yMax, model.zMax);

                        GL.Vertex3(model.xMax, model.yMax, model.zMax);
                        GL.Vertex3(model.xMax, model.yMin, model.zMax);

                        GL.Vertex3(model.xMax, model.yMax, model.zMax);
                        GL.Vertex3(model.xMax, model.yMax, model.zMin);

                        GL.Vertex3(model.xMin, model.yMax, model.zMax);
                        GL.Vertex3(model.xMin, model.yMax, model.zMin);

                        GL.Vertex3(model.xMin, model.yMax, model.zMax);
                        GL.Vertex3(model.xMin, model.yMin, model.zMax);

                        GL.Vertex3(model.xMax, model.yMax, model.zMin);
                        GL.Vertex3(model.xMax, model.yMin, model.zMin);

                        GL.Vertex3(model.xMax, model.yMax, model.zMin);
                        GL.Vertex3(model.xMin, model.yMax, model.zMin);

                        GL.Vertex3(model.xMax, model.yMin, model.zMax);
                        GL.Vertex3(model.xMin, model.yMin, model.zMax);

                        GL.Vertex3(model.xMax, model.yMin, model.zMax);
                        GL.Vertex3(model.xMax, model.yMin, model.zMin);

                        GL.End();
                        model.AnimationAfter();
                        GL.PopMatrix();
                    }
                } // End loop of models.
              
                if (Main.threeDSettings.showPrintbed.Checked)
                {
                    GL.Disable(EnableCap.CullFace);
                    GL.Enable(EnableCap.Blend);	// Turn Blending On
                    GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
                   
                    // Draw bottom
                    col = Main.threeDSettings.printerBase.BackColor;
                    float[] transblack = new float[] { 0, 0, 0, 0 };
                    GL.Material(MaterialFace.FrontAndBack, MaterialParameter.AmbientAndDiffuse, new OpenTK.Graphics.Color4(col.R, col.G, col.B, 130));
                    GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Specular, transblack);
                    GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Emission, transblack);
                    GL.PushMatrix();
                    GL.Translate(0, 0, -0.04);
                    if (ps.printerType < 2 || ps.printerType == 3)
                    {
                        GL.Begin(BeginMode.Quads);
                        GL.Normal3(0, 0, 1);

                        if (ps.printerType == 1)
                        {
                            if (dy1 > 0)
                            {
                                GL.Vertex3(ps.BedLeft, ps.BedFront, 0);
                                GL.Vertex3(ps.BedLeft + ps.PrintAreaWidth, ps.BedFront, 0);
                                GL.Vertex3(ps.BedLeft + ps.PrintAreaWidth, ps.BedFront + dy1, 0);
                                GL.Vertex3(ps.BedLeft, ps.BedFront + dy1, 0);
                            }

                            if (dy2 < ps.PrintAreaDepth)
                            {
                                GL.Vertex3(ps.BedLeft, ps.BedFront + dy2, 0);
                                GL.Vertex3(ps.BedLeft + ps.PrintAreaWidth, ps.BedFront + dy2, 0);
                                GL.Vertex3(ps.BedLeft + ps.PrintAreaWidth, ps.BedFront + ps.PrintAreaDepth, 0);
                                GL.Vertex3(ps.BedLeft, ps.BedFront + ps.PrintAreaDepth, 0);
                            }

                            if (dx1 > 0)
                            {
                                GL.Vertex3(ps.BedLeft, ps.BedFront + dy1, 0);
                                GL.Vertex3(ps.BedLeft + dx1, ps.BedFront + dy1, 0);
                                GL.Vertex3(ps.BedLeft + dx1, ps.BedFront + dy2, 0);
                                GL.Vertex3(ps.BedLeft, ps.BedFront + dy2, 0);
                            }

                            if (dx2 < ps.PrintAreaWidth)
                            {
                                GL.Vertex3(ps.BedLeft + dx2, ps.BedFront + dy1, 0);
                                GL.Vertex3(ps.BedLeft + ps.PrintAreaWidth, ps.BedFront + dy1, 0);
                                GL.Vertex3(ps.BedLeft + ps.PrintAreaWidth, ps.BedFront + dy2, 0);
                                GL.Vertex3(ps.BedLeft + dx2, ps.BedFront + dy2, 0);
                            }
                        }
                        else
                        {
                            GL.Vertex3(ps.BedLeft, ps.BedFront, 0);
                            GL.Vertex3(ps.BedLeft + ps.PrintAreaWidth, ps.BedFront, 0);
                            GL.Vertex3(ps.BedLeft + ps.PrintAreaWidth, ps.BedFront + ps.PrintAreaDepth, 0);
                            GL.Vertex3(ps.BedLeft + 0, ps.BedFront + ps.PrintAreaDepth, 0);
                        }

                        GL.End();
                    }
                    else if (ps.printerType == 2)
                    {
                        int ncirc = 32;
                        float delta = (float)(Math.PI * 2 / ncirc);
                        float alpha = 0;
                        GL.Begin(BeginMode.Quads);
                        GL.Normal3(0, 0, 1);
                        for (int i = 0; i < ncirc / 4; i++)
                        {
                            float alpha2 = (float)(alpha + delta);
                            float x1 = (float)(ps.rostockRadius * Math.Sin(alpha));
                            float y1 = (float)(ps.rostockRadius * Math.Cos(alpha));
                            float x2 = (float)(ps.rostockRadius * Math.Sin(alpha2));
                            float y2 = (float)(ps.rostockRadius * Math.Cos(alpha2));
                            GL.Vertex3(x1, y1, 0);
                            GL.Vertex3(x2, y2, 0);
                            GL.Vertex3(-x2, y2, 0);
                            GL.Vertex3(-x1, y1, 0);
                            GL.Vertex3(x1, -y1, 0);
                            GL.Vertex3(x2, -y2, 0);
                            GL.Vertex3(-x2, -y2, 0);
                            GL.Vertex3(-x1, -y1, 0);
                            alpha = alpha2;
                        }

                        GL.End();
                    }

                    GL.PopMatrix();
                    GL.Disable(EnableCap.Blend);
                } // End show the print bed. 

                gl.SwapBuffers();  // Show the scene on the screen. 
                fpsTimer.Stop();  // Stop the stopwatch timer. 

                double framerate = 1.0 / fpsTimer.Elapsed.TotalSeconds;
                Main.main.fpsLabel.Text = framerate.ToString("0") + " FPS";
                if ((framerate < 30) && (Main.globalSettings.DisableQualityReduction == false))
                {
                    slowCounter++;
                    if (slowCounter >= 10)
                    {
                        slowCounter = 0;
                        foreach (ThreeDModel model in view.models)
                        {
                            model.ReduceQuality();
                        }
                    }
                }
                else if (slowCounter > 0)
                {
                    slowCounter--;
                }
            }
            catch 
            {
            }
        }

        /// <summary>
        /// Sets up the projection matrix for the 3D view in OpenTK. The projection matrix is dependent on if we are in in orthogonal 3d mode. 
        /// </summary>
        private void SetupProjectionMatrix()
        {
            try
            {
                int w = gl.Width;
                int h = gl.Height;
                GL.MatrixMode(MatrixMode.Projection);

                float dx = RepetierHost.view.ThreeDView.viewCenter.X - RepetierHost.view.ThreeDView.userPosition.X;
                float dy = RepetierHost.view.ThreeDView.viewCenter.Y - RepetierHost.view.ThreeDView.userPosition.Y;
                float dz = RepetierHost.view.ThreeDView.viewCenter.Z - RepetierHost.view.ThreeDView.userPosition.Z;
                float dist = (float)Math.Sqrt((dx * dx) + (dy * dy) + (dz * dz));
                RepetierHost.view.ThreeDView.nearHeight = 2.0f * (float)(Math.Tan((RepetierHost.view.ThreeDView.zoom * 15f) * (Math.PI / 180f)) * RepetierHost.view.ThreeDView.nearDist);
                RepetierHost.view.ThreeDView.aspectRatio = (float)w / (float)h;
                RepetierHost.view.ThreeDView.nearDist = Math.Max(10, dist - (2f * ps.PrintAreaDepth));
                RepetierHost.view.ThreeDView.farDist = dist + (2 * ps.PrintAreaDepth);
                if (perspectiveModeisParrallel == true)
                {
                    RepetierHost.view.ThreeDView.persp = Matrix4.CreateOrthographic(
                        4.0f * RepetierHost.view.ThreeDView.nearHeight * RepetierHost.view.ThreeDView.aspectRatio,
                        4.0f * RepetierHost.view.ThreeDView.nearHeight,
                        RepetierHost.view.ThreeDView.nearDist,
                        RepetierHost.view.ThreeDView.farDist);
                }
                else
                {
                    RepetierHost.view.ThreeDView.persp = Matrix4.CreatePerspectiveFieldOfView((float)(RepetierHost.view.ThreeDView.zoom * 30f * Math.PI / 180f),
                        RepetierHost.view.ThreeDView.aspectRatio,
                        RepetierHost.view.ThreeDView.nearDist,
                        RepetierHost.view.ThreeDView.farDist);
                }

                GL.LoadMatrix(ref RepetierHost.view.ThreeDView.persp);
            }
            catch
            {
            }
        }

        /// <summary>
        /// A simple test to play with 3d rendering type things. Not in use right now. 
        /// </summary>
        /// <returns>Always false</returns>
        private bool SimpleTest()
        {
            gl.MakeCurrent();

            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);

            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, Width / (float)Height, 1.0f, 64.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.PushMatrix();
            GL.LoadMatrix(ref projection);

            //--------------------------------
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            Matrix4 modelview = Matrix4.LookAt(Vector3.Zero, Vector3.UnitZ, Vector3.UnitY);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            GL.LoadMatrix(ref modelview);

            GL.Begin(BeginMode.Triangles);

            GL.Color3(1.0f, 1.0f, 0.0f); GL.Vertex3(-1.0f, -1.0f, 4.0f);
            GL.Color3(1.0f, 0.0f, 0.0f); GL.Vertex3(1.0f, -1.0f, 4.0f);
            GL.Color3(0.2f, 0.9f, 1.0f); GL.Vertex3(0.0f, 1.0f, 4.0f);

            GL.End();
            GL.PopMatrix();
            GL.PopMatrix();
            
            return false;
        }

        /// <summary>
        /// Draw the background gradient before you draw the 3D text.   
        /// </summary>
        private void DrawBackGroundGradient()
        {
            //// http://stackoverflow.com/questions/468297/in-opengl-how-do-i-make-a-simple-background-quad
            // http://stackoverflow.com/questions/5467218/opengl-2d-hud-over-3d

            // Disable things setup in 3D mode
            GL.Disable(EnableCap.DepthTest); //glDisable(GL_DEPTH_TEST);
            GL.Disable(EnableCap.CullFace);//glDisable(GL_CULL_FACE);
            GL.Disable(EnableCap.Texture2D);//glDisable(GL_TEXTURE_2D);
            GL.Disable(EnableCap.Lighting);//glDisable(GL_LIGHTING);    

            // Swith to projection mode so that we can change to orthogonal view. 
            GL.MatrixMode(MatrixMode.Projection); //glMatrixMode(GL_PROJECTION);
            GL.LoadIdentity(); //glLoadIdentity();

            // Set the orthogonal view to the width and height of the controller. 
            int w = gl.Width;   // //int w = glutGet(GLUT_WINDOW_WIDTH);
            int h = gl.Height;   //int h = glutGet(GLUT_WINDOW_HEIGHT);
            GL.Ortho(0, w, 0, h, -1.0f, 1.0f); //gluOrtho2D(0, w, h, 0);

            // Swith to model view so that we can draw the quad
            GL.MatrixMode(MatrixMode.Modelview); //glMatrixMode(GL_MODELVIEW);          
            GL.LoadIdentity(); //glLoadIdentity();   

            // Draw the quad, specify color for each vertex
            GL.Begin(BeginMode.Quads);
            GL.Color3(Color.LightSkyBlue);
            GL.Vertex2(0, 0);
            GL.Color3(Color.WhiteSmoke);
            GL.Vertex2(0, h);
            GL.Color3(Color.White);
            GL.Vertex2(w, h);
            GL.Color3(Color.LightBlue);
            GL.Vertex2(w, 0);
            GL.End();

            
        }

        /// <summary>
        /// Runs the when the Form is loaded to setup some information related to OpenGL. Prints some information to the log about what version we are running of OpenGL.
        /// </summary>
        private static bool configureSettings = true;

        /// <summary>
        /// Event called on loading the form. Sends some basic information about opengl to the log and other things. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ThreeDControl_Load(object sender, EventArgs e)
        {
            if (configureSettings)
            {
                try
                {
                    Main.connection.log("OpenGL version:" + GL.GetString(StringName.Version), false, 3);
                    Main.connection.log("OpenGL extensions:" + GL.GetString(StringName.Extensions), false, 3);
                    Main.connection.log("OpenGL renderer:" + GL.GetString(StringName.Renderer), false, 3);
                    string sv = GL.GetString(StringName.Version).Trim();
                    int p = sv.IndexOf(" ");
                    if (p > 0)
                    {
                        sv = sv.Substring(0, p);
                    }

                    p = sv.IndexOf('.');
                    if (p > 0)
                    {
                        p = sv.IndexOf('.', p + 1);
                        if (p > 0)
                        {
                            sv = sv.Substring(0, p);
                        }
                    }

                    try
                    {
                        float val = 0;
                        float.TryParse(sv, NumberStyles.Float, GCode.format, out val);
                        Main.threeDSettings.openGLVersion = val;
                    }
                    catch
                    {
                        Main.threeDSettings.openGLVersion = 1.1f;
                    }

                    string extensions = GL.GetString(StringName.Extensions);
                    Main.threeDSettings.useVBOs = false;
                    foreach (string s in extensions.Split(' '))
                    {
                        if (s.Equals("GL_ARB_vertex_buffer_object")/* && Main.threeDSettings.openGLVersion>1.49*/)
                        {
                            Main.threeDSettings.useVBOs = true;
                        }
                    }

                    if (Main.threeDSettings.useVBOs)
                    {
                        Main.connection.log("Using fast VBOs for rendering is possible", false, 3);
                    }
                    else
                    {
                        Main.connection.log("Fast VBOs for rendering not supported. Using slower default method.", false, 3);
                    }
                }
                catch
                {
                }

                configureSettings = false;
            }

            loaded = true;
            SetupViewport();


            modeText = new MyTextWriter(new Size(gl.Width, gl.Height), new Size(gl.Width, gl.Height));
            modeText.AddLine("Developer mode, 开发模式", new PointF(gl.Width / 2, gl.Height / 4), new SolidBrush(Color.Orange));
           // modeText.AddLine("开发模式", new PointF(gl.Width / 2, gl.Height*3/4), new SolidBrush(Color.Black));
            //modeText.Update(0, "current mode");
        }

        /// <summary>
        /// Helps with picking a object when click on the screen??
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="viewport"></param>
        /// <returns></returns>
        private Matrix4 GluPickMatrix(float x, float y, float width, float height, int[] viewport)
        {
            Matrix4 result = Matrix4.Identity;
            if ((width <= 0.0f) || (height <= 0.0f))
            {
                return result;
            }

            float translateX = (viewport[2] - (2.0f * (x - viewport[0]))) / width;
            float translateY = (viewport[3] - (2.0f * (y - viewport[1]))) / height;
            result = Matrix4.Mult(Matrix4.CreateTranslation(translateX, translateY, 0.0f), result);
            float scaleX = viewport[2] / width;
            float scaleY = viewport[3] / height;
            result = Matrix4.Mult(Matrix4.Scale(scaleX, scaleY, 1.0f), result);
            return result;
        }

        private uint lastDepth = 0;

        private Geom3DLine pickLine = null; // Last pick up line ray

        private Geom3DLine viewLine = null; // Direction of view

        private Geom3DVector pickPoint = new Geom3DVector(0, 0, 0); // Koordinates of last pick

        /// <summary>
        /// Updates the pick line text which is related to determining where in 3d space a 2d mouse click should pick. 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void UpdatePickLine(int x, int y)
        {
            if (view == null)
            {
                return;
            }

            // Intersection on bottom plane
            int window_y = (Height - y) - (Height / 2);
            double norm_y = (double)window_y / (double)(Height / 2);
            int window_x = x - (Width / 2);
            double norm_x = (double)window_x / (double)(Width / 2);
            float fpy = (float)(RepetierHost.view.ThreeDView.nearHeight * (0.5 * norm_y)) * (perspectiveModeisParrallel == true ? 4f : 1f);
            float fpx = (float)(RepetierHost.view.ThreeDView.nearHeight * (0.5 * (RepetierHost.view.ThreeDView.aspectRatio * norm_x))) * (perspectiveModeisParrallel == true ? 4f : 1f);

            Vector4 frontPointN = perspectiveModeisParrallel == true ? new Vector4(fpx, fpy, 0, 1) : new Vector4(0, 0, 0, 1);
            Vector4 dirN = perspectiveModeisParrallel == true ? new Vector4(0, 0, -RepetierHost.view.ThreeDView.nearDist, 0) : new Vector4(fpx, fpy, -RepetierHost.view.ThreeDView.nearDist, 0);
            Matrix4 rotx = Matrix4.CreateFromAxisAngle(new Vector3(1, 0, 0), (float)(RepetierHost.view.ThreeDView.rotX * Math.PI / 180.0));
            Matrix4 rotz = Matrix4.CreateFromAxisAngle(new Vector3(0, 0, 1), (float)(RepetierHost.view.ThreeDView.rotZ * Math.PI / 180.0));
            Matrix4 trans = Matrix4.CreateTranslation(-ps.BedLeft - (ps.PrintAreaWidth * 0.5f), -ps.BedFront - (ps.PrintAreaDepth * 0.5f), -0.5f * ps.PrintAreaHeight);
            Matrix4 ntrans = Matrix4.LookAt(RepetierHost.view.ThreeDView.userPosition.X,
                RepetierHost.view.ThreeDView.userPosition.Y,
                RepetierHost.view.ThreeDView.userPosition.Z,
                RepetierHost.view.ThreeDView.viewCenter.X,
                RepetierHost.view.ThreeDView.viewCenter.Y,
                RepetierHost.view.ThreeDView.viewCenter.Z, 0, 0, 1.0f);

            ntrans = Matrix4.Mult(rotx, ntrans);
            ntrans = Matrix4.Mult(rotz, ntrans);
            ntrans = Matrix4.Mult(trans, ntrans);
            ntrans = Matrix4.Invert(ntrans);
            Vector4 frontPoint = perspectiveModeisParrallel == true ? Vector4.Transform(frontPointN, ntrans) : ntrans.Row3;
            Vector4 dirVec = Vector4.Transform(dirN, ntrans);
            pickLine = new Geom3DLine(new Geom3DVector(frontPoint.X / frontPoint.W, frontPoint.Y / frontPoint.W, frontPoint.Z / frontPoint.W),
                new Geom3DVector(dirVec.X, dirVec.Y, dirVec.Z), true);
            pickLine.dir.normalize();
         }

        /// <summary>
        /// Preforms the pick text determining where in 3d space a 2d mouse click should pick. I don't think this works right now. 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private ThreeDModel Picktest(int x, int y)
        {
            if (view == null)
            {
                return null;
            }
           
            gl.MakeCurrent();
            uint[] selectBuffer = new uint[128];
            GL.SelectBuffer(128, selectBuffer);
            GL.RenderMode(RenderingMode.Select);
            SetupViewport();

            GL.MatrixMode(MatrixMode.Projection);
            GL.PushMatrix();
            GL.LoadIdentity();

            int[] viewport = new int[4];
            GL.GetInteger(GetPName.Viewport, viewport);

            Matrix4 m = GluPickMatrix(x, viewport[3] - y, 1, 1, viewport);
            GL.MultMatrix(ref m);

           GL.MultMatrix(ref RepetierHost.view.ThreeDView.persp);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.ClearColor(Main.threeDSettings.background.BackColor);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);
            RepetierHost.view.ThreeDView.lookAt = Matrix4.LookAt(RepetierHost.view.ThreeDView.userPosition.X,
                RepetierHost.view.ThreeDView.userPosition.Y,
                RepetierHost.view.ThreeDView.userPosition.Z,
                RepetierHost.view.ThreeDView.viewCenter.X,
                RepetierHost.view.ThreeDView.viewCenter.Y,
                RepetierHost.view.ThreeDView.viewCenter.Z, 0, 0, 1.0f);

            // Intersection on bottom plane
            int window_y = (viewport[3] - y) - (viewport[3] / 2);
            double norm_y = (double)window_y / (double)(viewport[3] / 2);
            int window_x = x - (viewport[2] / 2);
            double norm_x = (double)window_x / (double)(viewport[2] / 2);
            float fpy = (float)(RepetierHost.view.ThreeDView.nearHeight * (0.5 * norm_y)) * (perspectiveModeisParrallel == true ? 4f : 1f);
            float fpx = (float)(RepetierHost.view.ThreeDView.nearHeight * (0.5 * (RepetierHost.view.ThreeDView.aspectRatio * norm_x))) * (perspectiveModeisParrallel == true ? 4f : 1f);

            Vector4 frontPointN = perspectiveModeisParrallel == true ? new Vector4(fpx, fpy, 0, 1) : new Vector4(0, 0, 0, 1);
            Vector4 dirN = perspectiveModeisParrallel == true ? new Vector4(0, 0, -RepetierHost.view.ThreeDView.nearDist, 0) : new Vector4(fpx, fpy, -RepetierHost.view.ThreeDView.nearDist, 0);
            Matrix4 rotx = Matrix4.CreateFromAxisAngle(new Vector3(1, 0, 0), (float)(RepetierHost.view.ThreeDView.rotX * Math.PI / 180.0));
            Matrix4 rotz = Matrix4.CreateFromAxisAngle(new Vector3(0, 0, 1), (float)(RepetierHost.view.ThreeDView.rotZ * Math.PI / 180.0));
            Matrix4 trans = Matrix4.CreateTranslation(-ps.BedLeft - (ps.PrintAreaWidth * 0.5f), -ps.BedFront - (ps.PrintAreaDepth * 0.5f), -0.5f * ps.PrintAreaHeight);
            Matrix4 ntrans = RepetierHost.view.ThreeDView.lookAt;
            ntrans = Matrix4.Mult(rotx, ntrans);
            ntrans = Matrix4.Mult(rotz, ntrans);
            ntrans = Matrix4.Mult(trans, ntrans);
            ntrans = Matrix4.Invert(ntrans);
            Vector4 frontPoint = perspectiveModeisParrallel == true ? Vector4.Transform(frontPointN, ntrans) : ntrans.Row3;
            Vector4 dirVec = Vector4.Transform(dirN, ntrans);
            pickLine = new Geom3DLine(new Geom3DVector(frontPoint.X / frontPoint.W, frontPoint.Y / frontPoint.W, frontPoint.Z / frontPoint.W),
                new Geom3DVector(dirVec.X, dirVec.Y, dirVec.Z), true);
            dirN = new Vector4(0, 0, -RepetierHost.view.ThreeDView.nearDist, 0);
            dirVec = Vector4.Transform(dirN, ntrans);
            viewLine = new Geom3DLine(new Geom3DVector(frontPoint.X / frontPoint.W, frontPoint.Y / frontPoint.W, frontPoint.Z / frontPoint.W),
                new Geom3DVector(dirVec.X, dirVec.Y, dirVec.Z), true);
            viewLine.dir.normalize();
            pickLine.dir.normalize();
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref RepetierHost.view.ThreeDView.lookAt);
            GL.Rotate(RepetierHost.view.ThreeDView.rotX, 1, 0, 0);
            GL.Rotate(RepetierHost.view.ThreeDView.rotZ, 0, 0, 1);
            GL.Translate(-ps.BedLeft - (ps.PrintAreaWidth * 0.5f), -ps.BedFront - (ps.PrintAreaDepth * 0.5f), -0.5f * ps.PrintAreaHeight);

            GL.InitNames();
            int pos = 0;
            foreach (ThreeDModel model in view.models)
            {
                GL.PushName(pos++);
                GL.PushMatrix();
                model.AnimationBefore();
                GL.Translate(model.Position.x, model.Position.y, model.Position.z);
                GL.Rotate(model.Rotation.z, Vector3.UnitZ);
                GL.Rotate(model.Rotation.y, Vector3.UnitY);
                GL.Rotate(model.Rotation.x, Vector3.UnitX);
                GL.Scale(model.Scale.x, model.Scale.y, model.Scale.z);
                model.Paint();
                model.AnimationAfter();
                GL.PopMatrix();
                GL.PopName();
            }

            GL.MatrixMode(MatrixMode.Projection);
            GL.PopMatrix();
            GL.MatrixMode(MatrixMode.Modelview);

            int hits = GL.RenderMode(RenderingMode.Render);
            ThreeDModel selected = null;
            if (hits > 0)
            {
                selected = view.models.ElementAt((int)selectBuffer[3]);
                lastDepth = selectBuffer[1];
                for (int i = 1; i < hits; i++)
                {
                    if (selectBuffer[(4 * i) + 1] < lastDepth)
                    {
                        lastDepth = selectBuffer[(i * 4) + 1];
                        selected = view.models.ElementAt((int)selectBuffer[(i * 4) + 3]);
                    }
                }

                double dfac = (double)lastDepth / uint.MaxValue;
                dfac = -(RepetierHost.view.ThreeDView.farDist * RepetierHost.view.ThreeDView.nearDist) / ((dfac * (RepetierHost.view.ThreeDView.farDist - RepetierHost.view.ThreeDView.nearDist)) - RepetierHost.view.ThreeDView.farDist);
                Geom3DVector crossPlanePoint = new Geom3DVector(viewLine.dir).scale((float)dfac).add(viewLine.point);
                Geom3DPlane objplane = new Geom3DPlane(crossPlanePoint, viewLine.dir);
                objplane.intersectLine(pickLine, pickPoint);                
            }
            
            return selected;
        }

        /// <summary>
        /// Resize event. Redraw the viewport so that everything fits in the new size view. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gl_Resize(object sender, EventArgs e)
        {
            SetupViewport();
            gl.Invalidate();
        }

        Geom3DPlane movePlane = new Geom3DPlane(new Geom3DVector(0, 0, 0), new Geom3DVector(0, 0, 1)); // Plane where object movement occurs

        Geom3DVector moveStart, moveLast, movePos;

        /// <summary>
        /// Mouse down event. I don't think the pick test stuff is working right now. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gl_MouseDown(object sender, MouseEventArgs e)
        {
            lastX = xDown = e.X;
            lastY = yDown = e.Y;
            startRotX = RepetierHost.view.ThreeDView.rotX;
            startRotZ = RepetierHost.view.ThreeDView.rotZ;
            startViewCenter = RepetierHost.view.ThreeDView.viewCenter;
            startUserPosition = RepetierHost.view.ThreeDView.userPosition;
            movePlane = new Geom3DPlane(new Geom3DVector(0, 0, 0), new Geom3DVector(0, 0, 1));
            moveStart = moveLast = new Geom3DVector(0, 0, 0);
            UpdatePickLine(e.X, e.Y);
            movePlane.intersectLine(pickLine, moveStart);
            if (e.Button == MouseButtons.Right)
            {
                ThreeDModel sel = Picktest(e.X, e.Y);
                if (sel != null)
                {
                    movePlane = new Geom3DPlane(pickPoint, new Geom3DVector(0, 0, 1));
                    moveStart = moveLast = new Geom3DVector(pickPoint);
                }

                if (sel != null && view.eventObjectMoved != null)
                {
                    view.eventObjectSelected(sel);
                }
            }
        }

        /// <summary>
        /// Mouse moves  event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gl_MouseMove(object sender, MouseEventArgs e)
        {
            double window_y = (gl.Height - e.Y) - (gl.Height / 2);
            double window_x = e.X - (gl.Width / 2);
            if (e.Button == MouseButtons.None)
            {
                speedX = speedY = 0;
                return;
            }

            xPos = e.X;
            yPos = e.Y;
            UpdatePickLine(e.X, e.Y);
            movePos = new Geom3DVector(0, 0, 0);
            movePlane.intersectLine(pickLine, movePos);
            float d = Math.Min(gl.Width, gl.Height) / 3;
            speedX = Math.Max(-1, Math.Min(1, (xPos - xDown) / d));
            speedY = Math.Max(-1, Math.Min(1, (yPos - yDown) / d));
        }

        /// <summary>
        /// Mouse moves up event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gl_MouseUp(object sender, MouseEventArgs e)
        {
            speedX = speedY = 0;
        }

        /// <summary>
        /// Scroll wheel event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gl_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta != 0)
            {
                RepetierHost.view.ThreeDView.zoom *= 1f - (e.Delta / 2000f);
                if (RepetierHost.view.ThreeDView.zoom < 0.01)
                {
                    RepetierHost.view.ThreeDView.zoom = 0.01f;
                }

                if (RepetierHost.view.ThreeDView.zoom > 5.9)
                {
                    RepetierHost.view.ThreeDView.zoom = 5.9f;
                }

                gl.Invalidate();
            }
        }

        /// <summary>
        /// Application idle event. Called by the timer_tick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Application_Idle(object sender, EventArgs e)
        {
            if ((!loaded) || ((speedX == 0) && (speedY == 0)))
            {
                return;
            }
            
            sw.Stop(); // we've measured everything since last Idle run
            double milliseconds = sw.Elapsed.TotalMilliseconds;
            sw.Reset(); // reset stopwatch
            sw.Start(); // restart stopwatch
            Keys k = Control.ModifierKeys;

            modeOptions tempMode = currentMode;

            if ((k == Keys.Shift) || (Control.MouseButtons == MouseButtons.Middle))
            {
                tempMode = modeOptions.MoveViewpoint;
            }

            if (k == Keys.Control)
            {
                tempMode = modeOptions.Rotation;
            }

            if ((k == Keys.Alt) || (Control.MouseButtons == MouseButtons.Right))
            {
                tempMode = modeOptions.MoveObject;
            }

            if (tempMode == modeOptions.Rotation)
            {
                float d = Math.Min(gl.Width, gl.Height) / 3;
                speedX = (xPos - xDown) / d;
                speedY = (yPos - yDown) / d;
                RepetierHost.view.ThreeDView.rotZ = startRotZ + (speedX * 50);
                RepetierHost.view.ThreeDView.rotX = startRotX + (speedY * 50);
                gl.Invalidate();
            }
            else if (tempMode == modeOptions.Move)
            {
                speedX = (xPos - xDown) / gl.Width;
                speedY = (yPos - yDown) / gl.Height;
                RepetierHost.view.ThreeDView.userPosition.X = startUserPosition.X + speedX * 200 * RepetierHost.view.ThreeDView.zoom;
                RepetierHost.view.ThreeDView.userPosition.Z = startUserPosition.Z - speedY * 200 * RepetierHost.view.ThreeDView.zoom;
                if (RepetierHost.view.ThreeDView.rotX == 90 && RepetierHost.view.ThreeDView.rotZ == 0 && perspectiveModeisParrallel == true)
                {
                    RepetierHost.view.ThreeDView.viewCenter.X = startViewCenter.X + speedX * 200 * RepetierHost.view.ThreeDView.zoom;
                    RepetierHost.view.ThreeDView.viewCenter.Z = startViewCenter.Z - speedY * 200 * RepetierHost.view.ThreeDView.zoom;
                }

                gl.Invalidate();
            }
            else if (tempMode == modeOptions.MoveViewpoint)
            {
                speedX = (xPos - xDown) / gl.Width;
                speedY = (yPos - yDown) / gl.Height;
                RepetierHost.view.ThreeDView.viewCenter.X = startViewCenter.X - speedX * 200 * RepetierHost.view.ThreeDView.zoom;
                RepetierHost.view.ThreeDView.viewCenter.Z = startViewCenter.Z + speedY * 200 * RepetierHost.view.ThreeDView.zoom;
                if ((RepetierHost.view.ThreeDView.rotX == 90) && (RepetierHost.view.ThreeDView.rotZ == 0) && (perspectiveModeisParrallel == true))
                {
                    RepetierHost.view.ThreeDView.userPosition.X = startUserPosition.X - (speedX * (200 * RepetierHost.view.ThreeDView.zoom));
                    RepetierHost.view.ThreeDView.userPosition.Z = startUserPosition.Z + (speedY * (200 * RepetierHost.view.ThreeDView.zoom));
                }
                
                gl.Invalidate();
            }
            else if (tempMode == modeOptions.Zoom)
            {
                RepetierHost.view.ThreeDView.zoom *= (1 - speedY);
                speedY = 0;
                if (RepetierHost.view.ThreeDView.zoom < 0.01)
                {
                    RepetierHost.view.ThreeDView.zoom = 0.01f;
                }

                if (RepetierHost.view.ThreeDView.zoom > 5.9)
                {
                    RepetierHost.view.ThreeDView.zoom = 5.9f;
                }

                yDown = yPos;
                gl.Invalidate();
            }
            else if (tempMode == modeOptions.MoveObject)
            {
                Geom3DVector diff = movePos.sub(moveLast);
                moveLast = movePos;
                speedX = (xPos - lastX) * 200 * RepetierHost.view.ThreeDView.zoom / gl.Width;
                speedY = (yPos - lastY) * 200 * RepetierHost.view.ThreeDView.zoom / gl.Height;
                if (view.eventObjectMoved != null)
                {
                    view.eventObjectMoved(diff.x, diff.y);
                }

               lastX = xPos;
                lastY = yPos;
                gl.Invalidate();
            }
        }  
     
        /// <summary>
        /// Resets the view to the default and turns off parallel projection. 
        /// </summary>
        public void ResetView()
        {
            RepetierHost.view.ThreeDView.rotX = 20;
            RepetierHost.view.ThreeDView.rotZ = 0;
            RepetierHost.view.ThreeDView.zoom = 1.0f;
            RepetierHost.view.ThreeDView.userPosition = new Vector3(0f * ps.PrintAreaWidth, -1.6f * (float)Math.Sqrt(ps.PrintAreaDepth * ps.PrintAreaDepth + ps.PrintAreaWidth * ps.PrintAreaWidth + ps.PrintAreaHeight * ps.PrintAreaHeight), 0.0f * ps.PrintAreaHeight);
            RepetierHost.view.ThreeDView.viewCenter = new Vector3(0f * ps.PrintAreaWidth, ps.PrintAreaDepth * 0f, 0.0f * ps.PrintAreaHeight);
            perspectiveModeisParrallel = false;
            gl.Invalidate();
        }
      
        /// <summary>
        /// Key pressed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ThreeDControl_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {            
            if (e.KeyChar == '-')
            {
                RepetierHost.view.ThreeDView.zoom *= 1.05f;
                if (RepetierHost.view.ThreeDView.zoom > 10)
                {
                    RepetierHost.view.ThreeDView.zoom = 10;
                }

                gl.Invalidate();
                e.Handled = true;
            }

            if (e.KeyChar == '+')
            {
                RepetierHost.view.ThreeDView.zoom *= 0.95f;
                if (RepetierHost.view.ThreeDView.zoom < 0.01)
                {
                    RepetierHost.view.ThreeDView.zoom = 0.01f;
                }

                gl.Invalidate();
                e.Handled = true;
            }
        }

        /// <summary>
        /// The timer tick event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            Application_Idle(sender, e);
            timeCall++;

            foreach (ThreeDModel m in view.models)
            {
                if (m.Changed || m.hasAnimations)
                {
                    // if ((Main.threeDSettings.drawMethod == 0 && (timeCall % 9) != 0))
                    if ((ThreeDSettings.currentDrawMethod == ThreeDSettings.drawMethod.DrawElements) && ((timeCall % 9) != 0))
                    {
                        return;
                    }

                    if (m.hasAnimations && (ThreeDSettings.currentDrawMethod != ThreeDSettings.drawMethod.DrawElements))
                    {
                        gl.Invalidate();
                    }
                    else if ((timeCall % 3) == 0)
                    {
                        gl.Invalidate();
                    }

                    return;
                }
            }
        }

        /// <summary>
        /// Resets the position of the camera and the look at fields so that you look at the object from the top. 
        /// </summary>
        public void GoToTopView()
        {
            RepetierHost.view.ThreeDView.rotX = 90;
            RepetierHost.view.ThreeDView.rotZ = 0;
            RepetierHost.view.ThreeDView.zoom = 1.0f;
            RepetierHost.view.ThreeDView.viewCenter = new Vector3(0f * ps.PrintAreaWidth, ps.PrintAreaDepth * 0.25f, 0.0f * ps.PrintAreaHeight);
            RepetierHost.view.ThreeDView.userPosition = new Vector3(0f * ps.PrintAreaWidth, -1.6f * (float)Math.Sqrt(ps.PrintAreaDepth * ps.PrintAreaDepth + ps.PrintAreaWidth * ps.PrintAreaWidth + ps.PrintAreaHeight * ps.PrintAreaHeight), 0.0f * ps.PrintAreaHeight);
            RepetierHost.view.ThreeDView.viewCenter = new Vector3(0f * ps.PrintAreaWidth, ps.PrintAreaDepth * 0f, 0.0f * ps.PrintAreaHeight);
            gl.Invalidate();
        }

        /// <summary>
        /// Resets the position of the camera and the look at fields so that you look at the object from the Front. 
        /// </summary>
        public void GoToFrontView()
        {
            RepetierHost.view.ThreeDView.rotX = 0;
            RepetierHost.view.ThreeDView.rotZ = 0;
            RepetierHost.view.ThreeDView.zoom = 1.0f;
            RepetierHost.view.ThreeDView.viewCenter = new Vector3(0f * ps.PrintAreaWidth, ps.PrintAreaDepth * 0.25f, 0.0f * ps.PrintAreaHeight);
            RepetierHost.view.ThreeDView.userPosition = new Vector3(0f * ps.PrintAreaWidth, -1.6f * (float)Math.Sqrt(ps.PrintAreaDepth * ps.PrintAreaDepth + ps.PrintAreaWidth * ps.PrintAreaWidth + ps.PrintAreaHeight * ps.PrintAreaHeight), 0.0f * ps.PrintAreaHeight);
            RepetierHost.view.ThreeDView.viewCenter = new Vector3(0f * ps.PrintAreaWidth, ps.PrintAreaDepth * 0f, 0.0f * ps.PrintAreaHeight);
            gl.Invalidate();
        }

        /// <summary>
        /// Resets the position of the camera and the look at fields so that you look at the object from the Side. 
        /// </summary>
        public void GoToSideView()
        {
            RepetierHost.view.ThreeDView    .rotX = 0;
            RepetierHost.view.ThreeDView.rotZ = 270;
            RepetierHost.view.ThreeDView.zoom = 1.0f;

            RepetierHost.view.ThreeDView.viewCenter = new Vector3(0f * ps.PrintAreaWidth, ps.PrintAreaDepth * 0.25f, 0.0f * ps.PrintAreaHeight);
            RepetierHost.view.ThreeDView.userPosition = new Vector3(0f * ps.PrintAreaWidth, -1.6f * (float)Math.Sqrt(ps.PrintAreaDepth * ps.PrintAreaDepth + ps.PrintAreaWidth * ps.PrintAreaWidth + ps.PrintAreaHeight * ps.PrintAreaHeight), 0.0f * ps.PrintAreaHeight);
            RepetierHost.view.ThreeDView.viewCenter = new Vector3(0f * ps.PrintAreaWidth, ps.PrintAreaDepth * 0f, 0.0f * ps.PrintAreaHeight);
            gl.Invalidate();
        }
       
        /// <summary>
        /// Not sure this is used. Looks like is meant to determine if the key pressed is one of interest. 
        /// </summary>
        /// <param name="keyData"></param>
        /// <returns>True if one of the important keys is pressed</returns>
        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Right:
                case Keys.Left:
                case Keys.Up:
                case Keys.Down:
                    return true;
                case Keys.Shift | Keys.Right:
                case Keys.Shift | Keys.Left:
                case Keys.Shift | Keys.Up:
                case Keys.Shift | Keys.Down:
                    return true;
            }

            return base.IsInputKey(keyData);
        }

        /// <summary>
        /// Event control for pressing down a key while the controller is selected. 
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Specifics about the key pressed down</param>
        public void ThreeDControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                RepetierHost.view.ThreeDView.rotZ -= 5;
                gl.Invalidate();
                e.Handled = true;
            }

            if (e.KeyCode == Keys.Right)
            {
                RepetierHost.view.ThreeDView.rotZ += 5;
                gl.Invalidate();
                e.Handled = true;
            }

            if (e.KeyCode == Keys.Up)
            {
                RepetierHost.view.ThreeDView.rotX -= 5;
                gl.Invalidate();
                e.Handled = true;
            }

            if (e.KeyCode == Keys.Down)
            {
                RepetierHost.view.ThreeDView.rotX += 5;
                gl.Invalidate();
                e.Handled = true;
            }

            if (e.KeyValue == '-')
            {
                RepetierHost.view.ThreeDView.zoom *= 1.05f;
                if (RepetierHost.view.ThreeDView.zoom > 10)
                {
                    RepetierHost.view.ThreeDView.zoom = 10;
                }

                gl.Invalidate();
                e.Handled = true;
            }

            if (e.KeyValue == '+')
            {
                RepetierHost.view.ThreeDView.zoom *= 0.95f;
                if (RepetierHost.view.ThreeDView.zoom < 0.01)
                {
                    RepetierHost.view.ThreeDView.zoom = 0.01f;
                }

                gl.Invalidate();
                e.Handled = true;
            }

            // If the event hasn't been handled, then send it on to the listSTLObjects key down to see if we should delete something. 
            // Basically we are making a way to delele objects. 
            if (Main.main.current3Dview == Main.ThreeDViewOptions.STLeditor && e.Handled == false)
            {
                Main.main.listSTLObjects_KeyDown(sender, e);
            }

            if (e.Handled == false)
            {
                Main.main.fileAddOrRemove.SaveAFile_KeyDown(sender, e);
            }
        }

        /// <summary>
        /// Changes to the opposite of the current prospective mode. Orthogonal or prospective. 
        /// </summary>
        public void ChangeProspectiveMode()
        {
            perspectiveModeisParrallel = !perspectiveModeisParrallel;
            gl.Invalidate();
        }

        /// <summary>
        /// Not working right now. I was trying to make a function that would center the view directly on the object.   
        /// </summary>
        internal void CenterViewOnObjects()
        {
            float xCenter = 0f * ps.PrintAreaWidth;
            float yCenter = ps.PrintAreaDepth * 0f;
            float zCenter = 0.0f * ps.PrintAreaHeight;           

            foreach (STL tempSTL in Main.main.listSTLObjects.Items)
            {
                xCenter += (tempSTL.xMax + tempSTL.xMin);
                yCenter += (tempSTL.yMax + tempSTL.yMin);
                zCenter += (tempSTL.zMax + tempSTL.zMin);
            }

            RepetierHost.view.ThreeDView.viewCenter = new Vector3(xCenter, yCenter, zCenter);
            gl.Invalidate();
        }
    }
}
