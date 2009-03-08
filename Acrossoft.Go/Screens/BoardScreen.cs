using Acrossoft.Engine.Controls;
using Acrossoft.Engine.Screens;
using Acrossoft.Go.Display;
using Acrossoft.Go.Game;
using Acrossoft.GoUtils.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Acrossoft.Go.Screens
{
    public class BoardScreen : Screen
    {
        private Board m_board;
        private BoardControl m_boardControl;
        private BoardDisplay m_boardDisplay;

        private Board m_enemyBoard;
        private BoardDisplay m_enemyBoardDisplay;

        private Stone m_color;
        private Point m_cursorPosition;
        private bool m_groupdisplay; // debug display

        private Function m_upFunction;
        private Function m_downFunction;
        private Function m_leftFunction;
        private Function m_playFunction;
        private Function m_rightFunction;

        public BoardScreen(Microsoft.Xna.Framework.Game game)
            : base(game)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            m_board = new Board(19);
            m_boardControl = new BoardControl(m_board);
            m_boardDisplay = new BoardDisplay(m_board)
                                 {
                                     Position = new Point(100, 100),
                                     Size = 500,
                                     ShowCursor = true
                                 };
            m_boardDisplay.Initialize(GraphicsDevice);

            m_enemyBoard = new Board(19);
            m_enemyBoardDisplay = new BoardDisplay(m_enemyBoard)
            {
                Position = new Point(680, 100),
                Size = 500
            };
            m_enemyBoardDisplay.Initialize(GraphicsDevice);

            var controls = (IControlsProvider) Game.Services.GetService(typeof (IControlsProvider));
            m_downFunction = new Function("boardDown");
            m_downFunction.AssignKey(Keys.Down, 100, 50);
            m_downFunction.AssignButton(Buttons.LeftThumbstickDown, 100, 50);
            m_downFunction.AssignButton(Buttons.DPadDown, 100, 50);
            controls.CurrentConfig.RegisterFunction(m_downFunction);
            m_upFunction = new Function("boardUp");
            m_upFunction.AssignKey(Keys.Up, 100, 50);
            m_upFunction.AssignButton(Buttons.LeftThumbstickUp, 100, 50);
            m_upFunction.AssignButton(Buttons.DPadUp, 100, 50);
            controls.CurrentConfig.RegisterFunction(m_upFunction);
            m_leftFunction = new Function("boardLeft");
            m_leftFunction.AssignKey(Keys.Left, 100, 50);
            m_leftFunction.AssignButton(Buttons.LeftThumbstickLeft, 100, 50);
            m_leftFunction.AssignButton(Buttons.DPadLeft, 100, 50);
            controls.CurrentConfig.RegisterFunction(m_leftFunction);
            m_rightFunction = new Function("boardRight");
            m_rightFunction.AssignKey(Keys.Right, 100, 50);
            m_rightFunction.AssignButton(Buttons.LeftThumbstickRight, 100, 50);
            m_rightFunction.AssignButton(Buttons.DPadRight, 100, 50);
            controls.CurrentConfig.RegisterFunction(m_rightFunction);
            m_playFunction = new Function("boardPlay");
            m_playFunction.AssignKey(Keys.Enter);
            m_playFunction.AssignButton(Buttons.A);
            controls.CurrentConfig.RegisterFunction(m_playFunction);

            m_cursorPosition.X = (m_board.Size - 1)/2;
            m_cursorPosition.Y = (m_board.Size - 1)/2;

            m_color = Stone.BLACK;
            m_groupdisplay = false;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (m_downFunction.Triggered && m_cursorPosition.Y < m_board.Size - 1)
            {
                m_cursorPosition.Y++;
            }
            if (m_upFunction.Triggered && m_cursorPosition.Y > 0)
            {
                m_cursorPosition.Y--;
            }
            if (m_rightFunction.Triggered && m_cursorPosition.X < m_board.Size - 1)
            {
                m_cursorPosition.X++;
            }
            if (m_leftFunction.Triggered && m_cursorPosition.X > 0)
            {
                m_cursorPosition.X--;
            }

            int mouseX = Mouse.GetState().X - m_boardDisplay.Position.X;
            int mouseY = Mouse.GetState().Y - m_boardDisplay.Position.Y;
            bool click = false;
            if (mouseX >= 0 && mouseX < m_boardDisplay.Size && mouseY >= 0 && mouseY < m_boardDisplay.Size)
            {
                m_cursorPosition = m_boardDisplay.ComputePositionFromObjetCoords(mouseX, mouseY);
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    click = true;
                }
            }

            m_boardDisplay.SetCursorPosition(m_cursorPosition);
            if ((m_playFunction.Triggered || click) && m_boardControl.CanPlay(m_cursorPosition, m_color))
            {
                m_boardControl.Play(m_cursorPosition, m_color);
                m_color = (m_color == Stone.WHITE) ? Stone.BLACK : Stone.WHITE;
            }

            // display only a particular group (for debug)
            bool numkeypressed = false;
            int kd = 10;
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad0))
            {
                kd = 0;
                numkeypressed = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad1))
            {
                kd = 1;
                numkeypressed = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad2))
            {
                kd = 2;
                numkeypressed = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad3))
            {
                kd = 3;
                numkeypressed = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad4))
            {
                kd = 4;
                numkeypressed = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad5))
            {
                kd = 5;
                numkeypressed = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad6))
            {
                kd = 6;
                numkeypressed = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad7))
            {
                kd = 7;
                numkeypressed = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad8))
            {
                kd = 8;
                numkeypressed = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad9))
            {
                kd = 9;
                numkeypressed = true;
            }

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
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            m_boardDisplay.Draw();
            m_enemyBoardDisplay.Draw();
        }
    }
}