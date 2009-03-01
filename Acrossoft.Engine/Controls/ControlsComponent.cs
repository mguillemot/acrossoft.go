using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Acrossoft.Engine.Controls
{
    public class ControlsComponent : GameComponent, IControlsProvider
    {
        private readonly ControlsConfig m_controlsConfig = new ControlsConfig();
        private readonly ControlsState m_controlsState = new ControlsState();

        public ControlsComponent(Game game) 
            : base(game)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            // Register as a service
            Game.Services.AddService(typeof(IControlsProvider), this);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            var padState = GamePad.GetState(PlayerIndex.One);
            var keyboardState = Keyboard.GetState(PlayerIndex.One);
            m_controlsState.ComputeState(keyboardState, padState);
        }

        public ControlsState CurrentState
        {
            get { return m_controlsState; }
        }

        public ControlsConfig CurrentConfig
        {
            get { return m_controlsConfig; }
        }
    }
}
