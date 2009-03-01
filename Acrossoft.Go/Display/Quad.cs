using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Acrossoft.Go.Display
{
    public class Quad
    {
        private readonly VertexPositionNormalTexture[] m_vertices;
        private readonly int[] m_indices;

        public Quad(Vector3 origin, Vector3 normal, Vector3 up, float width, float height)
        {
            m_vertices = new VertexPositionNormalTexture[4];
            m_indices = new []{0, 1, 2, 2, 1, 3};

            // Calculate the quad corners
            Vector3 left = Vector3.Cross(normal, up);
            Vector3 uppercenter = (up * height / 2) + origin;
            Vector3 UpperLeft = uppercenter + (left * width / 2);
            Vector3 UpperRight = uppercenter - (left * width / 2);
            Vector3 LowerLeft = UpperLeft - (up * height);
            Vector3 LowerRight = UpperRight - (up * height);

            Vector2 textureUpperLeft = new Vector2(0.0f, 0.0f);
            Vector2 textureUpperRight = new Vector2(1.0f, 0.0f);
            Vector2 textureLowerLeft = new Vector2(0.0f, 1.0f);
            Vector2 textureLowerRight = new Vector2(1.0f, 1.0f);
            m_vertices[0].Position = LowerLeft;
            m_vertices[0].TextureCoordinate = textureLowerLeft;
            m_vertices[1].Position = UpperLeft;
            m_vertices[1].TextureCoordinate = textureUpperLeft;
            m_vertices[2].Position = LowerRight;
            m_vertices[2].TextureCoordinate = textureLowerRight;
            m_vertices[3].Position = UpperRight;
            m_vertices[3].TextureCoordinate = textureUpperRight;
        }

        public VertexPositionNormalTexture[] Vertices
        {
            get { return m_vertices; }
        }

        public int[] Indices
        {
            get { return m_indices; }
        }
    }
}
