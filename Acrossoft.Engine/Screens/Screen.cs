using Microsoft.Xna.Framework;

namespace Acrossoft.Engine.Screens
{
    public abstract class Screen : DrawableGameComponent
    {
        protected Screen(Game game) 
            : base(game)
        {
        }
    }
}