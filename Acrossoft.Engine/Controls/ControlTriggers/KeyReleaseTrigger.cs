using Microsoft.Xna.Framework.Input;

namespace Acrossoft.Engine.Controls.ControlTriggers
{
    public class KeyReleaseTrigger : ControlTrigger
    {
        private readonly Keys m_key;

        private bool m_triggered;

        public KeyReleaseTrigger(Keys key)
        {
            m_key = key;
        }

        internal override void Update(ControlsState state)
        {
            m_triggered = state.JustReleasedKeys.Contains(m_key);
        }

        public override bool IsTriggered
        {
            get { return m_triggered; }
        }
    }
}
