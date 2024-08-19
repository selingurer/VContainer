public class JoystickInput : IInputProvider
{
    private FixedJoystick _joystick;

    public JoystickInput(FixedJoystick joystick)
    {
        _joystick = joystick;
    }

    public float Horizontal => _joystick.Horizontal;
    public float Vertical => _joystick.Vertical;
}