using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TrackCameraPlaceholder
{
    public enum Type
    {
        OrbitalStatic, // Makes a on site look at to Player (surveillance camera)
        Follow // Moves following the Player at starting distance (3th person)
    }

    public Transform StartingPoint = null;
    public float ProgressionThreshold = 0f; // This camera will get enable when level progression gets over this value
    public Type CameraType = Type.Follow;
    [System.NonSerialized]
    public bool Visited = false;
}

public class CamerasController : MonoBehaviour
{
    [SerializeField]
    private Camera _playerCamera = null;
    [SerializeField]
    private float _cameraSwitchDuration = 0.5f;
    [SerializeField]
    private Easings.Functions _cameraSwitchEasing = Easings.Functions.Linear;
    [SerializeField]
    private TrackCameraPlaceholder[] _trackCameras = null;

    private TrackCameraPlaceholder _currentCameraSetting = null;
    private bool _cameraSettingSwitchOnGoing = false;
    private Vector3 _cameraSwitchStartPos = Vector3.zero;
    private Quaternion _cameraSwitchStartRot = Quaternion.identity;
    private float _cameraSwitchProgress = 0f;
    
    void Update()
    {
        foreach (var trackCamPlaceholder in _trackCameras)
        {
            if (GameManager.Instance.LevelProgression >= trackCamPlaceholder.ProgressionThreshold 
                && trackCamPlaceholder.Visited == false)
            {
                // Go next camera setting in array
                trackCamPlaceholder.Visited = true;
                TriggerCameraSettingChange(trackCamPlaceholder);
            }
        }

        if (_cameraSettingSwitchOnGoing)
        {
            UpdateCameraSettingSwitch();
        }
        else
        {
            // Run camera behaviour based on current settings
            switch (_currentCameraSetting.CameraType)
            {
                case TrackCameraPlaceholder.Type.Follow:
                    _playerCamera.transform.SetParent(GameManager.Instance.PlayerScript.transform, true);
                    break;
                case TrackCameraPlaceholder.Type.OrbitalStatic:
                    _playerCamera.transform.LookAt(GameManager.Instance.PlayerScript.transform);
                    break;
                default: break;
            }
        }
    }

    private void TriggerCameraSettingChange(TrackCameraPlaceholder newSetting)
    {
        // Prepare for a camera switch animation
        _playerCamera.transform.SetParent(null); // Remove previous parent set for not messing up with next setting
        _currentCameraSetting = newSetting;
        _cameraSettingSwitchOnGoing = true;
        _cameraSwitchStartPos = _playerCamera.transform.position;
        _cameraSwitchStartRot = _playerCamera.transform.rotation;
    }

    private void UpdateCameraSettingSwitch()
    {
        // Set new values of pos and rot for the camera based on current animation progress
        _cameraSwitchProgress += Time.deltaTime / _cameraSwitchDuration;
        float progressEased = Easings.Interpolate(_cameraSwitchProgress, _cameraSwitchEasing);
        _playerCamera.transform.position = Vector3.Lerp(_cameraSwitchStartPos, _currentCameraSetting.StartingPoint.position, progressEased);
        _playerCamera.transform.rotation = Quaternion.Slerp(_cameraSwitchStartRot, _currentCameraSetting.StartingPoint.rotation, progressEased);

        if (_cameraSwitchProgress >= 1f)
        {
            // Finish camera switch animation
            _cameraSettingSwitchOnGoing = false;
            _cameraSwitchProgress = 0f; // Reset
        }
    }
}
