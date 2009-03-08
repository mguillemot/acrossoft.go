using Acrossoft.Engine.Console;
using Acrossoft.Engine.Controls;
using Acrossoft.Engine.Network;
using Acrossoft.Go.Display;
using Acrossoft.Go.Game;
using Acrossoft.Go.Network;
using Acrossoft.Go.Screens;
using Acrossoft.GoAi;
using Acrossoft.GoUtils.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Acrossoft.Go
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GoGame : Microsoft.Xna.Framework.Game
    {
        public GoGame()
        {
            var graphics = new GraphicsDeviceManager(this)
                           {
                               PreferredBackBufferWidth = 1280,
                               PreferredBackBufferHeight = 720
                           };

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            Components.Add(new GamerServicesComponent(this));
            Components.Add(new ControlsComponent(this));
            Components.Add(new NetworkComponent(this));
            Components.Add(new MessageDispatcherComponent(this));
            Components.Add(new GameSessionComponent(this));
            Components.Add(new BoardScreen(this)); // current screen
            Components.Add(new ConsoleComponent(this));
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
            Console.WriteLine("Initialized.");
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            BoardDisplay.LoadContent(Content);
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);
        }
    }
}
