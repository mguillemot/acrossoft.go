using Acrossoft.Go.Game;
using Acrossoft.GoUtils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Acrossoft.Go.Display
{
    public class BoardDisplay
    {
        private static Texture2D s_blackPointTexture;
        private static Texture2D s_blackStoneTexture;
        private static Texture2D s_whiteStoneTexture;

        private readonly Board m_board;
        private BufferedGeometryRenderer m_renderer;

        public BoardDisplay(Board board)
        {
            m_board = board;
        }

        public static void LoadContent(ContentManager contentManager)
        {
            s_blackStoneTexture = contentManager.Load<Texture2D>("black_stone");
            s_whiteStoneTexture = contentManager.Load<Texture2D>("white_stone");
            s_blackPointTexture = contentManager.Load<Texture2D>("black_point");
        }

        public void Initialize(GraphicsDevice device)
        {
            m_renderer = new BufferedGeometryRenderer(device);
        }

        public void Draw(Point position, int size)
        {
            float gridCellSize = (float) size/(m_board.Size + 1);
            var gridSize = (int) ((m_board.Size - 1)*gridCellSize);
            var gridCorner = new Point(position.X + (int) gridCellSize, position.Y + (int) gridCellSize);
            m_renderer.Begin();
            m_renderer.FillRectangle(new Rectangle(position.X, position.Y, size, size), new Color(219, 178, 91));
            m_renderer.DrawRectangle(new Rectangle(position.X, position.Y, size, size), Color.Black);
            for (int i = 0; i < m_board.Size; i++)
            {
                int x = gridCorner.X + (int) (i*gridCellSize);
                int y = gridCorner.Y + (int) (i*gridCellSize);
                m_renderer.DrawLine(new Point(x, gridCorner.Y), new Point(x, gridCorner.Y + gridSize), Color.Black);
                m_renderer.DrawLine(new Point(gridCorner.X, y), new Point(gridCorner.X + gridSize, y), Color.Black);
            }
            DrawPointAt(position, gridCellSize, 3, 3);
            DrawPointAt(position, gridCellSize, 9, 3);
            DrawPointAt(position, gridCellSize, 15, 3);
            DrawPointAt(position, gridCellSize, 3, 9);
            DrawPointAt(position, gridCellSize, 9, 9);
            DrawPointAt(position, gridCellSize, 15, 9);
            DrawPointAt(position, gridCellSize, 3, 15);
            DrawPointAt(position, gridCellSize, 9, 15);
            DrawPointAt(position, gridCellSize, 15, 15);
            for (int i = 1; i <= m_board.Size; i++)
            {
                for (int j = 1; j <= m_board.Size; j++)
                {
                    Stone stone = m_board.Get(i, j);
                    if (stone != Stone.NONE)
                    {
                        Texture2D texture = (stone == Stone.BLACK) ? s_blackStoneTexture : s_whiteStoneTexture;
                        m_renderer.DrawSprite(texture,
                                              new Rectangle(position.X + 1 + (int) ((i - 0.5f)*gridCellSize),
                                                            position.Y + 1 + (int) ((j - 0.5f)*gridCellSize),
                                                            (int) gridCellSize - 2, (int) gridCellSize - 2));
                    }
                }
            }
            DrawCursorAt(position, gridCellSize, 1, 1);
            m_renderer.End();
        }

        private void DrawPointAt(Point position, float gridCellSize, int x, int y)
        {
            m_renderer.DrawSprite(s_blackPointTexture, new Rectangle(position.X + (int)((x + 1) * gridCellSize) - 4, position.Y + (int)((y + 1) * gridCellSize) - 4, 9, 9));
        }

        private void DrawCursorAt(Point position, float gridCellSize, int x, int y)
        {
            int cursorHalfSize = (int)(gridCellSize/2) + 1;
            int cursorCornerSize = (int)(0.7f*gridCellSize/2);
            int centerX = position.X + (int) ((x + 1)*gridCellSize);
            int centerY = position.Y + (int) ((y + 1)*gridCellSize);
            Point topLeftCorner = new Point(centerX - cursorHalfSize, centerY - cursorHalfSize);
            Point topRightCorner = new Point(centerX + cursorHalfSize, centerY - cursorHalfSize);
            Point bottomLeftCorner = new Point(centerX - cursorHalfSize, centerY + cursorHalfSize);
            Point bottomRightCorner = new Point(centerX + cursorHalfSize, centerY + cursorHalfSize);
            m_renderer.DrawLine(topLeftCorner, new Point(topLeftCorner.X, topLeftCorner.Y + cursorCornerSize), Color.Red);
            m_renderer.DrawLine(topLeftCorner, new Point(topLeftCorner.X + cursorCornerSize, topLeftCorner.Y), Color.Red);
            m_renderer.DrawLine(topRightCorner, new Point(topRightCorner.X, topRightCorner.Y + cursorCornerSize), Color.Red);
            m_renderer.DrawLine(topRightCorner, new Point(topRightCorner.X - cursorCornerSize, topRightCorner.Y), Color.Red);
            m_renderer.DrawLine(bottomLeftCorner, new Point(bottomLeftCorner.X, bottomLeftCorner.Y - cursorCornerSize), Color.Red);
            m_renderer.DrawLine(bottomLeftCorner, new Point(bottomLeftCorner.X + cursorCornerSize, bottomLeftCorner.Y), Color.Red);
            m_renderer.DrawLine(bottomRightCorner, new Point(bottomRightCorner.X, bottomRightCorner.Y - cursorCornerSize), Color.Red);
            m_renderer.DrawLine(bottomRightCorner, new Point(bottomRightCorner.X - cursorCornerSize, bottomRightCorner.Y), Color.Red);
        }
    }
}