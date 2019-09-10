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
using System.Threading;
using System.Diagnostics;
using RepRap_Phone_Host.GlobalValues;
using System.Windows;

namespace RepRap_Phone_Host.RenderUtils
{
    /// <summary>
    /// This is the main game class that will be used by our renderers
    /// </summary>
    class RenderingController : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphicsDeviceManager;
        BasicEffect basicEffect;

        public VertexBuffer gridBuffer;
        public List<VertexPositionColoredNormal> boxList;
        public bool isLoadingBox = true;//This property is used to determine if we have finished loading our intial content
        
        //View properties
        public static int zoomLevel = 0;
        public static float xRot = 0;
        public static float yRot = 0;
        public static float zRot = 0;
        public static int xOffset = 0;
        public static int yOffset = 0;
        public static float objectSink = 0;
        public static float objectScale = 1;

        static Color bgColor = Color.Black;
        static Color boxColor = Color.White;

        public RenderingController()
        {
            graphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //Update the grid is the size of the bed changes
            Settings.bedHeigthChangedEvent += bedSizeChangedHandler;
            Settings.bedLengthChangedEvent += bedSizeChangedHandler;
            Settings.bedWithChangedEvent += bedSizeChangedHandler;

            // Determine the visibility of the dark background.
            Visibility darkBackgroundVisibility =
                (Visibility)Application.Current.Resources["PhoneDarkThemeVisibility"];

            // Write the theme background value.
            if (darkBackgroundVisibility != Visibility.Visible)
            {
                bgColor = Color.White;
                boxColor = Color.Black;
            }
        }

        void bedSizeChangedHandler(object o)
        {
            isLoadingBox = true;

            boxList = BoxCreator.createBox(Settings.bedWidth, Settings.bedLength, Settings.bedHeigth, boxColor);

            gridBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColoredNormal), boxList.Count, BufferUsage.None);
            gridBuffer.SetData<VertexPositionColoredNormal>(boxList.ToArray());

            zoomLevel = 0;

            isLoadingBox = false;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //This is needed for the application to be able to reset properly
            GraphicsDevice.DeviceReset += (o, e) =>
            {
                if (boxList != null && boxList.Count > 0)
                {
                    gridBuffer.Dispose();
                    gridBuffer = null;
                    gridBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColoredNormal), boxList.Count, BufferUsage.None);
                    gridBuffer.SetData<VertexPositionColoredNormal>(boxList.ToArray());
                }
            };

            base.Initialize();
        }

        public BasicEffect BasicEffect
        {
            get { return this.basicEffect; }
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            basicEffect = new BasicEffect(GraphicsDevice);

            loadBox();
        }

        /// <summary>
        /// This function is used to load the grid box
        /// that represents the printbed.
        /// </summary>
        private void loadBox()
        {
            //return;
            new Thread(new ThreadStart(() =>
            {
                boxList = BoxCreator.createBox(Settings.bedWidth, Settings.bedLength, Settings.bedHeigth, boxColor);

                gridBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColoredNormal), boxList.Count, BufferUsage.None);
                gridBuffer.SetData<VertexPositionColoredNormal>(boxList.ToArray());

                isLoadingBox = false;
            })).Start();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(bgColor);

            base.Draw(gameTime);
        }

        #region View Manipulation
        /// <summary>
        /// This method zooms in on the current rendering position by increments of 5
        /// </summary>
        public void zoomIn()
        {
            if (zoomLevel > Math.Max(Settings.bedWidth, Settings.bedLength) - 10)
                return;

            zoomLevel += 5;
        }

        /// <summary>
        /// This method zooms out of the current rendering position by increments of 5
        /// </summary>
        public void zoomOut()
        {
            if (zoomLevel < -Math.Min(Settings.bedWidth, Settings.bedLength) + 10)
                return;

            zoomLevel -= 5;
        }

        /// <summary>
        /// This method is used to rotate the camera left by increments of 0.05
        /// </summary>
        public void rotateLeft()
        {
            yRot -= 0.05f;
        }

        /// <summary>
        /// This method is used to rotate the camera right in increments of 0.05
        /// </summary>
        public void rotateRight()
        {
            yRot += 0.05f;
        }

        /// <summary>
        /// This method is used to rotate the camera up in increments of 0.05
        /// </summary>
        public void rotateUp()
        {
            xRot -= 0.05f;
        }

        /// <summary>
        /// This method is used to rotate the camera down in increments of 0.05
        /// </summary>
        public void rotateDown()
        {
            xRot += 0.05f;
        }

        /// <summary>
        /// This method is used to move the camera left in increments of 5 
        /// </summary>
        public void moveLeft()
        {
            xOffset += 5;
        }

        /// <summary>
        /// This method is used to move the camera right in increments of 5
        /// </summary>
        public void moveRight()
        {
            xOffset -= 5;
        }

        /// <summary>
        /// This method is used to move the camera up in increments of 5
        /// </summary>
        public void moveUp()
        {
            yOffset -= 5;
        }

        /// <summary>
        /// This method is used to move the camera down in increments of 5
        /// </summary>
        public void moveDown()
        {
            yOffset += 5;
        }
        #endregion
    }
}
