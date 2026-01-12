using System;

public class ObjectSelectEvent : EventArgs
{
    public SelectionObject SelectionObject;

    public ObjectSelectEvent(SelectionObject rewindObject)
    {
        this.SelectionObject = rewindObject;
    }
}