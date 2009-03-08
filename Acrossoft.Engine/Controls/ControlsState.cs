using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace Acrossoft.Engine.Controls
{
    public class ControlsState
    {
        private static readonly Buttons[] ALL_BUTTONS = new[]
                                                            {
                                                                Buttons.A,
                                                                Buttons.B,
                                                                Buttons.Back,
                                                                Buttons.BigButton,
                                                                Buttons.DPadDown,
                                                                Buttons.DPadLeft,
                                                                Buttons.DPadRight,
                                                                Buttons.DPadUp,
                                                                Buttons.LeftShoulder,
                                                                Buttons.LeftStick,
                                                                Buttons.LeftThumbstickDown,
                                                                Buttons.LeftThumbstickLeft,
                                                                Buttons.LeftThumbstickRight,
                                                                Buttons.LeftThumbstickUp,
                                                                Buttons.LeftTrigger,
                                                                Buttons.RightShoulder,
                                                                Buttons.RightStick,
                                                                Buttons.RightThumbstickDown,
                                                                Buttons.RightThumbstickLeft,
                                                                Buttons.RightThumbstickRight,
                                                                Buttons.RightThumbstickUp,
                                                                Buttons.RightTrigger,
                                                                Buttons.Start,
                                                                Buttons.X,
                                                                Buttons.Y
                                                            };


        private Dictionary<Keys, long> m_pressedKeys = new Dictionary<Keys, long>();
        private readonly List<Keys> m_justPressedKeys = new List<Keys>();
        private readonly List<Keys> m_justReleasedKeys = new List<Keys>();

        private Dictionary<Buttons, long> m_pressedButtons = new Dictionary<Buttons, long>();
        private readonly List<Buttons> m_justPressedButtons = new List<Buttons>();
        private readonly List<Buttons> m_justReleasedButtons = new List<Buttons>();

        private KeyboardState m_keyboardState;
        private GamePadState m_padState;

        public KeyboardState KeyboardState
        {
            get { return m_keyboardState; }
        }

        public Dictionary<Keys, long> PressedKeys
        {
            get { return m_pressedKeys; }
        }

        public List<Keys> JustPressedKeys
        {
            get { return m_justPressedKeys; }
        }

        public List<Keys> JustReleasedKeys
        {
            get { return m_justReleasedKeys; }
        }

        public GamePadState PadState
        {
            get { return m_padState; }
        }

        public Dictionary<Buttons, long> PressedButtons
        {
            get { return m_pressedButtons; }
        }

        public List<Buttons> JustReleasedButtons
        {
            get { return m_justReleasedButtons; }
        }

        public List<Buttons> JustPressedButtons
        {
            get { return m_justPressedButtons; }
        }

        public void ComputeState(KeyboardState keyboardState, GamePadState padState)
        {
            // Compute newly pressed & relased key set
            m_keyboardState = keyboardState;
            var previouslyPressedKeys = m_pressedKeys;
            m_pressedKeys = new Dictionary<Keys, long>();
            m_justPressedKeys.Clear();
            m_justReleasedKeys.Clear();
            foreach (Keys key in keyboardState.GetPressedKeys())
            {
                long time;
                if (previouslyPressedKeys.TryGetValue(key, out time))
                {
                    m_pressedKeys.Add(key, time);
                }
                else
                {
                    m_justPressedKeys.Add(key);
                    m_pressedKeys.Add(key, DateTime.Now.Ticks);
                }
            }
            foreach (Keys key in previouslyPressedKeys.Keys)
            {
                if (keyboardState.IsKeyUp(key))
                {
                    m_justReleasedKeys.Add(key);
                }
            }

            // Compute pad state
            m_padState = padState;
            var previouslyPressedButtons = m_pressedButtons;
            m_pressedButtons = new Dictionary<Buttons, long>();
            m_justPressedButtons.Clear();
            m_justReleasedButtons.Clear();
            if (padState.IsConnected)
            {
                foreach (Buttons button in ALL_BUTTONS)
                {
                    bool buttonDown = IsButtonDown(padState, button);
                    bool previouslyDown = previouslyPressedButtons.ContainsKey(button);
                    if (buttonDown && !previouslyDown)
                    {
                        m_pressedButtons.Add(button, DateTime.Now.Ticks);
                    }
                    else if (!buttonDown && previouslyDown)
                    {
                        m_justReleasedButtons.Add(button);
                    }
                }
            }
        }

        public static bool IsButtonDown(GamePadState state, Buttons button)
        {
            switch (button)
            {
                case Buttons.LeftThumbstickLeft:
                    return state.ThumbSticks.Left.LengthSquared() > 0.8f &&
                           state.ThumbSticks.Left.Y > state.ThumbSticks.Left.X &&
                           state.ThumbSticks.Left.Y < -state.ThumbSticks.Left.X;
                case Buttons.LeftThumbstickRight:
                    return state.ThumbSticks.Left.LengthSquared() > 0.8f &&
                           state.ThumbSticks.Left.Y < state.ThumbSticks.Left.X &&
                           state.ThumbSticks.Left.Y > -state.ThumbSticks.Left.X;
                case Buttons.LeftThumbstickDown:
                    return state.ThumbSticks.Left.LengthSquared() > 0.8f &&
                           state.ThumbSticks.Left.Y < state.ThumbSticks.Left.X &&
                           state.ThumbSticks.Left.Y < -state.ThumbSticks.Left.X;
                case Buttons.LeftThumbstickUp:
                    return state.ThumbSticks.Left.LengthSquared() > 0.8f &&
                           state.ThumbSticks.Left.Y > state.ThumbSticks.Left.X &&
                           state.ThumbSticks.Left.Y > -state.ThumbSticks.Left.X;
                case Buttons.RightThumbstickLeft:
                    return state.ThumbSticks.Right.X < -0.8f &&
                           Math.Abs(state.ThumbSticks.Right.Y) < 0.4f;
                case Buttons.RightThumbstickRight:
                    return state.ThumbSticks.Right.X > 0.8f &&
                           Math.Abs(state.ThumbSticks.Right.Y) < 0.4f;
                case Buttons.RightThumbstickDown:
                    return state.ThumbSticks.Right.Y < -0.8f &&
                           Math.Abs(state.ThumbSticks.Right.X) < 0.4f;
                case Buttons.RightThumbstickUp:
                    return state.ThumbSticks.Right.Y > 0.8f &&
                           Math.Abs(state.ThumbSticks.Right.X) < 0.4f;
                default:
                    return state.IsButtonDown(button);
            }
        }
    }
}