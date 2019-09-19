using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Player _playerScript = null;
    [SerializeField]
    private CatmullRomSpline _levelTrackSpline = null;

    private float _levelProgression = 0f; // [0-1]

    void Update()
    {
        if (ShouldPlayerMove())
        {
            // Make the level progresses forward by the player's speed
            Vector3 currentDerivative = _levelTrackSpline.DerivativeAt(_levelProgression);
            _levelProgression += (Time.deltaTime * _playerScript.GetCurrentSpeed() / _levelTrackSpline.SpanCount) / currentDerivative.magnitude;

            _playerScript.MoveTo(_levelTrackSpline.ValueAt(_levelProgression));
        }
    }
    
    public bool ShouldPlayerMove()
    {
        // Player moves while a pointer is on screen
        return _playerScript.CurrentState == Player.State.Playing
                && _levelProgression <= 1f 
                && Input.GetMouseButton(0); // This will work in touch devices since Input.simulateMouseWithTouches flag is enabled
    }
}
