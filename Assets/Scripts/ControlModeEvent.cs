using System;

public class ControlModeEvent : EventArgs
{
    public bool State;

    public ControlModeEvent(bool state)
    {
        this.State = state;
    }
}