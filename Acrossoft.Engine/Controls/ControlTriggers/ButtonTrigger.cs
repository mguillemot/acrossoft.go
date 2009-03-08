using Microsoft.Xna.Framework.Input;

namespace Acrossoft.Engine.Controls.ControlTriggers
{
    public class ButtonTrigger : ControlTrigger
    {
        private readonly Buttons m_button;

        private bool m_triggered;

        public ButtonTrigger(Buttons button)
        {
            m_button = button;
        }

        internal override void Update(ControlsState state)
        {
            m_triggered = state.JustPressedButtons.Contains(m_button);
        }

        public override bool IsTriggered
        {
            get { return m_triggered; }
        }
    }
}
