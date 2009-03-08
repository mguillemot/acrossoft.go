using System;
using Microsoft.Xna.Framework.Input;

namespace Acrossoft.Engine.Controls.ControlTriggers
{
    public class RepeatableKeyTrigger : ControlTrigger
    {
        private readonly Keys m_key;
        private readonly int m_initialDelayTicks;
        private readonly int m_repeatDelayTicks;

        private long m_repeatStart = -1;
        private bool m_triggered;

        public RepeatableKeyTrigger(Keys key, int initialDelay, int repeatDelay)
        {
            m_key = key;
            m_initialDelayTicks = initialDelay*10000;
            m_repeatDelayTicks = repeatDelay*10000;
        }

        internal override void Update(ControlsState state)
        {
            m_triggered = state.JustPressedKeys.Contains(m_key);
            if (state.PressedKeys.ContainsKey(m_key))
            {
                long now = DateTime.Now.Ticks;
                long pressed = state.PressedKeys[m_key];
                if (m_repeatStart == -1 && now - pressed > m_initialDelayTicks)
                {
                    // initial trigger
                    m_triggered = true;
                    m_repeatStart = pressed + m_initialDelayTicks;
                }
                else if (m_repeatStart != -1 && now - m_repeatStart > m_repeatDelayTicks)
                {
                    // repeat trigger
                    m_triggered = true;
                    m_repeatStart += m_repeatDelayTicks;
                }
            }
            else
            {
                m_repeatStart = -1;
            }
        }

        public override bool IsTriggered
        {
            get { return m_triggered; }
        }
    }
}