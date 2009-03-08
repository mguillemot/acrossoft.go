using System.Collections.Generic;
using Acrossoft.Engine.Controls.ControlTriggers;
using Microsoft.Xna.Framework.Input;

namespace Acrossoft.Engine.Controls
{
    public class Function
    {
        private readonly string m_name;
        private readonly List<ControlTrigger> m_triggers = new List<ControlTrigger>();

        public Function(string name)
        {
            m_name = name;
        }

        public string Name
        {
            get { return m_name; }
        }

        public bool Triggered
        {
            get
            {
                foreach (var trigger in m_triggers)
                {
                    if (trigger.IsTriggered)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public void AssignKey(Keys key)
        {
            m_triggers.Add(new KeyTrigger(key));
        }

        public void AssignKey(Keys key, int autoInitialDelay, int autoRepeatDelay)
        {
            m_triggers.Add(new RepeatableKeyTrigger(key, autoInitialDelay, autoRepeatDelay));
        }

        public void AssignButton(Buttons button)
        {
            m_triggers.Add(new ButtonTrigger(button));
        }

        public void AssignButton(Buttons button, int autoInitialDelay, int autoRepeatDelay)
        {
            m_triggers.Add(new RepeatableButtonTrigger(button, autoInitialDelay, autoRepeatDelay));
        }

        internal void Update(ControlsState state)
        {
            foreach (var trigger in m_triggers)
            {
                trigger.Update(state);
            }
        }
    }
}