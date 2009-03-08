using Acrossoft.Engine.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Acrossoft.Engine.Console
{
    public class ConsoleComponent : DrawableGameComponent
    {
        private readonly ConsoleDisplay m_consoleDisplay = new ConsoleDisplay();
        private bool m_consoleVisible = false;
        private Function m_consoleToggleFunction;

        public ConsoleComponent(Game game) 
            : base(game)
        {
        }

        protected override void LoadContent()
        {
            ConsoleDisplay.LoadContent(Game.Content);
        }

        public override void Initialize()
        {
            base.Initialize();

            // Bind par défaut des fouches
            m_consoleToggleFunction = new Function("console");
            m_consoleToggleFunction.AssignKey(Keys.Tab);
            m_consoleToggleFunction.AssignButton(Buttons.Start);
            var controlsProvider = (IControlsProvider)Game.Services.GetService(typeof(IControlsProvider));
            controlsProvider.CurrentConfig.RegisterFunction(m_consoleToggleFunction);
            
            // Initialisation du display
            ConsoleDisplay.LoadContent(Game.Content);
            m_consoleDisplay.Initialize(GraphicsDevice);
            
            // Configuraiton de la console
            Console.LineWidth = m_consoleDisplay.CharacterWidth;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            if (m_consoleVisible)
            {
                m_consoleDisplay.Draw();
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (m_consoleToggleFunction.Triggered)
            {
                m_consoleVisible = !m_consoleVisible;
            }
        }
    }
}
