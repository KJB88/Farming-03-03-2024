using UnityEngine;

public class MSG_PickedUp : Message
{
    public Transform picker;

    public MSG_PickedUp(string messageType, Transform picker) 
        : base(messageType) => this.picker = picker;
}