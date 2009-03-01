using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Acrossoft.Go.Display
{
    public class BufferedGeometryRenderer
    {
        private readonly GraphicsDevice m_device;
        private readonly BasicEffect m_effect;
        private readonly SpriteBatch m_spriteBatch;
        private readonly VertexDeclaration m_vertexDeclaration;

        public BufferedGeometryRenderer(GraphicsDevice device)
        {
            m_device = device;
            m_spriteBatch = new SpriteBatch(device);
            m_effect = new BasicEffect(device, null);
            m_effect.View = Matrix.CreateLookAt(new Vector3(0.0f, 0.0f, 10.0f), Vector3.Zero, Vector3.Up);
            m_effect.Projection = Matrix.CreateOrthographic(m_device.Viewport.Width, m_device.Viewport.Height, 1.0f, 1000.0f);
            m_effect.World = Matrix.CreateTranslation((float)-m_device.Viewport.Width / 2, (float)m_device.Viewport.Height / 2, 0.0f);
            m_effect.VertexColorEnabled = true;
            m_vertexDeclaration = new VertexDeclaration(m_device, VertexPositionColor.VertexElements);
        }

        public void Begin()
        {
            m_spriteBatch.Begin();
        }

        public void DrawLine(Point from, Point to, Color color)
        {
            var vertexes = new VertexPositionColor[2];
            vertexes[0].Position = new Vector3(from.X, -from.Y, 0.0f);
            vertexes[0].Color = color;
            vertexes[1].Position = new Vector3(to.X, -to.Y, 0.0f);
            vertexes[1].Color = color;
            m_device.VertexDeclaration = m_vertexDeclaration; 
            m_effect.Begin();
            foreach (var pass in m_effect.CurrentTechnique.Passes)
            {
                pass.Begin();
                m_device.DrawUserPrimitives(PrimitiveType.LineList, vertexes, 0, 1);
                pass.End();
            }
            m_effect.End();
        }

        public void DrawSprite(Texture2D texture, Rectangle destination)
        {
            m_spriteBatch.Draw(texture, destination, Color.White);
        }

        public void DrawRectangle(Rectangle rectangle, Color color)
        {
            var vertexes = new VertexPositionColor[4];
            vertexes[0].Position = new Vector3(rectangle.X, -rectangle.Y, 0.0f);
            vertexes[0].Color = color;
            vertexes[1].Position = new Vector3(rectangle.X, -rectangle.Y - rectangle.Width, 0.0f);
            vertexes[1].Color = color;
            vertexes[2].Position = new Vector3(rectangle.X + rectangle.Height, -rectangle.Y, 0.0f);
            vertexes[2].Color = color;
            vertexes[3].Position = new Vector3(rectangle.X + rectangle.Height, -rectangle.Y - rectangle.Width, 0.0f);
            vertexes[3].Color = color;
            int[] indexes = new []{0,1,3,2,0};
            m_device.VertexDeclaration = m_vertexDeclaration;
            m_device.RenderState.CullMode = CullMode.None;
            m_effect.Begin();
            foreach (var pass in m_effect.CurrentTechnique.Passes)
            {
                pass.Begin();
                m_device.DrawUserIndexedPrimitives(PrimitiveType.LineStrip, vertexes, 0, 4, indexes, 0, 4);
                pass.End();
            }
            m_effect.End();
        }

        public void FillRectangle(Rectangle rectangle, Color color)
        {
            var vertexes = new VertexPositionColor[4];
            vertexes[0].Position = new Vector3(rectangle.X, -rectangle.Y, 0.0f);
            vertexes[0].Color = color;
            vertexes[1].Position = new Vector3(rectangle.X, -rectangle.Y - rectangle.Width, 0.0f);
            vertexes[1].Color = color;
            vertexes[2].Position = new Vector3(rectangle.X + rectangle.Height, -rectangle.Y, 0.0f);
            vertexes[2].Color = color;
            vertexes[3].Position = new Vector3(rectangle.X + rectangle.Height, -rectangle.Y - rectangle.Width, 0.0f);
            vertexes[3].Color = color;
            m_device.VertexDeclaration = m_vertexDeclaration;
            m_device.RenderState.CullMode = CullMode.None;
            m_effect.Begin();
            foreach (var pass in m_effect.CurrentTechnique.Passes)
            {
                pass.Begin();
                m_device.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertexes, 0, 2);
                pass.End();
            }
            m_effect.End();
        }

        public void End()
        {
            m_spriteBatch.End();
        }
    }
}
