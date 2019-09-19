using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum State
    {
        Playing,
        Death
    }

    public PlayerSettings PlayerSettings;

    [SerializeField]
    private Rigidbody _rigidbody = null;
    [SerializeField]
    private Collider _collider = null;

    public State CurrentState { get; private set; }

    private void Awake()
    {
        EnableRagdoll(false);
        CurrentState = State.Playing;
    }

    // The physics collision matrix is set up so only objects 
    // that need to interact with Player will trigger this.
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == LayersAndTags.Traps)
        {
            // Kill Player
            CurrentState = State.Death;
            EnableRagdoll(true);
        }
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

    private void EnableRagdoll(bool enabled)
    {
        _rigidbody.useGravity = enabled;
        _rigidbody.isKinematic = !enabled;
        _collider.isTrigger = !enabled;
    }
}
