using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace Acrossoft.Engine.Controls
{
    public class FunctionConfig
    {
        private readonly List<Keys> m_keys = new List<Keys>();
        private readonly List<Buttons> m_buttons = new List<Buttons>();

        public void AddKey(Keys key)
        {
            if (!m_keys.Contains(key))
            {
                m_keys.Add(key);
            }
        }

        public void RemoveKey(Keys key)
        {
            m_keys.Remove(key);
        }

        public void AddButton(Buttons button)
        {
            if (!m_buttons.Contains(button))
            {
                m_buttons.Add(button);
            }
        }

        public void RemoveButton(Buttons button)
        {
            m_buttons.Remove(button);
        }

        internal bool IsPressed(ControlsState state)
        {
            foreach (var key in m_keys)
            {
                if (state.KeyboardState.IsKeyDown(key))
                {
                    return true;
                }
            }
            foreach (var button in m_buttons)
            {
                if (ControlsState.IsButtonDown(state.PadState, button))
                {
                    return true;
                }
            }
            return false;
        }

        internal bool JustPressed(ControlsState state)
        {
            foreach (var key in m_keys)
            {
                if (state.PressedKeys.Contains(key))
                {
                    return true;
                }
            }
            foreach (var button in m_buttons)
            {
                if (state.PressedButtons.Contains(button))
                {
                    return true;
                }
            }
            return false;
        }

        internal bool JustReleased(ControlsState state)
        {
            foreach (var key in m_keys)
            {
                if (state.ReleasedKeys.Contains(key))
                {
                    return true;
                }
            }
            foreach (var button in m_buttons)
            {
                if (state.ReleasedButtons.Contains(button))
                {
                    return true;
                }
            }
            return false;
        }
    }
}