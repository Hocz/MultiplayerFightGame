using UnityEngine;
using Unity.Netcode.Components;

[DisallowMultipleComponent]
public class CustomClientNetworkTransform : NetworkTransform
{
    protected override bool OnIsServerAuthoritative()
    {
        return false;
    }
}
