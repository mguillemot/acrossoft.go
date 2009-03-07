using Acrossoft.Engine.Console;
using Acrossoft.Engine.Controls;
using Acrossoft.Go.Display;
using Acrossoft.Go.Game;
using Acrossoft.GoAi;
using Acrossoft.GoUtils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Acrossoft.Go
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GoGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        private Board m_board;
        private BoardControl m_boardControl;
        private BoardDisplay m_boardDisplay;

        public GoGame()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;

            Content.RootDirectory = "Content";

            Components.Add(new ControlsComponent(this));
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
            m_board = new Board(19);
            m_boardControl = new BoardControl(m_board);
            m_boardDisplay = new BoardDisplay(m_board);
            m_boardDisplay.Initialize(GraphicsDevice);

            for (int i = 1; i <= m_board.Size; i++)
            {
                for (int j = 1; j <= m_board.Size; j++)
                {
                    m_board.Set(i, j, (i+j)%2==0 ? Stone.WHITE : Stone.BLACK);
                }
            }

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

            m_boardDisplay.Draw(new Point(200, 100), 500);

            base.Draw(gameTime);
        }
    }
}
