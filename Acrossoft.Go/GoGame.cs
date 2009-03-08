using Acrossoft.Engine.Console;
using Acrossoft.Engine.Controls;
using Acrossoft.Go.Display;
using Acrossoft.Go.Game;
using Acrossoft.GoAi;
using Acrossoft.GoUtils.Entities;
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

        private readonly Keys[] m_arrow ;
        private readonly bool[] m_arrowDown;
        private readonly System.TimeSpan[] m_arrowDownMemory;
        private bool m_buttonDown;
        private Point m_cursorPosition;
        private Stone m_color;

        private bool m_groupdisplay; // debug display

        public GoGame()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;

            Content.RootDirectory = "Content";

            Components.Add(new ControlsComponent(this));
            Components.Add(new ConsoleComponent(this));

            m_arrowDown = new bool[4];
            m_arrow = new Keys[4];
            m_arrowDownMemory = new System.TimeSpan[4];
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

            m_arrow[0] = Keys.Left;
            m_arrow[1] = Keys.Up;
            m_arrow[2] = Keys.Right;
            m_arrow[3] = Keys.Down;

            for (int i = 0; i < 4; ++i)
            {
                m_arrowDown[i] = false;
            }
            m_buttonDown = false;
            m_cursorPosition.X = (m_board.Size - 1) / 2;
            m_cursorPosition.Y = (m_board.Size - 1) / 2;

            m_color = Stone.BLACK;
            m_groupdisplay = false;

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

            for (int i = 0; i < 4; ++i)
            {
                if (Keyboard.GetState().IsKeyDown(m_arrow[i]))
                {
                    if (!m_arrowDown[i])
                    {
                        m_arrowDown[i] = true;
                        m_arrowDownMemory[i] = gameTime.TotalGameTime;
                    }
                    else
                    {
                        if ( (gameTime.TotalGameTime - m_arrowDownMemory[i]).Milliseconds >= 100 )
                        {
                            m_arrowDownMemory[i] = gameTime.TotalGameTime;
                        }
                    }
                }
                else
                {
                    m_arrowDown[i] = false;
                }

                if (m_arrowDown[i] && m_arrowDownMemory[i].CompareTo(gameTime.TotalGameTime) == 0)
                {
                    switch (i)
                    {
                        case 0: if (--m_cursorPosition.X < 0) { m_cursorPosition.X = 0; } break;
                        case 1: if (--m_cursorPosition.Y < 0) { m_cursorPosition.Y = 0; } break;
                        case 2: if (++m_cursorPosition.X > m_board.Size - 1) { m_cursorPosition.X = m_board.Size - 1; } break;
                        case 3: if (++m_cursorPosition.Y > m_board.Size - 1) { m_cursorPosition.Y = m_board.Size - 1; } break;
                    }
                }
            }

            m_boardDisplay.SetCursorPosition(m_cursorPosition);

            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                if (!m_buttonDown)
                {
                    m_buttonDown = true;
                    m_boardControl.Play(m_cursorPosition, m_color);

                    m_color = (m_color == Stone.WHITE) ? Stone.BLACK : Stone.WHITE;
                }
                else
                {
                    // compare time
                }
            }
            else
            {
                m_buttonDown = false;
            }

            // display only a particular group (for debug)
            bool numkeypressed = false;
            int kd = 10;
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad0)) { kd = 0; numkeypressed = true; }
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad1)) { kd = 1; numkeypressed = true; }
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad2)) { kd = 2; numkeypressed = true; }
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad3)) { kd = 3; numkeypressed = true; }
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad4)) { kd = 4; numkeypressed = true; }
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad5)) { kd = 5; numkeypressed = true; }
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad6)) { kd = 6; numkeypressed = true; }
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad7)) { kd = 7; numkeypressed = true; }
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad8)) { kd = 8; numkeypressed = true; }
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad9)) { kd = 9; numkeypressed = true; }
            
            if (numkeypressed)
            {
                if (!m_groupdisplay)
                {
                    m_groupdisplay = true;
                }
                else
                {
                    m_boardControl.RestoreBoard();
                }
                m_boardControl.HighlightGroup(kd);
            }
            else
            {
                if (m_groupdisplay)
                {
                    m_boardControl.RestoreBoard();
                    m_groupdisplay = false;
                }
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

            m_boardDisplay.Draw(new Point(200,100), 500);

            base.Draw(gameTime);
        }
    }
}
