using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using Acrossoft.GoUtils.Entities;

namespace Acrossoft.GoAi
{
    public class Ai
    {
        private readonly Random m_randomgenerator;
        private BoardEx m_boardex;

        public Ai(int size)
        {
            m_boardex = new BoardEx(size);
            m_randomgenerator = new Random();
        }

        public void Set(BoardEx boardex)
        {
            m_boardex = (BoardEx)boardex.Clone();
        }
    }
}
