using System.Collections.Generic;

namespace Acrossoft.Engine.Controls
{
    public class ControlsConfig
    {
        private readonly Dictionary<int, FunctionConfig> m_controls = new Dictionary<int, FunctionConfig>();

        public FunctionConfig GetFunctionConfig(int function)
        {
            FunctionConfig result;
            if (!m_controls.TryGetValue(function, out result))
            {
                result = new FunctionConfig();
                m_controls[function] = result;
            }
            return result;
        }

        public bool IsPressed(int function, ControlsState state)
        {
            return m_controls[function].IsPressed(state);
        }

        public bool JustPressed(int function, ControlsState state)
        {
            return m_controls[function].JustPressed(state);
        }

        public bool JustReleased(int function, ControlsState state)
        {
            return m_controls[function].JustReleased(state);
        }
    }
}