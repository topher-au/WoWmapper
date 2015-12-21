namespace WoWmapper.Input
{
    public delegate void ControllerConnected();

    public delegate void ControllerDisconnected();

    public delegate void ButtonDown(InputButton Button);

    public delegate void ButtonUp(InputButton Button);

    public delegate void TouchpadMoved(InputTouchpadEventArgs e);

    public class InputTouchpadEventArgs
    {
        public InputTouch[] touches;
    }
}