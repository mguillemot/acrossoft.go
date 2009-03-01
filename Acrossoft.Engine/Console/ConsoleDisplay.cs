using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Acrossoft.Engine.Console
{
    public class ConsoleDisplay
    {
        private static SpriteFont s_systemFont;

        private GraphicsDevice m_device;
        private BasicEffect m_basicEffect;
        private VertexPositionColor[] m_grid;
        private SpriteBatch m_spriteBatch;
        private VertexBuffer m_vertexBuffer;
        private VertexDeclaration m_vertexDeclaration;

        public ConsoleDisplay()
        {
            CharacterHeight = 12;
        }

        public static void LoadContent(ContentManager content)
        {
            s_systemFont = content.Load<SpriteFont>("SystemFont");
        }

        public int CharacterHeight { get; set; }

        public int CharacterWidth
        {
            get { return 159; }  // TODO rendre ça dépendant de la résolution
        }

        public void Initialize(GraphicsDevice device)
        {
            m_device = device;
            m_basicEffect = new BasicEffect(device, null)
                {
                VertexColorEnabled = true,
                World = Matrix.CreateTranslation(100.0f, 50.0f, 0.0f),
                View = Matrix.CreateLookAt(new Vector3(0.0f, 0.0f, 1.0f), Vector3.Zero, Vector3.Up),
                Projection = Matrix.CreateOrthographicOffCenter(0, device.Viewport.Width,
                                device.Viewport.Height, 0,
                                1.0f,
                                1000.0f)
                };

            // Init grid
            int pixelHeight = CharacterHeight*16 + 8;
            m_grid = new VertexPositionColor[4];
            m_grid[0].Position = new Vector3(0, device.Viewport.Height - pixelHeight, 0);
            m_grid[0].Color = Color.Black;
            m_grid[0].Color.A = 200;
            m_grid[1].Position = new Vector3(device.Viewport.Width, device.Viewport.Height - pixelHeight, 0);
            m_grid[1].Color = Color.Black;
            m_grid[1].Color.A = 200;
            m_grid[2].Position = new Vector3(device.Viewport.Width, device.Viewport.Height, 0);
            m_grid[2].Color = Color.Black;
            m_grid[2].Color.A = 200;
            m_grid[3].Position = new Vector3(0, device.Viewport.Height, 0);
            m_grid[3].Color = Color.Black;
            m_grid[3].Color.A = 200;

            // Init vertex buffer
            m_vertexBuffer = new VertexBuffer(device, VertexPositionColor.SizeInBytes*m_grid.Length,
                                              BufferUsage.None);
            m_vertexBuffer.SetData(m_grid);
            m_vertexDeclaration = new VertexDeclaration(m_device, VertexPositionColor.VertexElements);

            // Init 2D batch
            m_spriteBatch = new SpriteBatch(device);
        }

        public void Draw()
        {
            // Background
            m_device.VertexDeclaration = m_vertexDeclaration;
            m_device.Vertices[0].SetSource(m_vertexBuffer, 0, VertexPositionColor.SizeInBytes);
            m_basicEffect.World = Matrix.Identity;
            m_device.RenderState.CullMode = CullMode.None;
            m_basicEffect.Begin();
            foreach (EffectPass pass in m_basicEffect.CurrentTechnique.Passes)
            {
                pass.Begin();
                m_device.DrawPrimitives(PrimitiveType.TriangleFan, 0, 2);
                pass.End();
            }
            m_basicEffect.End();

            // Text
            m_spriteBatch.Begin();
            m_spriteBatch.DrawString(s_systemFont, Console.GetFormattedText(CharacterHeight), new Vector2(3, m_device.Viewport.Height - (CharacterHeight * 16 + 8)), Color.LightGreen);
            m_spriteBatch.End();
        }
    }
}