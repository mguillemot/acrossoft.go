namespace Acrossoft.Engine.Controls
{
    public interface IControlsProvider
    {
        ControlsState CurrentState { get; }

        ControlsConfig CurrentConfig { get;  }
    }
}