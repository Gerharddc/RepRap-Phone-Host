using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;
using System.Diagnostics;
using RepRap_Phone_Host.RenderUtils;
using System.Windows;
using RepRap_Phone_Host.GlobalValues;
using System.Threading;
using Microsoft.Phone.Controls;

namespace RepRap_Phone_Host.Stl
{
    class StlComponent : DrawableGameComponent
    {
        VertexBuffer vertexBuffer;
        List<VertexPositionColoredNormal> vertexList;

        Matrix world = Matrix.CreateTranslation(0, 0, 0);//The world matrix is not intialy used to do any alterations
        Matrix view;
        Matrix projection;

        //We need to rember the previous rotation that our model had sothat we can determine how much it has changed
        static float prevModelXRot = 0;
        static float prevModelYRot = 0;
        static float prevModelZRot = 0;

        static float modelXRot = 0;
        static float modelYRot = 0;
        static float modelZRot = 0;

        static float objectSink = 0;
        static float objectScale = 1;

        static float modelXOffset = 0;
        static float modelYOffset = 0;

        //We also need to remember the previous scale
        static float prevModelScale = 1;

        public PhoneApplicationPage mainPage;

        RenderingController controller;

        public StlComponent(Game game, PhoneApplicationPage _mainPage)
            : base(game)
        {
            mainPage = _mainPage;

            controller = (RenderingController)this.Game;
        }

        #region DrawableGameComponent Overrides
        public override void Initialize()
        {
            projection = Matrix.CreateOrthographic((Settings.bedWidth - RenderingController.zoomLevel) * GraphicsDevice.Viewport.AspectRatio,
                Settings.bedLength - RenderingController.zoomLevel, 0.1f, 1000.0f);

            view = Matrix.CreateLookAt(new Vector3(Settings.bedWidth / 2, Settings.bedLength / 2, Settings.bedHeigth),
                new Vector3(Settings.bedWidth / 2, Settings.bedLength / 2, 0), Vector3.Up);

            //This is needed for the application to be able to reset properly
            GraphicsDevice.DeviceReset += (o, e) =>
            {
                if (vertexList != null && vertexList.Count > 0)
                {
                    if (vertexBuffer != null)
                    {
                        vertexBuffer.Dispose();
                        vertexBuffer = null;
                    }

                    vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColoredNormal), vertexList.Count, BufferUsage.WriteOnly);
                    vertexBuffer.SetData<VertexPositionColoredNormal>(vertexList.ToArray());
                }
            };

            loadStlContent();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            if (vertexList != null)
            {
                vertexList.Clear();
                vertexList = null;
            }
            if (vertexBuffer != null)
            {
                vertexBuffer.Dispose();
                vertexBuffer = null;
            }

            base.UnloadContent();
        }

        protected override void Dispose(bool disposing)
        {
            Values.stl_IsBusy = false;
            //stlLoadingThread.Abort();

            UnloadContent();

            base.Dispose(disposing);
        }

        public override void Update(GameTime gameTime)
        {
            //Update the camera to the currently requested parameters 
            world = Matrix.CreateTranslation(-Settings.bedWidth / 2, -Settings.bedLength / 2, 0) * Matrix.CreateRotationX(RenderingController.xRot) *
                Matrix.CreateRotationY(RenderingController.yRot) * Matrix.CreateRotationZ(RenderingController.zRot) *
                Matrix.CreateTranslation(Settings.bedWidth / 2, Settings.bedLength / 2, 0);

            view = Matrix.CreateLookAt(new Vector3(Settings.bedWidth / 2 + RenderingController.xOffset, Settings.bedLength / 2 + RenderingController.yOffset, Settings.bedHeigth),
                new Vector3(Settings.bedWidth / 2 + RenderingController.xOffset, Settings.bedLength / 2 + RenderingController.yOffset, 0), Vector3.Up);

            var max = Math.Max(Settings.bedWidth, Settings.bedLength) - RenderingController.zoomLevel;

            projection = Matrix.CreateOrthographic(max * GraphicsDevice.Viewport.AspectRatio,
                max, 0.1f, 1000.0f);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            var basicEffect = controller.BasicEffect;

            basicEffect.World = world;
            basicEffect.View = view;
            basicEffect.Projection = projection;
            basicEffect.VertexColorEnabled = true;
            basicEffect.EnableDefaultLighting();

            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;//very important for stl to render properly
            GraphicsDevice.RasterizerState = rasterizerState;

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                //First draw the grid
                if (controller.gridBuffer != null)
                {
                    GraphicsDevice.SetVertexBuffer(controller.gridBuffer);
                    GraphicsDevice.DrawPrimitives(PrimitiveType.LineList, 0, controller.gridBuffer.VertexCount);
                }

                //Then the Stl model
                if (vertexBuffer != null)
                {
                    try
                    {
                        GraphicsDevice.SetVertexBuffer(vertexBuffer);
                        GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, vertexBuffer.VertexCount / 3);
                    }
                    catch (Exception) { }
                }
            }

            base.Draw(gameTime);
        }
        #endregion

        #region Loading
        /// <summary>
        /// This method should be called when the current stl file
        /// changes sothat the value inside the class can be changed.
        /// </summary>
        /// <param name="property_Name"></param>
        /// <param name="property_Value"></param>
        void ApplicationSettings_currentStlFileChanged(object property_Value)
        {
            loadStlContent();
        }

        static Thread stlLoadingThread = null;//This is the thread that will do the loading

        /// <summary>
        /// This method loads the stl file specified in ApplicationSettings
        /// into the graphics device.
        /// </summary>
        public void loadStlContent()
        {
            //This needs to run in a background thread because it is a bit slow
            if (stlLoadingThread == null || stlLoadingThread.ThreadState != ThreadState.Running)
            {
                stlLoadingThread = new Thread(new ThreadStart(() =>
                {
                    if (GraphicsDevice == null || Values.stl_IsBusy)
                        return;

                    //Reset prev properties
                    prevModelXRot = 0;
                    prevModelYRot = 0;
                    prevModelZRot = 0;
                    prevModelScale = 1;

                    Values.stl_IsBusy = true;

                    //This is needed to prevent memory fom leaking
                    if (vertexList != null)
                    {
                        vertexList.Clear();
                        vertexList = null;
                    }

                    try
                    {
                        vertexList = StlImporter.loadStlFromFile(Values.currentStlFile, Color.Red);

                        //Stop here if we should stop loading
                        if (!Values.stl_IsBusy)
                            return;

                        rotateCentreAndScaleIfNeeded();
                    }
                    catch (Exception e)
                    {
                        mainPage.Dispatcher.BeginInvoke(() =>
                            {
                                MessageBox.Show(e.ToString());
                            });
                        Debug.WriteLine(e);
                        Values.stl_IsBusy = false;
                    }

                    Values.stl_IsBusy = false;
                }));

                stlLoadingThread.Start();
            }

            //We only want one thread running at a time
            //if (stlLoadingThread.ThreadState != ThreadState.Running)
            //stlLoadingThread.Start();
        }

        /// <summary>
        /// This function reloads the stl vertices into the vertexbuffer from the vertexlist
        /// </summary>
        private void loadStlVerticesInBuffer()
        {
            try
            {
                if (vertexList == null)
                    return;

                //This is needed to prevent memory fom leaking
                if (vertexBuffer != null)
                {
                    vertexBuffer.Dispose();
                    vertexBuffer = null;
                }

                vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColoredNormal), vertexList.Count, BufferUsage.WriteOnly);
                vertexBuffer.SetData<VertexPositionColoredNormal>(vertexList.ToArray());
            }
            catch (Exception)
            {
                return;
            }
        }
        #endregion

        #region model rotation
        private void removeModelRotation()
        {
            //Rotate the model back to 0
            //VertexRotator.rotateVertexList(ref vertexList,
            //  MathHelper.ToRadians(-prevModelXRot), MathHelper.ToRadians(-prevModelYRot), MathHelper.ToRadians(-prevModelZRot));
            VertexRotator.rotateVertexList(ref vertexList, MathHelper.ToRadians(-prevModelXRot), 0, 0);
            VertexRotator.rotateVertexList(ref vertexList, 0, MathHelper.ToRadians(-prevModelYRot), 0);
            VertexRotator.rotateVertexList(ref vertexList, 0, 0, MathHelper.ToRadians(-prevModelZRot));
        }

        private void applyModelRotation()
        {
            //Store the amount of cahnge in shorter variables
            /*var xChanged = xRot - prevModelXRot;
            var yChanged = yRot - prevModelYRot;
            var zChanged = zRot - prevModelZRot;*/

            //Rotate the model back to 0
            //VertexRotator.rotateVertexList(ref vertexList,
            //  MathHelper.ToRadians(xChanged), MathHelper.ToRadians(yChanged), MathHelper.ToRadians(zChanged));

            VertexRotator.rotateVertexList(ref vertexList, 0, 0, MathHelper.ToRadians(modelZRot));
            VertexRotator.rotateVertexList(ref vertexList, 0, MathHelper.ToRadians(modelYRot), 0);
            VertexRotator.rotateVertexList(ref vertexList, MathHelper.ToRadians(modelXRot), 0, 0);

            //Store the new current rotation for later removal
            prevModelXRot = modelXRot;
            prevModelYRot = modelYRot;
            prevModelZRot = modelZRot;
        }

        public void changeXRot(float rot)
        {
            if (rot == modelXRot)
                return;

            modelXRot = rot;
            rotateCentreAndScaleIfNeeded();
        }

        public void changeYRot(float rot)
        {
            if (rot == modelYRot)
                return;

            modelYRot = rot;
            rotateCentreAndScaleIfNeeded();
        }

        public void changeZRot(float rot)
        {
            if (rot == modelZRot)
                return;

            modelZRot = rot;
            rotateCentreAndScaleIfNeeded();
        }
        #endregion

        #region model centreing
        private void centreModel()
        {
            //We should centre the model
            var centrePosition = new Vector3(Settings.bedWidth / 2 + modelXOffset, Settings.bedLength / 2 + modelYOffset, 0);
            VertexCentrer.centreVertexList(ref vertexList, centrePosition, objectSink);
        }

        public void changeXOffset(float off)
        {
            if (off == modelXOffset)
                return;

            modelXOffset = off;
            rotateCentreAndScaleIfNeeded();
        }

        public void changeYOffset(float off)
        {
            if (off == modelYOffset)
                return;

            modelYOffset = off;
            rotateCentreAndScaleIfNeeded();
        }
        #endregion

        #region Model scaling
        private void removeModelScaling()
        {
            VertexScaler.rotateVertexList(ref vertexList, 1 / prevModelScale);
        }

        private void applyModelScaling()
        {
            VertexScaler.rotateVertexList(ref vertexList, objectScale);

            prevModelScale = objectScale;
        }

        public void scaleModel(float scale)
        {
            if (scale == objectScale)
                return;

            objectScale = scale;
            rotateCentreAndScaleIfNeeded();
        }
        #endregion

        private void rotateCentreAndScaleIfNeeded()
        {
            new Thread(new ThreadStart(() =>
            {
                Values.stl_IsBusy = true;

                try
                {
                    removeModelRotation();
                    removeModelScaling();

                    applyModelScaling();
                    applyModelRotation();

                    centreModel();
                }
                catch (Exception) { }

                loadStlVerticesInBuffer();

                Values.stl_IsBusy = false;
            })).Start();
        }

        /// <summary>
        /// This function saves the current model with all its modifications and resets the modification
        /// parameters
        /// <returns>The path of the newly created file</returns>
        /// </summary>
        public string saveModelandReset()
        {
            //Save the current model to a file
            var newFilePath = Values.currentStlFile.Replace(".stl", "_mod.stl");
            StlExporter.exportStlToFile(vertexList, newFilePath);

            //Reset the modifiers
            modelXRot = 0;
            modelYRot = 0;
            modelZRot = 0;
            objectScale = 1;
            //modelXOffset = 0;
            //modelYOffset = 0;

            return newFilePath;
        }
    }
}
