namespace Acrossoft.Engine.Controls.ControlTriggers
{
    public abstract class ControlTrigger
    {
        internal abstract void Update(ControlsState state);

        public abstract bool IsTriggered { get; }
    }
}
