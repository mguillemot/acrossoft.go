using System.Collections.Generic;

namespace Acrossoft.Engine.Controls
{
    public class ControlsConfig
    {
        private readonly List<Function> m_functions = new List<Function>();

        public void RegisterFunction(Function function)
        {
            m_functions.Add(function);
        }

        public void UnregisterFunction(Function function)
        {
            m_functions.Remove(function);
        }

        public void UpdateState(ControlsState state)
        {
            foreach (var function in m_functions)
            {
                function.Update(state);
            }
        }
    }
}