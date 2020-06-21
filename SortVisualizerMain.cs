using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace SortVisualizer
{

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class SortVisualizerMain : Game
    {
        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private Texture2D dataBarTexture;
        private DataBarTexture dataBarTxtr;
        private SortVisualizer visualizer1;
        private SortVisualizer visualizer2;

        public SortVisualizerMain()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            dataBarTexture = Content.Load<Texture2D>("dataBar2");
            dataBarTxtr = new DataBarTexture(dataBarTexture, 4, 4);

            Rectangle viewportBounds = GraphicsDevice.Viewport.Bounds;

            Dataset data1 = new Dataset(250, Distribution.LinearDistribution);
            Dataset data2 = new Dataset(250, Distribution.LinearDistribution);

            visualizer1 = new SortVisualizer(data1, Order.RandomOrder, Sort.GnomeSorter,
                new Rectangle(0, 0, viewportBounds.Width / 2, viewportBounds.Height), dataBarTxtr);
            visualizer2 = new SortVisualizer(data2, Order.RandomOrder, Sort.BubbleSorter,
                new Rectangle(viewportBounds.Width / 2, 0, viewportBounds.Width / 2, viewportBounds.Height), dataBarTxtr);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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

            for (int i = 0; i < 30; i++)
            {
                visualizer1.Update(gameTime);
            }
            for (int i = 0; i < 30; i++)
            {
                visualizer2.Update(gameTime);
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            visualizer1.Draw(spriteBatch);
            visualizer2.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
