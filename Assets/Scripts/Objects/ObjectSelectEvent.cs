using System;

public class ObjectSelectEvent : EventArgs
{
    public RewindObject RewindObject;

    public ObjectSelectEvent(RewindObject rewindObject)
    {
        this.RewindObject = rewindObject;
    }
}