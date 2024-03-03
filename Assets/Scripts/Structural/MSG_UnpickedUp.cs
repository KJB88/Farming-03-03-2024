using UnityEngine;

public class MSG_UnpickedUp : Message
{
    public Vector3 placedPos;
    public bool inDropZone = false;

    public MSG_UnpickedUp(string messageType, Vector3 placedPos, bool inDropZone) : base(messageType)
    {
        this.placedPos = placedPos;
        this.inDropZone = inDropZone;
    }
}