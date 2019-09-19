using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "GameSettings/CreatePlayerSettings")]
public class PlayerSettings : ScriptableObject
{
    [Tooltip("Units per second")]
    public float Speed = 2f;
}
