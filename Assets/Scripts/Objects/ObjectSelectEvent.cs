using System;

public class ObjectSelectEvent : EventArgs
{
    public SelectionObject RewindObject;

    public ObjectSelectEvent(SelectionObject rewindObject)
    {
        this.RewindObject = rewindObject;
    }
}