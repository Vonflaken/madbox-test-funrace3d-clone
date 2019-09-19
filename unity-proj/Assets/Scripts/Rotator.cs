using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField]
    private Transform _target = null;
    [SerializeField, Tooltip("In degrees per second")]
    private float _speed = 45f;
    [SerializeField]
    private Vector3 _axis = Vector3.up;

    private void Update()
    {
        float deltaSpeed = _speed * Time.deltaTime;
        transform.rotation *= Quaternion.Euler(deltaSpeed * _axis.x, deltaSpeed * _axis.y, deltaSpeed * _axis.z);
    }
}
