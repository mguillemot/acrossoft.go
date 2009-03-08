using System;
using Microsoft.Xna.Framework.Input;

namespace Acrossoft.Engine.Controls.ControlTriggers
{
    public class RepeatableButtonTrigger : ControlTrigger
    {
        private readonly Buttons m_button;
        private readonly int m_initialDelayTicks;
        private readonly int m_repeatDelayTicks;

        private long m_repeatStart = -1;
        private bool m_triggered;

        public RepeatableButtonTrigger(Buttons button, int initialDelay, int repeatDelay)
        {
            m_button = button;
            m_initialDelayTicks = initialDelay*10000;
            m_repeatDelayTicks = repeatDelay*10000;
        }

        internal override void Update(ControlsState state)
        {
            m_triggered = state.JustPressedButtons.Contains(m_button);
            if (state.PressedButtons.ContainsKey(m_button))
            {
                long now = DateTime.Now.Ticks;
                long pressed = state.PressedButtons[m_button];
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