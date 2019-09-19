using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerSettings PlayerSettings;

    // The physics collision matrix is set up so only objects 
    // that need to interact with Player will trigger this.
    private void OnCollisionEnter(Collision collision)
    {
        
    }

    public void MoveTo(Vector3 newWorldPosition)
    {
        transform.position = newWorldPosition;
    }

    public float GetCurrentSpeed()
    {
        // May add modifiers like perks or temporal buffs
        return PlayerSettings.Speed;
    }
}
