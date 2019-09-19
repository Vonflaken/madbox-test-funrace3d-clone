using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Player _playerScript = null;
    [SerializeField]
    private CatmullRomSpline _levelTrackSpline = null;

    public static GameManager Instance = null;

    public float LevelProgression { get; private set; } // [0-1]

    public Player PlayerScript { get { return _playerScript; } }

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (ShouldPlayerMove())
        {
            // Make the level progresses forward by the player's speed
            Vector3 currentDerivative = _levelTrackSpline.DerivativeAt(LevelProgression);
            LevelProgression += (Time.deltaTime * _playerScript.GetCurrentSpeed() / _levelTrackSpline.SpanCount) / currentDerivative.magnitude;

            _playerScript.MoveTo(_levelTrackSpline.ValueAt(LevelProgression));
        }
    }
    
    public bool ShouldPlayerMove()
    {
        // Player moves while a pointer is on screen
        return _playerScript.CurrentState == Player.State.Playing
                && LevelProgression <= 1f 
                && Input.GetMouseButton(0); // This will work in touch devices since Input.simulateMouseWithTouches flag is enabled
    }
}
