using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using RepRap_Phone_Host.RenderUtils;
using RepRap_Phone_Host.GlobalValues;

namespace RepRap_Phone_Host.GCode
{
    class GCodeComponent : DrawableGameComponent
    {
        VertexBuffer vertexBuffer;

        List<VertexPositionColoredNormal> vertexList;

        Matrix world = Matrix.CreateTranslation(0, 0, 0);
        Matrix view;
        Matrix projection;

        public PhoneApplicationPage mainPage;

        public GCodeComponent(Game game, PhoneApplicationPage _mainPage)
            : base(game)
        {
            mainPage = _mainPage;
        }

        #region overrides
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

            loadGCodeContent();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            //loadGCodeContent();

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
            Values.GCode_IsBusy = false;

            //GCodeLoadingThread.Abort();          

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
            var controller = (RenderingController)this.Game;
            var basicEffect = controller.BasicEffect;

            basicEffect.World = world;
            basicEffect.View = view;
            basicEffect.Projection = projection;
            basicEffect.VertexColorEnabled = true;
            basicEffect.EnableDefaultLighting();

            GraphicsDevice.RasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState.FillMode = FillMode.Solid;

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                try
                {
                    if (vertexBuffer != null)
                    {
                        GraphicsDevice.SetVertexBuffer(vertexBuffer);
                        GraphicsDevice.DrawPrimitives(PrimitiveType.LineList, Values.layerStartIndices[Values.minLayer],
                            (Values.layerStartIndices[Values.maxLayer] - Values.layerStartIndices[Values.minLayer]) / 2);
                    }

                    if (controller.gridBuffer != null)
                    {
                        GraphicsDevice.SetVertexBuffer(controller.gridBuffer);
                        GraphicsDevice.DrawPrimitives(PrimitiveType.LineList, 0, controller.gridBuffer.VertexCount / 2);
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }

            base.Draw(gameTime);
        }
        #endregion

        #region loading
        Thread GCodeLoadingThread = null;//This is the thread that will do the loading

        public void loadGCodeContent()
        {
            //This needs to run in a background thread because it is quite slow
            if (GCodeLoadingThread == null || GCodeLoadingThread.ThreadState != ThreadState.Running)
            {
                GCodeLoadingThread = new Thread(new ThreadStart(() =>
                {
                    if (GraphicsDevice == null || Values.GCode_IsBusy)
                        return;

                    Values.GCode_IsBusy = true;

                    //This is needed to prevent memory fom leaking
                    if (vertexList != null)
                    {
                        vertexList.Clear();
                        vertexList = null;
                    }

                    try
                    {
                        vertexList = GCodeImporter.loadGCodeFromFile(Values.currentGCodeFile, Color.Red);

                        //Allow the operation to be cancelled
                        if (!Values.GCode_IsBusy)
                            return;

                        loadGCodeVerticesInBuffer();
                    }
                    catch (Exception e)
                    {
                        Values.GCode_IsBusy = false;

                        if (mainPage == null)
                            return;

                        if (e.GetType() == typeof(OutOfMemoryException))
                            mainPage.Dispatcher.BeginInvoke(() =>
                            {
                                MessageBox.Show("Sorry, but the GCodeImporter ran out of memory and your model could not be loaded!");
                            });
                        else
                            mainPage.Dispatcher.BeginInvoke(() =>
                            {
                                //MessageBox.Show(e.ToString());
                                MessageBox.Show("Sorry, but there was a problem loading the GCode file. Maybe it is invalid");
                            }); 
                    }

                    Values.GCode_IsBusy = false;
                }));

                GCodeLoadingThread.Start();
            }
        }

        private void loadGCodeVerticesInBuffer()
        {
            try
            {
                //This is needed to prevent memory fom leaking
                if (vertexBuffer != null)
                {
                    vertexBuffer.Dispose();
                    vertexBuffer = null;
                }

                vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColoredNormal), vertexList.Count, BufferUsage.WriteOnly);
                vertexBuffer.SetData<VertexPositionColoredNormal>(vertexList.ToArray());
            }
            catch (Exception e)
            {
                if (mainPage == null)
                    return;

                if (e.GetType() == typeof(OutOfMemoryException))
                    mainPage.Dispatcher.BeginInvoke(() =>
                    {
                        MessageBox.Show("Sorry, but the GCodeImporter ran out of memory and your model could not be loaded!");
                    });
                else if (e.GetType() != typeof(ThreadAbortException))
                    mainPage.Dispatcher.BeginInvoke(() =>
                    {
                        //MessageBox.Show(e.ToString());
                        MessageBox.Show("Sorry, but there was a problem loading the GCode file. Maybe it is invalid");
                    });
            }
        }
        #endregion
    }
}
